namespace HemaDungeon.Calculator.Modificators;

internal sealed class ScoreHealthModificator : IModificator
{
    public int Priority => int.MaxValue - 4;

    public void Accept(Character character, Character enemy)
    {
        character.ScoreHealth = (int) Math.Floor(character.Health / enemy.Damage);
    }
}