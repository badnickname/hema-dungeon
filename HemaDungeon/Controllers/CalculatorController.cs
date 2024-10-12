using HemaDungeon.Abilities;
using HemaDungeon.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("calc/api")]
public sealed class CalculatorController : ControllerBase
{
    [HttpGet("image/{name}")]
    [ResponseCache(Duration = 3600)]
    public async Task<IActionResult> GetImage(string name)
    {
        var file = await System.IO.File.ReadAllBytesAsync($"wwwroot/{name}");
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType($"wwwroot/{name}", out var contentType)) contentType = "image";
        return File(file, contentType);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetCharacters([FromServices] Context context)
    {
        var result = await context.Users.Include(x => x.Visits).Include(x => x.Cataclysms).ToListAsync();
        return new JsonResult(result);
    }

    [HttpPost("users/compare")]
    public async Task<CompareResult> Compare(CalculatorCompareModel model, [FromServices] AbilityService service, [FromServices] Context context)
    {
        var firstState = new FightState
        {
            Character = new FightCharacter
            {
                Character = await context.Users
                    .Include(x => x.Visits)
                    .Include(x => x.Cataclysms)
                    .Include(x => x.Tournaments)
                    .FirstAsync(x => x.Id == model.FirstUser.Id)
            }
        };
        firstState.Character.Health = firstState.Character.Character.Vitality;

        var secondState = new FightState
        {
            Character = new FightCharacter
            {
                Character = await context.Users
                    .Include(x => x.Visits)
                    .Include(x => x.Cataclysms)
                    .Include(x => x.Tournaments)
                    .FirstAsync(x => x.Id == model.SecondUser.Id)
            }
        };
        secondState.Character.Health = secondState.Character.Character.Vitality;

        var buff0 = model.FirstUser.DisableAbility == true ? new Buff() : service.Accept(firstState, secondState);
        var buff1 = model.SecondUser.DisableAbility == true ? new Buff() : service.Accept(secondState, firstState);

        var agility0 = buff0.StatesFactor is not null ? Math.Min(100, firstState.Character.Character.Agility * 2) : firstState.Character.Character.Agility;
        var power0 = buff0.StatesFactor is not null ? Math.Min(100, firstState.Character.Character.Power * 2) : firstState.Character.Character.Power;
        var wisdom0 = buff0.StatesFactor is not null ? Math.Min(100, firstState.Character.Character.Wisdom * 2) : firstState.Character.Character.Wisdom;
        var stamina0 = buff0.StatesFactor is not null ? Math.Min(100, firstState.Character.Character.Stamina * 2) : firstState.Character.Character.Stamina;
        
        var agility1 = buff1.StatesFactor is not null ? Math.Min(100, secondState.Character.Character.Agility * 2) : secondState.Character.Character.Agility;
        var power1 = buff1.StatesFactor is not null ? Math.Min(100, secondState.Character.Character.Power * 2) : secondState.Character.Character.Power;
        var wisdom1 = buff1.StatesFactor is not null ? Math.Min(100, secondState.Character.Character.Wisdom * 2) : secondState.Character.Character.Wisdom;
        var stamina1 = buff1.StatesFactor is not null ? Math.Min(100, secondState.Character.Character.Stamina * 2) : secondState.Character.Character.Stamina;
        
        
        firstState.Damage =
            Math.Max(agility0 + buff0.Agility - agility1 - buff1.Agility, 1) +
            Math.Max(power0 - power1, 1) +
            Math.Max(wisdom0 + buff0.Wisdom - wisdom1 - buff1.Wisdom, 1) +
            Math.Max(stamina0 - stamina1, 1);
        firstState.Damage *= 5;
        if (firstState.Character.Character.Tournaments is not null && firstState.Character.Character.Tournaments.Count > 0)
        {
            firstState.Damage *= firstState.Character.Character.Tournaments.Count * 1.5;
        }
        if (firstState.Character.Character.Rang > secondState.Character.Character.Rang)
        {
            firstState.Damage *= (firstState.Character.Character.Rang - secondState.Character.Character.Rang + 1);
        }
        else if (firstState.Character.Character.Rang < secondState.Character.Character.Rang)
        {
            firstState.Damage /= (secondState.Character.Character.Rang - firstState.Character.Character.Rang + 1);
        }

        secondState.Damage =
            Math.Max(agility1 + buff1.Agility - agility0 - buff0.Agility, 1) +
            Math.Max(power1 - power0, 1) +
            Math.Max(wisdom1 + buff1.Wisdom - wisdom0 - buff0.Wisdom, 1) +
            Math.Max(stamina1 - stamina0, 1);
        secondState.Damage *= 5;
        if (secondState.Character.Character.Tournaments is not null && secondState.Character.Character.Tournaments.Count > 0)
        {
            secondState.Damage *= secondState.Character.Character.Tournaments.Count * 1.5;
        }
        if (secondState.Character.Character.Rang > firstState.Character.Character.Rang)
        {
            secondState.Damage *= (secondState.Character.Character.Rang - firstState.Character.Character.Rang);
        }
        else if (secondState.Character.Character.Rang < firstState.Character.Character.Rang)
        {
            secondState.Damage /= (firstState.Character.Character.Rang - secondState.Character.Character.Rang);
        }

        if (buff0.CopyStats || buff1.CopyStats)
        {
            firstState.Damage = 20;
            secondState.Damage = 20;
        }
        secondState.Damage = (int) (secondState.Damage * (buff0.ResistFactor > 0 ? buff0.ResistFactor : 1)) + buff1.Damage;
        firstState.Damage = (int) (firstState.Damage * (buff1.ResistFactor > 0 ? buff1.ResistFactor : 1)) + buff0.Damage;

        firstState.Calculated = buff0.Calculated;
        firstState.Name = buff0.Name ?? string.Empty;
        firstState.Description = buff0.Description ?? string.Empty;
        secondState.Calculated = buff1.Calculated;
        secondState.Name = buff1.Name ?? string.Empty;
        secondState.Description = buff1.Description ?? string.Empty;

        firstState.ScoreHealth = (int) Math.Ceiling(firstState.Character.Health / secondState.Damage);
        if (firstState.ScoreHealth == 0) firstState.ScoreHealth = 1;

        secondState.ScoreHealth = (int) Math.Ceiling(secondState.Character.Health / firstState.Damage);
        if (secondState.ScoreHealth == 0) secondState.ScoreHealth = 1;

        var result = new CompareResult(firstState, secondState);
        return result;
    }

