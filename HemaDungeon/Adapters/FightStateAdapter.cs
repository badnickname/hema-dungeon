using HemaDungeon.Abilities;
using HemaDungeon.Entities;
using Character = HemaDungeon.Calculator.Character;
using AbilityType = HemaDungeon.Calculator.AbilityType;

namespace HemaDungeon.Adapters;

public sealed class FightStateAdapter(AbilityService ability)
{
    public Character ToCharacter(FightState state, bool? disable, int? health)
    {
        var buff = ability.Accept(state);
        state.Name = buff.Name;
        state.Description = buff.Description;
        
        var character = new Character(
            health ?? state.Character.Character.Vitality, 
            0, 
            state.Character.Character.Wisdom,
            state.Character.Character.Stamina,
            state.Character.Character.Agility,
            state.Character.Character.Power,
            (AbilityType) state.Character.Character.Ability!,
            state.Character.Character.Rang,
            state.Character.Character.Tournaments?.Count ?? 0
        )
        {
            Force = !disable
        };

        return character;
    }

    public void EnrichFromCharacter(FightState state, Character character)
    {
        state.Character.Health = character.Health;
        state.ScoreHealth = character.ScoreHealth;
        state.Calculated = character.IsPassive || character.Force == true;
        state.Damage = character.Damage;
    }
}