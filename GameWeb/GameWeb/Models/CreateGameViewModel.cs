﻿using GameWeb.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class AtLeastOneCategoryAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var list = value as List<int>;
        return list != null && list.Count > 0;
    }
}

public class GamesCreateViewModel
{
    public Games? Game { get; set; }
    public List<GamesCategories>? GamesCategories { get; set; }
    [Required(ErrorMessage ="You need to upload image")]
    public IFormFile? ImageFile { get; set; }

    [AtLeastOneCategory(ErrorMessage = "Please select at least one category.")]
    public List<int>? SelectedCategoryIds { get; set; } = new List<int>();
}