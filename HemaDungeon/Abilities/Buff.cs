namespace HemaDungeon.Abilities;

public sealed class Buff
{
    public int Agility { get; set; }

    public int Damage { get; set; }

    public float ResistFactor { get; set; }

    public bool CopyStats { get; set; }

    public int Wisdom { get; set; }
    
    public bool Calculated { get; set; }
}