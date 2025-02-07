namespace HemaDungeon.Calculator.Passives;

internal sealed class Wisdom : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Мудрость предков", out var spell)) 
            spell = new Character.Spell("Мудрость увеличена на 10", 1, "P");
        if (spell.Value > 0)
        {
            character.Wisdom += 10;
        }
        character.Spells["Мудрость предков"] = spell;
    }
}