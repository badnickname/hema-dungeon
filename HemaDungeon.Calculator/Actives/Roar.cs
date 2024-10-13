namespace HemaDungeon.Calculator.Actives;

internal sealed class Roar : IModificator
{
    public int Priority => int.MaxValue - 22;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force != true) return;
        enemy.Health -= 100;
        if (enemy.Health < 0) enemy.Health = 0;
    }
}