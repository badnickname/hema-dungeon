namespace HemaDungeon.Calculator.Modificators;

public sealed class BubbleModificator : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        enemy.Hits -= enemy.Hits / 3;
    }
}