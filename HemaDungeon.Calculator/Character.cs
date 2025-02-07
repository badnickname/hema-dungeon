namespace HemaDungeon.Calculator;

public sealed class Character
{
    public Character(double health, double maxHealth, double damage, double wisdom, double stamina, double agility, double power, AbilityType ability, int rang, int tournamentsCount, int league, IDictionary<string, Spell> spells)
    {
        Health = health;
        MaxHealth = maxHealth;
        Damage = damage;
        Wisdom = wisdom;
        Stamina = stamina;
        Agility = agility;
        Power = power;
        Ability = ability;
        Rang = rang;
        League = league;
        TournamentsCount = tournamentsCount;
        Spells = spells;
    }

    public int Rang { get; set; }

    public int League { get; set; }

    public double Health { get; set; }

    public double MaxHealth { get; set; }

    public double Damage { get; set; }

    public double Wisdom { get; set; }

    public double Stamina { get; set; }

    public double Agility { get; set; }

    public double Power { get; set; }

    public int ScoreHealth { get; set; }

    public int TournamentsCount { get; set; }

    public int Hits { get; set; }
    
    public double HealthAfter { get; set; }

    public AbilityType Ability { get; set; }

    internal IList<IModificator> List { get; set; } = new List<IModificator>();

    public IDictionary<string, Spell> Spells { get; set; }

    public sealed record Spell(string Description, int Value, string Type);
}