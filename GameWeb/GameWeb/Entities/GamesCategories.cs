using System.ComponentModel.DataAnnotations;

namespace GameWeb.Entities;

public class GamesCategories
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Musisz podać nazwę kategorii")]
    public string CategoryName { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;

    // Relation with Games
    public List<GamesAndCategories> GamesAndCategoriesList { get; set; } = new();
    public List<Games> GamesList { get; set; } = new();
}