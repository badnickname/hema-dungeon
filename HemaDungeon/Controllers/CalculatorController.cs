using HemaDungeon.Abilities;
using HemaDungeon.Adapters;
using HemaDungeon.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
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
        var file = await System.IO.File.ReadAllBytesAsync($"wwwroot/images/{name}");
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType($"wwwroot/images/{name}", out var contentType)) contentType = "image";
        return File(file, contentType);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetCharacters([FromServices] Context context)
    {
        var result = await context.Users.Include(x => x.Visits).Include(x => x.Cataclysms).ToListAsync();
        return new JsonResult(result);
    }
 
    [HttpPost("users/compare")]
    public async Task<CompareResult> Compare(CalculatorCompareModel model, [FromServices] Calculator.Calculator service, [FromServices] FightStateAdapter adapter, [FromServices] Context context)
    {
        var firstState = await GetState(model.FirstUser.Id, context);
        var secondState = await GetState(model.SecondUser.Id, context);
        
        var first = adapter.ToCharacter(firstState, model.FirstUser.DisableAbility, model.FirstUser.Health);
        var second = adapter.ToCharacter(secondState, model.SecondUser.DisableAbility, model.SecondUser.Health);

        service.Accept(first, second);

        adapter.EnrichFromCharacter(firstState, first);
        adapter.EnrichFromCharacter(secondState, second);

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
    public async Task<CalculateResult> Calculate(CalculatorCompareModel model, [FromServices] Context context, [FromServices] FightStateAdapter adapter, [FromServices] Calculator.Calculator service)
    {
        var compare = await Compare(model, service, adapter, context);

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