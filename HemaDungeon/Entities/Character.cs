using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HemaDungeon.Entities;

public sealed class Character : IdentityUser
{
    public string Name { get; set; }

    public string Avatar { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string Story { get; set; }
    
    // Physics
    public int PushUp { get; set; }

    public int PullUp { get; set; }

    public int Abdominal { get; set; }

    public float RunTwenty { get; set; }

    public int Rang { get; set; }

    public int Score { get; set; }
    // Physics

    // Stats
    [NotMapped]
    public float Power => Math.Min(60, PushUp) + PullUp * 3;

    [NotMapped]
    public double Agility => Math.Min(50.0, Abdominal) +  (RunTwenty > 0 ? (6.6 - RunTwenty) : 0) * 1.5;

    [NotMapped]
    public float Wisdom => Rang > 0 ? (10 - Rang) * 10 : 0;

    [NotMapped]
    public double Vitality => (Score > 0 ? Score : 1) * (Rang > 0 ? (11 - Rang) * 10 : 10) * (Visits?.Aggregate(0.0, (factor, visit) => visit.WasHere ? factor + 1 : factor / 2) ?? 1);
    // Stats

    public ICollection<Visit>? Visits { get; set; }
}