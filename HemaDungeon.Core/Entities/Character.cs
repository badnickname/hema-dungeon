using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HemaDungeon.Core.Abilities;
using Microsoft.AspNetCore.Identity;

namespace HemaDungeon.Core.Entities;

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

    public Region? Region { get; set; }

    public bool? IsDead { get; set; }

    public bool? VisitToday { get; set; }

    public AbilityType? Ability { get; set; }

    public ICollection<Page> Pages { get; set; }
    // Physics

    // Stats
    [NotMapped]
    public float Power => Math.Min(60, PushUp) + PullUp * 3;

    [NotMapped]
    public double Agility => Math.Min(50.0, Abdominal) + Math.Max(0,  (12.5f - RunTwenty) * 5);

    [NotMapped]
    public float Wisdom => Rang > 0 ? (11 - Rang) * 10 : 0;

    [NotMapped] 
    public double Stamina => Rope / 10.0 + RunFifteen * 2.2;

    [NotMapped]
    public double Vitality => new HealthService().Enrich(this);

    [NotMapped] public int League => Name.ToLower() == "кокос" || Name.ToLower() == "брен тебрил" || Name.ToLower() == "витгард солморн" ? 6 : 7;
    // Stats

    [NotMapped]
    public int Harmed { get; set; }

    [NotMapped]
    public int Healed { get; set; }

    [JsonIgnore]
    public ICollection<Visit>? Visits { get; set; }

    [JsonIgnore]
    public ICollection<Cataclysm>? Cataclysms { get; set; }

    [JsonIgnore]
    public ICollection<Tournament>? Tournaments { get; set; }
}