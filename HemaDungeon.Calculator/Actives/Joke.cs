namespace HemaDungeon.Calculator.Actives;

internal sealed class Joke : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == true)
        {
            var health = 50f;
            for (var i = 0; i < character.TournamentsCount; i++)
                health *= 1.5f;
            character.Health += health;
        }
    }
}