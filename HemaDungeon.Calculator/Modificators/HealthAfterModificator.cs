namespace HemaDungeon.Calculator.Modificators;

public sealed class HealthAfterModificator : IModificator
{
    public int Priority => int.MaxValue - 12;

    public void Accept(Character character, Character enemy)
    {
        character.HealthAfter = Math.Floor(Math.Max(0, character.Health - enemy.Damage * enemy.Hits));
    }
}