    [HttpPost("users/calculate")]
    public async Task<CalculateResult> Calculate(CalculatorCompareModel model, [FromServices] Context context, [FromServices] AbilityService service)
    {
        var compare = await Compare(model, service, context);
        if (model.FirstUser.Health.HasValue) compare.FirstUser.Character.Health = model.FirstUser.Health.Value;
        if (model.SecondUser.Health.HasValue) compare.SecondUser.Character.Health = model.SecondUser.Health.Value;

        var firstHealth = model.FirstUser.Health ?? 0.0;
        compare.FirstUser.Character.Health = firstHealth;
        compare.FirstUser.Character.Health -= compare.SecondUser.Damage * (model.SecondUser.Score ?? 0) + (model.SecondUser.Damage ?? 0);
        if (compare.FirstUser.Character.Health < 0) compare.FirstUser.Character.Health = 0;

        var secondHealth = model.SecondUser.Health ?? 0.0;
        compare.SecondUser.Character.Health = secondHealth;
        compare.SecondUser.Character.Health -= compare.FirstUser.Damage * (model.FirstUser.Score ?? 0) + (model.FirstUser.Damage ?? 0);
        if (compare.SecondUser.Character.Health < 0) compare.SecondUser.Character.Health = 0;

        return new CalculateResult(
            new CalculateResult.CalculatedUser(
                compare.FirstUser.Character.Character.Id,
                (float) compare.FirstUser.Character.Health,
                (int) Math.Ceiling(firstHealth / compare.SecondUser.Damage)
            ),
            new CalculateResult.CalculatedUser(
                compare.SecondUser.Character.Character.Id,
                (float) compare.SecondUser.Character.Health,
                (int) Math.Ceiling(secondHealth / compare.FirstUser.Damage)
            )
        );
    }

    public sealed record CompareResult(FightState FirstUser, FightState SecondUser);

    public sealed record CalculateResult(CalculateResult.CalculatedUser FirstUser, CalculateResult.CalculatedUser SecondUser)
    {
        public sealed record CalculatedUser(string Id, float Health, float Hits);
    };
}