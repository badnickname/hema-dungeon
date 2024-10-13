﻿namespace HemaDungeon.Calculator.Actives;

internal sealed class Gnome : IModificator
{
    public int Priority => 50;

    public void Accept(Character character, Character enemy)
    {
        if (character.Force == true) character.Damage += 10;
    }
}