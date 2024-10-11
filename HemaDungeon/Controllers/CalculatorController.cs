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
        var result = await context.Users.ToListAsync();
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
                    .FirstAsync(x => x.Id == model.SecondUser.Id)
            }
        };
        secondState.Character.Health = secondState.Character.Character.Vitality;

        var buff0 = service.Accept(firstState, secondState);
        var buff1 = service.Accept(secondState, firstState);

        firstState.Damage =
            Math.Max(firstState.Character.Character.Agility + buff0.Agility - secondState.Character.Character.Agility - buff1.Agility, 1) +
            Math.Max(firstState.Character.Character.Power - secondState.Character.Character.Power, 1) +
            Math.Max(firstState.Character.Character.Wisdom + buff0.Wisdom - secondState.Character.Character.Wisdom - buff1.Wisdom, 1) +
            Math.Max(firstState.Character.Character.Stamina - secondState.Character.Character.Stamina, 1);
        firstState.Damage *= 5;
        if (firstState.Character.Character.Rang > secondState.Character.Character.Rang)
        {
            firstState.Damage *= (firstState.Character.Character.Rang - secondState.Character.Character.Rang + 1);
        }
        else if (firstState.Character.Character.Rang < secondState.Character.Character.Rang)
        {
            firstState.Damage /= (secondState.Character.Character.Rang - firstState.Character.Character.Rang + 1);
        }

        secondState.Damage =
            Math.Max(secondState.Character.Character.Agility + buff1.Agility - firstState.Character.Character.Agility - buff0.Agility, 1) +
            Math.Max(secondState.Character.Character.Power - firstState.Character.Character.Power, 1) +
            Math.Max(secondState.Character.Character.Wisdom + buff1.Wisdom - firstState.Character.Character.Wisdom - buff0.Wisdom, 1) +
            Math.Max(secondState.Character.Character.Stamina - firstState.Character.Character.Stamina, 1);
        secondState.Damage *= 5;
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