namespace HemaDungeon.Calculator.Actives;

internal sealed class Joke : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == true) character.Health += 50;
    }
}