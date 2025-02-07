namespace HemaDungeon.Calculator.Passives;

public sealed class Bubble : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Бабл", out var spell)) 
            spell = new Character.Spell("Каждый 3 удар противника не наносит урон", 1, "P");
        if (spell.Value > 0) enemy.Hits -= enemy.Hits / 3;
        character.Spells["Бабл"] = spell;
    }
}