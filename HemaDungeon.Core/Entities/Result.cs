namespace HemaDungeon.Core.Entities;

public sealed class Result
{
    public string Id { get; set; }

    public DateTime CreateDate { get; set; }

    public Character First { get; set; }

    public int FirstScore { get; set; }

    public Character Second { get; set; }

    public int SecondScore { get; set; }

    public DateTime DateTime { get; set; }
}