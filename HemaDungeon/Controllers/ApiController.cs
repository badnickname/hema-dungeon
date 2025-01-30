using System.Net;
using System.Net.Mail;
using HemaDungeon.Core.Entities;
using HemaDungeon.Core.Reborn;
using HemaDungeon.Models;
using HemaDungeon.Options;
using HemaDungeon.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("api")]
public sealed class ApiController : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromForm] SignUpModel model, [FromServices] UserManager<Character> userManager, Context context, [FromServices] SignInManager<Character> signInManager)
    {
        if (context.Users.Any(x => x.Name == model.Name || x.Email == model.Email)) return Redirect("/");

        var user = new Character
        {
            Email = model.Email,
            Name = model.Name,
            Age = model.Age,
            Gender = model.Gender,
            Story = model.Story,
            UserName = model.Name,
            PushUp = model.PushUp ?? 0,
            PullUp = model.PullUp ?? 0,
            RunTwenty = model.RunTwenty ?? 0,
            RunFifteen = model.RunFifteen ?? 0,
            Abdominal = model.Abdominal ?? 0,
            Rope = model.Rope ?? 0,
            Rang = model.Rang ?? 0,
            Score = model.Score ?? 0,
            Avatar = "default.png",
            Author = model.Author ?? string.Empty,
        };

        var avatar = HttpContext.Request.Form.Files.GetFile("avatar");
        if (avatar is not null)
        {
            var filename = $"{Guid.NewGuid().ToString()}{avatar.FileName}";
            await using var filestream = System.IO.File.Create($"wwwroot/images/{filename}");
            await using var input = avatar.OpenReadStream();
            await input.CopyToAsync(filestream);
            filestream.Flush();

            user.Avatar = filename;
        }

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) return Redirect("/");

        user = await userManager.FindByEmailAsync(model.Email);
        await signInManager.SignInAsync(user!, true);

        return Redirect("/");
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromForm] SignInModel model, [FromServices] UserManager<Character> userManager, [FromServices] SignInManager<Character> signInManager)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null) return Content("Incorrect password or email", "text/plain");

        var result = await signInManager.PasswordSignInAsync(user, model.Password, true, false);
        if (!result.Succeeded) return Content("Incorrect password or email", "text/plain");
        return Redirect("/");
    }

    [HttpPost("password/reset")]
    public async Task<IActionResult> RestorePassword([FromBody] ResetPasswordModel model, [FromServices] IOptions<EmailOption> options, [FromServices] Context context, [FromServices] UserManager<Character> userManager)
    {
        var user = context.Users.FirstOrDefault(x => x.Email == model.Email);
        if (user is null) return Unauthorized();

        var code = await userManager.GeneratePasswordResetTokenAsync(user);

        var smtp = new SmtpClient(options.Value.Host, 587) {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(options.Value.Mail, options.Value.Password),
        };
        smtp.Send(new MailMessage(options.Value.Mail, user.Email!, "Hema Dungeon: Восстановление пароля", $"Ваш код для сброса пароля: {code}"));

        return Ok();
    }

    [HttpPost("password/commit")]
    public async Task<IActionResult> CommitPassword([FromForm] PasswordCommitModel model, [FromServices] Context context, [FromServices] UserManager<Character> userManager, [FromServices] SignInManager<Character> signInManager)
    {
        var user = context.Users.FirstOrDefault(x => x.Email == model.Email);
        if (user is null) return Unauthorized();

        await userManager.ResetPasswordAsync(user, model.Code, model.Password);
        await signInManager.PasswordSignInAsync(user, model.Password, true, false);
        return Redirect("/");
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUser([FromServices] SignInManager<Character> signInManager)
    {
        var user = await signInManager.UserManager.GetUserAsync(HttpContext.User);
        if (user is null) return Unauthorized();
        return new JsonResult(user);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUser([FromServices] CharacterRepository repository)
    {
        var characters = await repository.GetAllCharacters();
        return new JsonResult(characters);
    }

    [HttpPost("reborn")]
    public async Task<IActionResult> Reborn([FromForm] RebornModel model, [FromServices] RebornService service, [FromServices] SignInManager<Character> signInManager, [FromServices] Context context, CancellationToken token)
    {
        var user = await signInManager.UserManager.GetUserAsync(HttpContext.User);
        if (user is null) return Unauthorized();

        var character = await context.Users.Include(x => x.Tournaments).Include(x => x.Visits).FirstAsync(x => x.Id == user.Id, token);
        character.IsDead = false;
        var dead = service.Reborn(character, model);

        var avatar = HttpContext.Request.Form.Files.GetFile("avatar");
        if (avatar is not null)
        {
            var filename = $"{Guid.NewGuid().ToString()}{avatar.FileName}";
            await using var filestream = System.IO.File.Create($"wwwroot/images/{filename}");
            await using var input = avatar.OpenReadStream();
            await input.CopyToAsync(filestream);
            filestream.Flush();

            character.Avatar = filename;
        }

        context.DeadCharacters.Add(dead);
        await context.SaveChangesAsync(token);
        context.Visits.RemoveRange(character.Visits ?? []);
        context.RemoveRange(character.Tournaments ?? []);
        await context.SaveChangesAsync(token);
        return Redirect("/?dashboard=true");
    }

    [HttpPost("user")]
    [Authorize]
    public async Task<IActionResult> PostUser([FromForm] EditModel model, [FromServices] Context context, [FromServices] SignInManager<Character> signInManager)
    {
        var user = await signInManager.UserManager.GetUserAsync(HttpContext.User);
        if (user is null) return Unauthorized();

        user.Age = model.Age;
        user.Name = model.Name;
        user.Gender = model.Gender;
        user.Story = model.Story;
        user.Author = model.Author ?? string.Empty;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Redirect("/?dashboard=true");
    }

    [HttpPost("avatar")]
    [Authorize]
    public async Task<IActionResult> PostAvatar([FromServices] Context context, [FromServices] SignInManager<Character> signInManager)
    {
        var user = await signInManager.UserManager.GetUserAsync(HttpContext.User);
        if (user is null) return Unauthorized();

        var avatar = HttpContext.Request.Form.Files.GetFile("avatar")!;

        System.IO.File.Delete($"wwwroot/{user.Avatar}");

        var filename = $"{Guid.NewGuid().ToString()}{avatar.FileName}";
        await using var filestream = System.IO.File.Create($"wwwroot/images/{filename}");
        await using var input = avatar.OpenReadStream();
        await input.CopyToAsync(filestream);
        filestream.Flush();

        user.Avatar = filename;
        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Redirect("/?dashboard=true");
    }

    [HttpPost("state")]
    [Authorize]
    public async Task<IActionResult> PostState([FromForm] StateModel model, [FromServices] Context context, [FromServices] SignInManager<Character> signInManager)
    {
        var user = await signInManager.UserManager.GetUserAsync(HttpContext.User);
        if (user is null) return Unauthorized();

        user.PushUp = model.PushUp;
        user.PullUp = model.PullUp;
        user.Abdominal = model.Abdominal;
        user.RunTwenty = model.RunTwenty;
        user.RunFifteen = model.RunFifteen;
        user.Rang = model.Rang;
        user.Score = model.Score;
        user.Rope = model.Rope;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Redirect("/?dashboard=true");
    }
}