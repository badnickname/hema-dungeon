namespace HemaDungeon.Calculator.Passives;

internal sealed class AirBender : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Маг воздуха", out var spell)) 
            spell = new Character.Spell("Ловкость увеличена на 10", 1, "P");
        if (spell.Value > 0)
        {
            character.Agility += 10;
        }
        character.Spells["Маг воздуха"] = spell;
    }
}