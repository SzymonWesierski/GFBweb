using GameWeb.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class EditGameViewModel
{
    public Games? Game { get; set; }
    public List<GamesCategories>? GamesCategories { get; set; }
    public IFormFile? ImageFile { get; set; }

    [AtLeastOneCategory(ErrorMessage = "Please select at least one category.")]
    public List<int>? SelectedCategoryIds { get; set; } = new List<int>();
}