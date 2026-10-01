namespace ApsCourseFinder.Models;

public class SubjectMarkInput
{
    public string? Subject { get; set; }
    public int? Mark { get; set; }
}

public class FinderInput
{
    public List<SubjectMarkInput>? Marks { get; set; }
}

public record ApsLine(string Subject, int Mark, int Points, bool Counted);
public record ApsResult(int Total, IReadOnlyList<ApsLine> Lines);

public enum MatchStatus { Qualifies, Almost, NotYet }

/// <param name="Gap">How far short the learner is (marks, or APS points). Used to spot "almost there".</param>
public record Check(string Label, bool Met, string Detail, int Gap, bool IsAps = false);

public class CourseMatch
{
    public required Course Course { get; init; }
    public required MatchStatus Status { get; init; }
    public required IReadOnlyList<Check> Checks { get; init; }
}

public class ResultsViewModel
{
    public required ApsResult Aps { get; init; }
    public required IReadOnlyList<CourseMatch> Matches { get; init; }
    public List<string> Warnings { get; init; } = new();
}

public class CourseBrowseViewModel
{
    public string? Query { get; init; }
    public Faculty? Faculty { get; init; }
    public required IReadOnlyList<Course> Courses { get; init; }
}
