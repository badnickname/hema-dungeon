namespace HemaDungeon.Calculator.Actives;

public sealed class GiantSlayer : IModificator
{
    public int Priority => int.MaxValue - 13;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Убийца великанов", out var spell)) 
            spell = new Character.Spell("Каждый 5 удар подряд (3 - для гигантов) наносит урон равный 20% от максимального ХП цели", 0, "C");
        enemy.Health -= enemy.MaxHealth * 0.2f * spell.Value;
        character.Spells["Убийца великанов"] = spell;
    }
}