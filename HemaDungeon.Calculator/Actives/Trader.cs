namespace HemaDungeon.Calculator.Actives;

public sealed class Trader : IModificator
{
    public int Priority => int.MaxValue;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Хитрый торгаш", out var spell))
            spell = new Character.Spell("За принесенную пачку кукурузных палочек переживает смертельный урон с 1 ХП", 0, "P");
        if (spell.Value > 0) character.HealthAfter = Math.Max(character.HealthAfter, 1);
        character.Spells["Хитрый торгаш"] = spell;
    }
}