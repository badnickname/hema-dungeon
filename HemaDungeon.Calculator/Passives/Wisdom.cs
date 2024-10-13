namespace HemaDungeon.Calculator.Passives;

internal sealed class Wisdom : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == false) return;
        character.Wisdom += 10;
        character.IsPassive = true;
    }
}