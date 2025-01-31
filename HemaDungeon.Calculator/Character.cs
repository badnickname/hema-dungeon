namespace HemaDungeon.Calculator;

public sealed class Character
{
    public Character(double health, double damage, double wisdom, double stamina, double agility, double power, AbilityType ability, int rang, int tournamentsCount, int league)
    {
        Health = health;
        Damage = damage;
        Wisdom = wisdom;
        Stamina = stamina;
        Agility = agility;
        Power = power;
        Ability = ability;
        Rang = rang;
        League = league;
        TournamentsCount = tournamentsCount;
    }

    public int Rang { get; set; }

    public int League { get; set; }

    public double Health { get; set; }

    public double Damage { get; set; }

    public double Wisdom { get; set; }

    public double Stamina { get; set; }

    public double Agility { get; set; }

    public double Power { get; set; }

    public int ScoreHealth { get; set; }

    public bool? Force { get; set; }

    public bool IsPassive { get; set; }
    
    public int TournamentsCount { get; set; }

    public int Hits { get; set; }
    
    public double HealthAfter { get; set; }

    public AbilityType Ability { get; set; }

    internal IList<IModificator> List { get; set; } = new List<IModificator>();
}