namespace HemaDungeon.Calculator.Passives;

internal sealed class EmptyAbility : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Пусто", out var spell)) 
            spell = new Character.Spell("Он ничего не умеет, потому что Богдан не добавил ему скилл", 0, "P");
        character.Spells["Пусто"] = spell;
    }
}