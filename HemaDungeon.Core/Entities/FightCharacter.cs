namespace HemaDungeon.Core.Entities;

public sealed class FightCharacter
{
    public string Id { get; set; }

    public Character Character { get; set; }

    public double Health { get; set; }

    public string AuthorId { get; set; }
}