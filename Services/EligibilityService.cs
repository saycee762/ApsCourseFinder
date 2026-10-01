using ApsCourseFinder.Models;

namespace ApsCourseFinder.Services;

public class EligibilityService(CourseCatalog catalog)
{
    private const int AlmostApsGap = 3;      
    private const int AlmostMarkGap = 10;    

    public List<CourseMatch> Evaluate(IReadOnlyDictionary<string, int> marks, int aps) =>
        catalog.Courses.Select(c => Evaluate(c, marks, aps)).ToList();

    private static CourseMatch Evaluate(Course course, IReadOnlyDictionary<string, int> marks, int aps)
    {
        var checks = new List<Check>();

        if (course.MinAps is int min)
            checks.Add(new Check($"APS of {min} or more", aps >= min, $"You have {aps}", min - aps, IsAps: true));

        foreach (var req in course.Requirements)
            checks.Add(CheckRequirement(req, marks));

        if (course.AllSubjectsMin is int all && marks.Count > 0)
        {
            var low = marks.MinBy(kv => kv.Value);
            var met = low.Value >= all;
            checks.Add(new Check($"{all}% or more in every subject", met,
                met ? "All your subjects reach it" : $"Lowest is {low.Key} at {low.Value}%", all - low.Value));
        }

        var unmet = checks.Where(c => !c.Met).ToList();
        var status = unmet.Count == 0 ? MatchStatus.Qualifies
            : unmet.Count == 1 && unmet[0].Gap <= (unmet[0].IsAps ? AlmostApsGap : AlmostMarkGap) ? MatchStatus.Almost
            : MatchStatus.NotYet;

        return new CourseMatch { Course = course, Status = status, Checks = checks };
    }

    private static Check CheckRequirement(Requirement req, IReadOnlyDictionary<string, int> marks)
    {
        var passed = req.AnyOf
            .Where(o => marks.TryGetValue(o.Subject, out var m) && m >= o.MinMark)
            .Select(o => o.Subject).Distinct().ToList();

        if (passed.Count >= req.Count)
            return new Check(req.Describe(), true, string.Join(", ", passed.Take(req.Count).Select(s => $"{s} {marks[s]}%")), 0);

        var taken = req.AnyOf.Where(o => marks.ContainsKey(o.Subject)).ToList();
        var detail = taken.Count > 0
            ? "You have " + string.Join(", ", taken.Select(o => $"{o.Subject} {marks[o.Subject]}%"))
            : "No matching subject entered";
        var gap = taken.Count > 0 ? taken.Min(o => o.MinMark - marks[o.Subject]) : int.MaxValue;
        return new Check(req.Describe(), false, detail, gap);
    }
}
