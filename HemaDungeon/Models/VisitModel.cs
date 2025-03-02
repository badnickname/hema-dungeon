namespace HemaDungeon.Models;

public sealed class VisitModel
{
    public DateTime DateTime { get; set; }

    public string Region { get; set; }

    public string[]? Ids { get; set; }

    public string[]? SkipIds { get; set; }
}