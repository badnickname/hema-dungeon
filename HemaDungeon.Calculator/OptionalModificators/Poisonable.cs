namespace HemaDungeon.Calculator.OptionalModificators;

internal sealed class Poisonable : IModificator
{
    public int Priority => int.MaxValue;

    public void Accept(Character character, Character enemy)
    {
        if (enemy.Ability == AbilityType.Poison && character.ScoreHealth > 3) character.ScoreHealth = 3;
    }
}