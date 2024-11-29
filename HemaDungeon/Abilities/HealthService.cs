using HemaDungeon.Entities;

namespace HemaDungeon.Abilities;

public sealed class HealthService
{
    public double Enrich(Character character)
    {
        var value = (character.Score > 0 ? character.Score : 1) * 5;
        var factor = (double) 0;
        var criticalDay = new DateTime(2024, 11, 2, 0, 0, 0, DateTimeKind.Utc);
        var prevDay = character.Visits?.FirstOrDefault();
        var harm = (double) 0;
        foreach (var visit in character.Visits?.Where(x => !x.CanSkip).OrderBy(x => x.Date) ?? Enumerable.Empty<Visit>())
        {
            if (visit.WasHere) factor += 1;
            else
            {
                if (visit.Date < criticalDay)
                {
                    factor /= 2.0;
                    harm /= 2.0;
                }
                else
                {
                    factor /= 3;
                    harm /= 3.0;
                }
            }

            var delta = Math.Max((visit.Date?.Month ?? 0) - (prevDay?.Date?.Month ?? 0), 0);
            if (delta > 0)
            {
                harm += (value * factor - harm) / 2.0;
            }

            prevDay = visit;
        }

        var result = value * factor == 0 ? value : value * factor - harm;
        return result < 1 ? 1 : result;
    } 
}