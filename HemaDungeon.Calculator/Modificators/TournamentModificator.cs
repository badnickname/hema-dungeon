namespace HemaDungeon.Calculator.Modificators;

public sealed class TournamentModificator : IModificator
{
    public int Priority => int.MaxValue - 10;

    public void Accept(Character character, Character enemy)
    {
        for (var i = 0; i < character.TournamentsCount; i++)
            character.Damage *= 1.5f;
    }
}