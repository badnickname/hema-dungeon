namespace HemaDungeon.Calculator.Passives;

public sealed class Bubble : IModificator
{
    public int Priority => 0;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == false) return;
        enemy.Hits -= enemy.Hits / 3;
        character.IsPassive = true;
    }
}