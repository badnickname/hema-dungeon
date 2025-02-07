namespace HemaDungeon.Calculator.Actives;

public sealed class Imba : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Имба", out var spell)) 
            spell = new Character.Spell("Урон нанесенный при обоюдках не проходит по Витгарду", 0, "C");
        enemy.Hits = Math.Max(0, enemy.Hits - spell.Value);
        character.Spells["Имба"] = spell;
    }
}