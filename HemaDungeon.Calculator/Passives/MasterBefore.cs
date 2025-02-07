namespace HemaDungeon.Calculator.Passives;

internal sealed class MasterBefore : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Мастер", out var spell)) 
            spell = new Character.Spell("Увеличивает статы в 2 раза", 1, "P");
        if (spell.Value > 0)
        {
            character.Agility = Math.Min(100f, 2f * character.Agility);
            character.Stamina = Math.Min(100f, 2f * character.Stamina);
            character.Power = Math.Min(100f, 2f * character.Power);
            character.Wisdom = Math.Min(100f, 2f * character.Wisdom);
        }
        character.Spells["Мастер"] = spell;
    }
}