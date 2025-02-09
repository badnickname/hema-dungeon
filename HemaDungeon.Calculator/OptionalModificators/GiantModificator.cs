namespace HemaDungeon.Calculator.OptionalModificators;

public sealed class GiantModificator : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Гиганты", out var spell)) 
            spell = new Character.Spell("Урон от первого попадания не проходит в бою. В случае с волшебным народом - не засчитывается", 1, "P");
        if (spell.Value > 0)
        {
            if (enemy.List.Any(x => x is SmallModificator))
            {
                enemy.Hits = 0;
            }
            else
            {
                enemy.Hits = Math.Max(0, enemy.Hits - 1);
                character.ScoreHealth += 1;
            }
        }
        character.Spells["Гиганты"] = spell;
    }
}