namespace HemaDungeon.Core.Entities;

public sealed class FightState
{
    public string Id { get; set; }

    public FightCharacter Character { get; set; }

    public int ScoreHealth { get; set; }

    public double Damage { get; set; }

    public string AuthorId { get; set; }

    public bool Calculated { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}