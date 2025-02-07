namespace HemaDungeon.Calculator.Passives;

internal sealed class Armor : IModificator
{
    public int Priority => int.MaxValue - 10;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Стальная броня", out var spell)) 
            spell = new Character.Spell("Снижает получаемый урон на 20%. На механизмы не действуют яды", 1, "P");
        if (spell.Value > 0) enemy.Damage *= 0.8f;
        character.Spells["Стальная броня"] = spell;
    }
}