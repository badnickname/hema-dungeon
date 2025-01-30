using HemaDungeon.Core.Entities;

namespace HemaDungeon.Core.Abilities;

public sealed class HealthService
{
    public double Enrich(Character character)
    {
        var value = (character.Score > 0 ? character.Score : 1) * 5;
        var factor = character.Visits?.Where(x => !x.CanSkip).Select(x => x.WasHere).Aggregate(0.0, (root, x) => x ? root + 1 : root / 2.0) ?? 0.0;
        if (factor * value == 0) return value;
        return value * factor;
    } 
}