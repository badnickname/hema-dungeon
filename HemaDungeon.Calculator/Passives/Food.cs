namespace HemaDungeon.Calculator.Passives;

internal sealed class Food : IModificator
{
    public int Priority => 10;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == false) return;
        if (character.Stamina > enemy.Stamina) character.Damage += 10;
        character.IsPassive = true;
    }
}