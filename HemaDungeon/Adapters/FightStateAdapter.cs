using HemaDungeon.Core.Abilities;
using HemaDungeon.Core.Entities;
using HemaDungeon.Models;
using Character = HemaDungeon.Calculator.Character;
using AbilityType = HemaDungeon.Calculator.AbilityType;

namespace HemaDungeon.Adapters;

public sealed class FightStateAdapter(AbilityService ability)
{
    public Character ToCharacter(FightState state, int? health, ICollection<CalculatorCompareModel.Spell> spells)
    {
        var buff = ability.Accept(state);
        state.Name = buff.Name;
        state.Description = buff.Description;

        var character = new Character(
            health ?? state.Character.Character.Vitality,
            state.Character.Character.Vitality,
            0,
            state.Character.Character.Wisdom,
            state.Character.Character.Stamina,
            state.Character.Character.Agility,
            state.Character.Character.Power,
            (AbilityType) state.Character.Character.Ability!,
            state.Character.Character.Rang,
            state.Character.Character.Tournaments?.Count ?? 0,
            state.Character.Character.League,
            spells.ToDictionary(s => s.Key, s => new Character.Spell(s.Description, s.Value, s.Type))
        );

        return character;
    }

    public CalculatorCompareResult EnrichFromCharacter(FightState state, Character character)
    {
        return new CalculatorCompareResult
        {
            Damage = character.Damage,
            Health = character.Health,
            Id = state.Id,
            ScoreHealth = character.ScoreHealth,
            Spells = character.Spells.Select(x => new CalculatorCompareResult.Spell(x.Key, x.Value.Description, x.Value.Value, x.Value.Type)).ToList()
        };
    }
}