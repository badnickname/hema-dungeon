namespace HemaDungeon.Calculator.Actives;

internal sealed class Tail : IModificator
{
    public int Priority => 50;

    public void Accept(Character character, Character enemy)
    {
        var health = 50f;
        for (var i = 0; i < character.TournamentsCount; i++)
            health *= 1.5f;
        if (character.Force == true && character.Health > health)
        {
            character.Health -= health;
        }
    }
}