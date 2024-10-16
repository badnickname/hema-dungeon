namespace HemaDungeon.Calculator.Modificators;

public sealed class TournamentModificator : IModificator
{
    public int Priority => int.MaxValue - 13;

    public void Accept(Character character, Character enemy)
    {
        character.Damage *= 5;
    }
}