namespace HemaDungeon.Calculator.Actives;

public sealed class AirShield : IModificator
{
    public int Priority => int.MaxValue - 13;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == true)
        {
            character.Health *= 0.8;
            enemy.Damage *= 0.5;
        }
    }
}