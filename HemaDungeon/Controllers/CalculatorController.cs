using HemaDungeon.Abilities;
using HemaDungeon.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using AbilityType = HemaDungeon.Calculator.AbilityType;
using Character = HemaDungeon.Calculator.Character;

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
    public async Task<CompareResult> Compare(CalculatorCompareModel model, [FromServices] Calculator.Calculator service, [FromServices] Context context)
    {
        var firstState = await GetState(model.FirstUser.Id, context);
        var secondState = await GetState(model.SecondUser.Id, context);

        var ability = new AbilityService();
        var buff0 = ability.Accept(firstState, secondState, model.FirstUser.DisableAbility == false);
        firstState.Name = buff0.Name;
        firstState.Description = buff0.Description;
        
        var buff1 = ability.Accept(secondState, firstState, model.FirstUser.DisableAbility == false);
        secondState.Name = buff1.Name;
        secondState.Description = buff1.Description;

        var first = new Character(
            model.FirstUser.Health ?? firstState.Character.Character.Vitality, 
            0, 
            firstState.Character.Character.Wisdom,
            firstState.Character.Character.Stamina,
            firstState.Character.Character.Agility,
            firstState.Character.Character.Power,
            (AbilityType) firstState.Character.Character.Ability!,
            firstState.Character.Character.Rang,
            firstState.Character.Character.Tournaments?.Count ?? 0
        )
        {
            Force = !model.FirstUser.DisableAbility
        };
        var second = new Character(
            model.SecondUser.Health ?? secondState.Character.Character.Vitality, 
            0, 
            secondState.Character.Character.Wisdom,
            secondState.Character.Character.Stamina,
            secondState.Character.Character.Agility,
            secondState.Character.Character.Power,
            (AbilityType) secondState.Character.Character.Ability!,
            secondState.Character.Character.Rang,
            secondState.Character.Character.Tournaments?.Count ?? 0
        )
        {
            Force = !model.SecondUser.DisableAbility
        };
        service.Accept(first, second);

        firstState.Character.Health = first.Health;
        firstState.ScoreHealth = first.ScoreHealth;
        firstState.Calculated = first.IsPassive || first.Force == true;
        firstState.Damage = first.Damage;
        secondState.Character.Health = second.Health;
        secondState.ScoreHealth = second.ScoreHealth;
        secondState.Calculated = second.IsPassive || second.Force == true;
        secondState.Damage = second.Damage;

        var result = new CompareResult(firstState, secondState);
        return result;
    }

    private static async Task<FightState> GetState(string id, Context context)
    {
        return new FightState
        {
            Character = new FightCharacter
            {
                Character = await context.Users
                    .Include(x => x.Visits)
                    .Include(x => x.Cataclysms)
                    .Include(x => x.Tournaments)
                    .FirstAsync(x => x.Id == id)
            }
        };
    }
 
    [HttpPost("users/calculate")]
    public async Task<CalculateResult> Calculate(CalculatorCompareModel model, [FromServices] Context context, [FromServices] Calculator.Calculator service)
    {
        var compare = await Compare(model, service, context);

        return new CalculateResult(
            new CalculateResult.CalculatedUser(
                compare.FirstUser.Character.Character.Id,
                (float) Math.Floor(Math.Max(0, compare.FirstUser.Character.Health - compare.SecondUser.Damage * (model.SecondUser.Score ?? 0))),
                compare.FirstUser.ScoreHealth
            ),
            new CalculateResult.CalculatedUser(
                compare.SecondUser.Character.Character.Id,
                (float) Math.Floor(Math.Max(0, compare.SecondUser.Character.Health - compare.FirstUser.Damage * (model.FirstUser.Score ?? 0))),
                compare.SecondUser.ScoreHealth
            )
        );
    }

    public sealed record CompareResult(FightState FirstUser, FightState SecondUser);

    public sealed record CalculateResult(CalculateResult.CalculatedUser FirstUser, CalculateResult.CalculatedUser SecondUser)
    {
        public sealed record CalculatedUser(string Id, float Health, float Hits);
    };
}