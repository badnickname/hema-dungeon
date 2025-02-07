namespace HemaDungeon.Models;

public sealed class CalculatorCompareResult
{
    public string Id { get; set; }

    public double Health { get; set; }

    public int ScoreHealth { get; set; }

    public double Damage { get; set; }

    public ICollection<Spell> Spells { get; set; }

    public sealed record Spell(string Key, string Description, int Value, string Type);
}