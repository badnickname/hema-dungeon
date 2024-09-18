namespace HemaDungeon.Models;

public sealed class VisitModel
{
    public DateTime DateTime { get; set; }

    public string[]? Names { get; set; }

    public string[]? SkipNames { get; set; }
}