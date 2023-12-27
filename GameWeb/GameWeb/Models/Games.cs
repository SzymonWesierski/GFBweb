using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace GameWeb.Models;

public class Games
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Pole tytuł jest wymagane")]
    public string Title { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;

    //Relation with Comments
    public ICollection<Comments> CommentsList { get; set; } = new List<Comments>();
    // Relation with GamesCategories
    public List<GamesAndCategories> GamesAndCategoriesList { get; set; } = new();
    public List<GamesCategories> CategoryList { get; set; } = new();
}
