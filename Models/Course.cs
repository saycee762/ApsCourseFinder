namespace ApsCourseFinder.Models;

public enum Faculty { Health, Engineering, Management, Humanities }

public static class FacultyExtensions
{
    public static string Title(this Faculty f) => f switch
    {
        Faculty.Health => "Health & Environmental Science",
        Faculty.Engineering => "Engineering, Built Environment & IT",
        Faculty.Management => "Management Sciences",
        _ => "Humanities"
    };

    public static string Css(this Faculty f) => f.ToString().ToLowerInvariant();
}

/// <summary>A subject and the minimum mark (%) needed in it.</summary>
public record SubjectOption(string Subject, int MinMark);

/// <summary>
/// One entry in a course's "subject combination". It is met when the learner satisfies
/// at least <see cref="Count"/> of the options in <see cref="AnyOf"/>.
/// All of a course's requirements must be met.
/// </summary>
public record Requirement(IReadOnlyList<SubjectOption> AnyOf, int Count = 1, string? Label = null)
{
    /// <summary>One specific subject, e.g. Mathematics 50%.</summary>
    public static Requirement One(string subject, int min) => new(new[] { new SubjectOption(subject, min) });

    /// <summary>Any one of these subjects at the same minimum mark.</summary>
    public static Requirement Any(int min, params string[] subjects) =>
        new(subjects.Select(s => new SubjectOption(s, min)).ToArray());

    /// <summary>Any one of these subjects, each with its own minimum, e.g. Maths 40% or Maths Lit 60%.</summary>
    public static Requirement Mixed(params (string Subject, int Min)[] options) =>
        new(options.Select(o => new SubjectOption(o.Subject, o.Min)).ToArray());

    /// <summary>At least <paramref name="count"/> of these subjects at the same minimum mark.</summary>
    public static Requirement AtLeast(int count, int min, params string[] subjects) =>
        new(subjects.Select(s => new SubjectOption(s, min)).ToArray(), count);

    public string Describe() =>
        Label ?? (Count > 1
            ? $"Any {Count} of: " + string.Join(", ", AnyOf.Select(o => $"{o.Subject} {o.MinMark}%"))
            : string.Join(" or ", AnyOf.Select(o => $"{o.Subject} {o.MinMark}%")));
}

public class Course
{
    public required Faculty Faculty { get; init; }
    public required string Name { get; init; }
    public required string Field { get; init; }
    public int? MinAps { get; init; }
    public string Duration { get; init; } = "Not stated";
    public string Campus { get; init; } = "Bloemfontein";
    public string Careers { get; init; } = "";
    public IReadOnlyList<Requirement> Requirements { get; init; } = Array.Empty<Requirement>();
    /// <summary>Set when every subject must reach this mark (e.g. Design and Studio Art).</summary>
    public int? AllSubjectsMin { get; init; }
    public string? Note { get; init; }
}
