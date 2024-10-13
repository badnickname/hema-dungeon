namespace HemaDungeon.Calculator.Passives;

internal sealed class EmptyAbility : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == false) return;
        character.IsPassive = true;
    }
}