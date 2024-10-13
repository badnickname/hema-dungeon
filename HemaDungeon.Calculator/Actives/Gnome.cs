namespace HemaDungeon.Calculator.Actives;

internal sealed class Gnome : IModificator
{
    public int Priority => int.MaxValue - 14;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == true) character.Damage += 10;
    }
}