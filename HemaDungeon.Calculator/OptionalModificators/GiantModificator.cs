namespace HemaDungeon.Calculator.OptionalModificators;

public sealed class GiantModificator : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        enemy.Hits = Math.Max(0, enemy.Hits - 1);
        if (enemy.Ability is AbilityType.GodDefence or AbilityType.Worm or AbilityType.Gnome) enemy.Hits = 0;
    }
}