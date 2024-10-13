namespace HemaDungeon.Calculator.Passives;

internal sealed class EmptyAbility : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        character.IsPassive = true;
    }
}