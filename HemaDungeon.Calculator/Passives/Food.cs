namespace HemaDungeon.Calculator.Passives;

internal sealed class Food : IModificator
{
    public int Priority => 10;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Корм энергичных кошек", out var spell)) 
            spell = new Character.Spell("Если выносливость Кота Бориса больше, чем выносливость противника, добавь +10 к урону", 1, "P");
        if (spell.Value > 0)
        {
            if (character.Stamina > enemy.Stamina) character.Damage += 10;
        }
        character.Spells["Корм энергичных кошек"] = spell;
    }
}