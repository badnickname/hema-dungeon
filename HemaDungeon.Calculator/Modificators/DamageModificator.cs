namespace HemaDungeon.Calculator.Modificators;

internal sealed class DamageModificator : IModificator
{
    public int Priority => int.MaxValue - 5;

    public void Accept(Character character, Character enemy)
    {
        character.Damage =
            Math.Max(character.Agility - enemy.Agility, 1) +
            Math.Max(character.Power - enemy.Power, 1) +
            Math.Max(character.Wisdom - enemy.Wisdom, 1) +
            Math.Max(character.Stamina - enemy.Stamina, 1);
        character.Damage *= 5;
        for (var i = 0; i < character.TournamentsCount; i++)
            character.Damage *= 1.5f;
        if (character.Rang > enemy.Rang)
            character.Damage *= character.Rang - enemy.Rang + 1;
        else if (character.Rang < enemy.Rang)
            character.Damage /= character.Rang - enemy.Rang + 1;
    }
}