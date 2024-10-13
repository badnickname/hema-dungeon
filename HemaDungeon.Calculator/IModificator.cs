namespace HemaDungeon.Calculator;

internal interface IModificator
{
    int Priority { get; }
    void Accept(Character character, Character enemy);
}