namespace HemaDungeon.Entities;

public sealed class FightState
{
    public string Id { get; set; }

    public FightCharacter Character { get; set; }

    public int ScoreHealth { get; set; }

    public double Damage { get; set; }
}