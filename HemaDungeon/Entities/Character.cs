using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HemaDungeon.Abilities;
using Microsoft.AspNetCore.Identity;

namespace HemaDungeon.Entities;

public sealed class Character : IdentityUser
{
    public string Name { get; set; }

    public string Avatar { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string Story { get; set; }

    public string Author { get; set; }

    // Physics
    public int PushUp { get; set; }

    public int PullUp { get; set; }

    public int Abdominal { get; set; }

    public float RunTwenty { get; set; }

    public float RunFifteen { get; set; }

    public int Rang { get; set; }

    public int Score { get; set; }

    public int Rope { get; set; }

    public AbilityType? Ability { get; set; }

    public ICollection<Page> Pages { get; set; }
    // Physics

    // Stats
    [NotMapped]
    public float Power => Math.Min(60, PushUp) + PullUp * 3;

    [NotMapped]
    public double Agility => Math.Min(50.0, Abdominal) +  (RunTwenty > 0 ? (20 - RunTwenty) : 0) * 5;

    [NotMapped]
    public float Wisdom => Rang > 0 ? (11 - Rang) * 10 : 0;

    [NotMapped] 
    public double Stamina => Rope / 10.0 + RunFifteen * 2.5;

    [NotMapped]
    public double Vitality {
        get
        {
            var value = (Score > 0 ? Score : 1) * 5;
            var visits = Visits?.Where(x => !x.CanSkip).OrderBy(x => x.Date).Aggregate(0.0, (factor, visit) => visit.WasHere ? factor + 1 : factor / 2) ?? 1;
            if (value * visits == 0) return value;
            var cataclysm = Cataclysms?.Count ?? 0;
            if (cataclysm == 0) cataclysm = 1;
            return value * visits / (float) cataclysm;
        }
    }
    // Stats

    [JsonIgnore]
    public ICollection<Visit>? Visits { get; set; }

    [JsonIgnore]
    public ICollection<Cataclysm>? Cataclysms { get; set; }
}