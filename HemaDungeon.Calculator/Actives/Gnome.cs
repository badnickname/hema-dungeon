namespace HemaDungeon.Calculator.Actives;

internal sealed class Gnome : IModificator
{
    public int Priority => int.MaxValue - 14;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Гномьи технологии", out var spell)) 
            spell = new Character.Spell("Оружие, на 10 см ниже максимальной допустимой длины наносит дополнительно 10 ед. урона при попадании", 1, "P");
        if (spell.Value > 0) character.Damage += 10;
        character.Spells["Гномьи технологии"] = spell;
    }
}