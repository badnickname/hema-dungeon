namespace HemaDungeon.Calculator.Passives;

public class Workaholic : IModificator
{
    public int Priority => int.MaxValue - 10;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Бесцельность бытия", out var spell)) 
            spell = new Character.Spell("При посещении за последние 5 дней менее 5 тренировок урон увеличивается вдвое", 0, "P");
        if (spell.Value > 0)
        {
            character.Damage *= 2;
        }
        character.Spells["Бесцельность бытия"] = spell;
    }
}