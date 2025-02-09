namespace HemaDungeon.Calculator.OptionalModificators;

public sealed class SmallModificator : IModificator
{
    public int Priority => 1447;
    
    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Волшебный народ", out var spell)) 
            spell = new Character.Spell("Может убедить противников, какой поединок им предстоит на арене", 1, "P");
        character.Spells["Волшебный народ"] = spell;
    }
}