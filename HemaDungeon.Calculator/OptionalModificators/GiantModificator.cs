namespace HemaDungeon.Calculator.OptionalModificators;

public sealed class GiantModificator : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (!character.Spells.TryGetValue("Гиганты", out var spell)) 
            spell = new Character.Spell("Большие существа, попасть по которым не просто", 1, "P");
        if (spell.Value > 0)
        {
            enemy.Hits = Math.Max(0, enemy.Hits - 1);
            character.ScoreHealth = 1;
            if (enemy.Ability is AbilityType.GodDefence or AbilityType.Worm or AbilityType.Gnome)
            {
                enemy.Hits = 0;
                character.ScoreHealth = 666;
            }
        }
        character.Spells["Гиганты"] = spell;
    }
}