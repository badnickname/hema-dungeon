using HemaDungeon.Calculator.Actives;
using HemaDungeon.Calculator.Modificators;
using HemaDungeon.Calculator.OptionalModificators;
using HemaDungeon.Calculator.Passives;

namespace HemaDungeon.Calculator;

public sealed class Calculator
{
    public void Accept(Character first, Character second)
    {
        AcceptBase(first);
        AcceptBase(second);
        AcceptAbility(first);
        AcceptAbility(second);

        var firstList = first.List.Select(x => new { Modificator = x, x.Priority, Character = first, Enemy = second });
        var secondList = second.List.Select(x => new { Modificator = x, x.Priority, Character = second, Enemy = first });
        var list = firstList.Concat(secondList).OrderBy(x => x.Priority);
        foreach (var item in list) item.Modificator.Accept(item.Character, item.Enemy);
    }

    private static void AcceptBase(Character character)
    {
        character.List.Add(new DamageModificator());
        character.List.Add(new ScoreHealthModificator());
        character.List.Add(new TournamentModificator());
        if (character.Ability != AbilityType.Armor) character.List.Add(new Poisonable());
    }
 
    private static void AcceptAbility(Character character)
    {
        switch (character.Ability)
        {
            case AbilityType.GodDefence:
                character.List.Add(new GodDefence());
                break;
            case AbilityType.Joke:
                character.List.Add(new Joke());
                break;
            case AbilityType.Tail:
                character.List.Add(new Tail());
                break;
            case AbilityType.Roar:
                character.List.Add(new Roar());
                break;
            case AbilityType.Food:
                character.List.Add(new Food());
                break;
            case AbilityType.Air:
                character.List.Add(new AirBender());
                break;
            case AbilityType.Armor:
                character.List.Add(new Armor());
                break;
            case AbilityType.Klukalo:
                character.List.Add(new EmptyAbility());
                break;
            case AbilityType.Mimic:
                character.List.Add(new EmptyAbility());
                break;
            case AbilityType.Worm:
                character.List.Add(new EmptyAbility());
                break;
            case AbilityType.Poison:
                character.List.Add(new EmptyAbility());
                break;
            case AbilityType.Wisdom:
                character.List.Add(new Wisdom());
                break;
            case AbilityType.Calm:
                character.List.Add(new EmptyAbility());
                break;
            case AbilityType.Gnome:
                character.List.Add(new Gnome());
                break;
            case AbilityType.DungeonMaster:
                character.List.Add(new MasterBefore());
                character.List.Add(new GodDefence());
                break;
        }
    }
}