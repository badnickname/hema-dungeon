using HemaDungeon.Entities;
using HemaDungeon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HemaDungeon.Controllers;

[ApiController]
[Route("api")]
public sealed class ApiController : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromForm] SignUpModel model, [FromServices] UserManager<Character> userManager, Context context, [FromServices] SignInManager<Character> signInManager)
    {
        if (context.Users.Any(x => x.Name == model.Name || x.Email == model.Email)) return Redirect("/");

        var avatar = HttpContext.Request.Form.Files.GetFile("avatar")!;
        var filename = $"{Guid.NewGuid().ToString()}{avatar.FileName}";
        await using var filestream = System.IO.File.Create($"wwwroot/{filename}");
        await using var input = avatar.OpenReadStream();
        await input.CopyToAsync(filestream);
        filestream.Flush();

        var user = new Character
        {
            Email = model.Email,
            Name = model.Name,
            Avatar = filename,
            Age = model.Age,
            Gender = model.Gender,
            Story = model.Story,
            UserName = model.Name
        };
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
        if (user is null) return Redirect("/login");

        var result = await signInManager.PasswordSignInAsync(user, model.Password, true, false);
        return Redirect(!result.Succeeded ? "/login" : "/");
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUser([FromServices] SignInManager<Character> signInManager)
    {
        var user = await signInManager.UserManager.GetUserAsync(HttpContext.User);
        if (user is null) return Unauthorized();
        return new JsonResult(user);
    }

    [HttpGet("users")]
    public IActionResult GetUser([FromServices] Context context)
    {
        return new JsonResult(context.Users.ToList());
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

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Redirect("/");
    }

    [HttpPost("avatar")]
    [Authorize]
    public async Task<IActionResult> PostAvatar([FromServices] Context context, [FromServices] SignInManager<Character> signInManager)
    {
        var user = await signInManager.UserManager.GetUserAsync(HttpContext.User);
        if (user is null) return Unauthorized();

        System.IO.File.Delete($"wwwroot/{user.Avatar}");

        var avatar = HttpContext.Request.Form.Files.GetFile("avatar")!;
        var filename = $"{Guid.NewGuid().ToString()}{avatar.FileName}";
        await using var filestream = System.IO.File.Create($"wwwroot/{filename}");
        await using var input = avatar.OpenReadStream();
        await input.CopyToAsync(filestream);
        filestream.Flush();

        user.Avatar = filename;
        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Redirect("/");
    }
}