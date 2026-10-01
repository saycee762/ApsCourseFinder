using ApsCourseFinder.Models;

namespace ApsCourseFinder.Services;

/// <summary>
/// CUT rules (from the Subject Selection Guide): only seven subjects count towards the APS,
/// and Life Orientation counts as 1 point. So APS = best six other subjects + Life Orientation.
/// </summary>
public class ApsCalculator
{
    public const int MaxSubjects = 7;
    public const int LifeOrientationPoints = 1;
    public const int LifeOrientationPassMark = 40;

    /// <summary>NSC achievement level (1-7) for a percentage.</summary>
    public static int Level(int mark) => mark switch
    {
        >= 80 => 7,
        >= 70 => 6,
        >= 60 => 5,
        >= 50 => 4,
        >= 40 => 3,
        >= 30 => 2,
        _ => 1
    };

    public ApsResult Calculate(IReadOnlyDictionary<string, int> marks)
    {
        var scored = marks.Select(kv => (Subject: kv.Key, Mark: kv.Value, Points: PointsFor(kv.Key, kv.Value))).ToList();

        var counted = scored
            .Where(s => s.Subject != Subjects.LO)
            .OrderByDescending(s => s.Points).ThenByDescending(s => s.Mark)
            .Take(MaxSubjects - 1)
            .Select(s => s.Subject)
            .ToHashSet();
        counted.Add(Subjects.LO);

        var lines = scored.Select(s => new ApsLine(s.Subject, s.Mark, s.Points, counted.Contains(s.Subject))).ToList();
        return new ApsResult(lines.Where(l => l.Counted).Sum(l => l.Points), lines);
    }

    private static int PointsFor(string subject, int mark) =>
        subject == Subjects.LO
            ? (mark >= LifeOrientationPassMark ? LifeOrientationPoints : 0)
            : Level(mark);
}
