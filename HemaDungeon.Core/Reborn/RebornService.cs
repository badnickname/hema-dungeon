using HemaDungeon.Core.Entities;

namespace HemaDungeon.Core.Reborn;

public sealed class RebornService
{
    public DeadCharacter Reborn(Character character, RebornModel model)
    {
        var dead = new DeadCharacter
        {
            Id = Guid.NewGuid().ToString(),
            ParentId = character.Id,
            Abdominal = character.Abdominal,
            RunTwenty = character.RunTwenty,
            RunFifteen = character.RunFifteen,
            Author = character.Author,
            PushUp = character.PushUp,
            PullUp = character.PullUp,
            Avatar = character.Avatar,
            Age = character.Age,
            Gender = character.Gender,
            Story = character.Story,
            Name = character.Name,
            Rang = character.Rang,
            Score = character.Score,
            Rope = character.Rope,
            DeadVisits = character.Visits?.Select(x => new DeadVisit { Id = Guid.NewGuid().ToString(), CanSkip = x.CanSkip, Date = x.Date, WasHere = x.WasHere }).ToList() ??
                         [],
            DeadTournaments = character.Tournaments?.Select(x => new DeadTournament { Id = Guid.NewGuid().ToString() }).ToList() ?? []
        };

        character.Age = model.Age;
        character.Name = model.Name;
        character.Gender = model.Gender;
        character.Story = model.Story;
        return dead;
    }
}