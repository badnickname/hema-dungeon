﻿namespace HemaDungeon.Calculator.Passives;

internal sealed class Armor : IModificator
{
    public int Priority => int.MaxValue - 10;

    public void Accept(Character character, Character enemy)
    {
        enemy.Damage *= 0.2f;
        character.IsPassive = true;
    }
}