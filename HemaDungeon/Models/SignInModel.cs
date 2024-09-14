﻿using System.ComponentModel.DataAnnotations;

namespace HemaDungeon.Models;

public sealed class SignInModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}