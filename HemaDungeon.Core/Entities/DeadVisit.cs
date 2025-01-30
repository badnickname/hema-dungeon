namespace HemaDungeon.Core.Entities;

public sealed class DeadVisit
{
    public string Id { get; set; }

    public DeadCharacter DeadCharacter { get; set; }

    public DateTime? Date { get; set; }

    public bool CanSkip { get; set; }
    
    public bool WasHere { get; set; }
}