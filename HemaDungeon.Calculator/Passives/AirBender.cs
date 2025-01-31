namespace HemaDungeon.Calculator.Passives;

internal sealed class AirBender : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        character.Agility += 10;
    }
}