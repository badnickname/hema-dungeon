namespace HemaDungeon.Calculator.Passives;

internal sealed class GodDefence : IModificator
{
    public int Priority => int.MaxValue - 10;

    public void Accept(Character character, Character enemy)
    {
        if (enemy.Rang > 3) enemy.Damage *= 0.2f;
        character.IsPassive = true;
    }
}