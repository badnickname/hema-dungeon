namespace HemaDungeon.Calculator.Actives;

internal sealed class Tail : IModificator
{
    public int Priority => 50;

    public void Accept(Character character, Character enemy)
    {
        if (character is { Force: true, Health: > 50 })
        {
            character.Health -= 50;
        }
    }
}