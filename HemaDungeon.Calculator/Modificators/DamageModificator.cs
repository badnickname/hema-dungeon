namespace HemaDungeon.Calculator.Modificators;

internal sealed class DamageModificator : IModificator
{
    public int Priority => int.MaxValue - 15;

    public void Accept(Character character, Character enemy)
    {
        character.Damage =
            Math.Max(character.Agility - enemy.Agility, 1) +
            Math.Max(character.Power - enemy.Power, 1) +
            Math.Max(character.Wisdom - enemy.Wisdom, 1) +
            Math.Max(character.Stamina - enemy.Stamina, 1);
        character.Damage *= 5;
        if (character.Rang > enemy.Rang)
            character.Damage *= character.Rang - enemy.Rang + 1;
        else if (character.Rang < enemy.Rang)
            character.Damage /= enemy.Rang - character.Rang + 1;
        if (character.League > enemy.League)
            character.Damage *= character.League - enemy.League + 1;
        else if (character.League < enemy.League)
            character.Damage /= enemy.League - character.League + 1;
    }
}