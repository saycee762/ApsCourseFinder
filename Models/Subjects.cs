namespace ApsCourseFinder.Models;

/// <summary>Every subject a learner can pick, plus named constants used by the course catalog.</summary>
public static class Subjects
{
    public const string English = "English", Afrikaans = "Afrikaans", LO = "Life Orientation",
        Maths = "Mathematics", MathLit = "Mathematical Literacy", TechMaths = "Technical Mathematics",
        PhysSci = "Physical Sciences", TechSci = "Technical Sciences", LifeSci = "Life Sciences",
        AgriSci = "Agricultural Sciences", Accounting = "Accounting", Economics = "Economics",
        BusStudies = "Business Studies", IT = "Information Technology",
        EGD = "Engineering Graphics & Design";

    public static readonly string[] Languages =
    {
        English, Afrikaans, "isiZulu", "isiXhosa", "Sesotho", "Sepedi",
        "Setswana", "Siswati", "isiNdebele", "Tshivenda", "Xitsonga"
    };

    /// <summary>Grouped for the dropdown (a list, so the order is guaranteed).</summary>
    public static readonly IReadOnlyList<(string Group, string[] Items)> Groups = new (string, string[])[]
    {
        ("Languages", Languages),
        ("Mathematics & Sciences", new[] { Maths, MathLit, TechMaths, PhysSci, TechSci, LifeSci }),
        ("Commerce", new[] { Accounting, BusStudies, Economics }),
        ("Agriculture", new[] { AgriSci, "Agricultural Technology", "Agricultural Management Practices" }),
        ("Humanities & Arts", new[] { "History", "Geography", "Religion Studies", "Dance Studies", "Design", "Dramatic Arts", "Music", "Visual Arts" }),
        ("Technology & Services", new[] { EGD, "Civil Technology", "Electrical Technology", "Mechanical Technology", "Computer Applications Technology", IT, "Consumer Studies", "Hospitality Studies", "Tourism" }),
        ("Compulsory", new[] { LO }),
    };

    public static readonly HashSet<string> All = Groups.SelectMany(g => g.Items).ToHashSet();
}
