﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GameWeb.Entities;

public class ApplicationUsers : IdentityUser
{
    public ICollection<Comments> CommentsList { get; set; } = new List<Comments>();
    public string? ImagePath { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
}


