using ApsCourseFinder.Models;
using ApsCourseFinder.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApsCourseFinder.Controllers;

public class HomeController(ApsCalculator calculator, EligibilityService eligibility, CourseCatalog catalog) : Controller
{
    private const int DefaultRows = 7;

    [HttpGet]
    public IActionResult Index() => View(new FinderInput { Marks = BlankRows() });

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Index(FinderInput input)
    {
        var rows = (input.Marks ?? new()).Where(r => !string.IsNullOrWhiteSpace(r.Subject) || r.Mark.HasValue).ToList();
        var marks = new Dictionary<string, int>();
        var errors = new List<string>();

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.Subject) || !Subjects.All.Contains(row.Subject))
            {
                AddOnce(errors, "Choose a subject for every mark you enter.");
                continue;
            }
            if (row.Mark is not int mark || mark < 0 || mark > 100)
            {
                AddOnce(errors, $"Enter a mark from 0 to 100 for {row.Subject}.");
                continue;
            }
            if (!marks.TryAdd(row.Subject, mark))
                AddOnce(errors, $"{row.Subject} was entered more than once.");
        }

        if (marks.Count == 0 && errors.Count == 0)
            errors.Add("Add at least one subject and its mark.");

        if (errors.Count > 0)
        {
            foreach (var e in errors) ModelState.AddModelError(string.Empty, e);
            input.Marks = rows.Count > 0 ? rows : BlankRows();
            return View(input);
        }

        var aps = calculator.Calculate(marks);
        var model = new ResultsViewModel { Aps = aps, Matches = eligibility.Evaluate(marks, aps.Total) };

        if (marks.Count < ApsCalculator.MaxSubjects)
            model.Warnings.Add($"You entered {marks.Count} subjects. Your final APS uses seven, so it may be higher once you add the rest.");
        if (!marks.ContainsKey(Subjects.LO))
            model.Warnings.Add("Life Orientation was not entered. CUT counts it as 1 point towards your APS.");

        return View("Results", model);
    }

    /// <summary>Browse and search every CUT programme (no marks needed).</summary>
    [HttpGet]
    public IActionResult Courses(string? q, Faculty? faculty)
    {
        var courses = catalog.Courses.AsEnumerable();
        if (faculty is Faculty f) courses = courses.Where(c => c.Faculty == f);
        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            courses = courses.Where(c =>
                c.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                c.Field.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                c.Careers.Contains(term, StringComparison.OrdinalIgnoreCase));
        }
        return View(new CourseBrowseViewModel { Query = q, Faculty = faculty, Courses = courses.ToList() });
    }

    private static List<SubjectMarkInput> BlankRows() =>
        Enumerable.Range(0, DefaultRows).Select(_ => new SubjectMarkInput()).ToList();

    private static void AddOnce(List<string> list, string message)
    {
        if (!list.Contains(message)) list.Add(message);
    }
}
