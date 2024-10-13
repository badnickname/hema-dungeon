namespace HemaDungeon.Calculator.Passives;

internal sealed class MasterBefore : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        character.Agility = Math.Min(100f, 2f * character.Agility);
        character.Stamina = Math.Min(100f, 2f * character.Stamina);
        character.Power = Math.Min(100f, 2f * character.Power);
        character.Wisdom = Math.Min(100f, 2f * character.Wisdom);
        character.IsPassive = true;
    }
}