namespace HemaDungeon.Entities;

public sealed class Visit
{
    public string Id { get; set; }

    public Character Character { get; set; }

    public DateTime? Date { get; set; }

    public bool WasHere { get; set; }
}