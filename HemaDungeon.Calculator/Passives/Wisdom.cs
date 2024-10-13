namespace HemaDungeon.Calculator.Passives;

internal sealed class Wisdom : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        character.Wisdom += 10;
        character.IsPassive = true;
    }
}