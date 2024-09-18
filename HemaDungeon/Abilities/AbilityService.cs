using HemaDungeon.Entities;

namespace HemaDungeon.Abilities;

public sealed class AbilityService
{
    public Buff Accept(FightState state, FightState second)
    {
        switch (state.Character.Character.Ability)
        {
            case AbilityType.GodDefence:
            {
                if (second.Character.Character.Rang > 3) return new Buff { ResistFactor = 0.2f, Calculated = true };
                break;
            }
            case AbilityType.Food:
            {
                if (second.Character.Character.Stamina < state.Character.Character.Stamina) return new Buff { Damage = 10, Calculated = true };
                break;
            }
            case AbilityType.Air:
            {
                return new Buff { Agility = 10 };
            }
            case AbilityType.Armor:
            {
                if (second.Character.Character.Ability != AbilityType.Poison) return new Buff { ResistFactor = 0.8f, Calculated = true };
                break;
            }
            case AbilityType.Mimic:
            {
                return new Buff { CopyStats = true, Calculated = true };
            }
            case AbilityType.Wisdom:
            {
                return new Buff { Wisdom = 10, Calculated = true };
            }
        }

        return new Buff();
    }
}