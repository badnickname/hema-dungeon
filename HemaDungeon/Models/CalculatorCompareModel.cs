namespace HemaDungeon.Models;

public sealed class CalculatorCompareModel
{
    public User FirstUser { get; set; }

    public User SecondUser { get; set; }
 
    public sealed record User(string Id, int? Health, int? Score, ICollection<Spell>? Spells);

    public sealed record Spell(string Key, string Description, int Value, string Type);
}