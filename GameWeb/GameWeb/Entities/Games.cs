using System.ComponentModel.DataAnnotations;

namespace GameWeb.Entities;

public class Games
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Pole tytuł jest wymagane")]
    public string Title { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
    public string Description { get; set; } = string.Empty;
    public string MainImagePath { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; } = DateTime.Now;
    //rating
    public int NumberOfVotes { get; set; } = 0;
    public int TotalStars { get; set; } = 0;
    public double Rating { get; set; } = 0;

    //Relation with Comments
    public ICollection<Comments> CommentsList { get; set; } = new List<Comments>();
    // Relation with GamesCategories
    public List<GamesAndCategories> GamesAndCategoriesList { get; set; } = new();
    public List<GamesCategories> CategoryList { get; set; } = new();
}
