namespace HemaDungeon.Calculator.Actives;

internal sealed class Roar : IModificator
{
    public int Priority => int.MaxValue - 22;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force != true) return;
        var damage = 100f;
        for (var i = 0; i < character.TournamentsCount; i++)
            damage *= 1.5f;
        enemy.Health -= damage;
        if (enemy.Health < 0) enemy.Health = 0;
    }
}