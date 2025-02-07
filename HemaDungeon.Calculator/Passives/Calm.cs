namespace HemaDungeon.Calculator.Passives;

public sealed class Calm : IModificator
{
    public int Priority => int.MaxValue - 10;
    
    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Спокойствие самурая", out var spell)) 
            spell = new Character.Spell("Начинает поединок с 30 секундной медитации. Дает 5% урона", 0, "P");
        if (spell.Value > 0) character.Damage *= 1.05;
        character.Spells["Спокойствие самурая"] = spell;
    }
}