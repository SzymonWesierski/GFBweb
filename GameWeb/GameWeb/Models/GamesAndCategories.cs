
namespace GameWeb.Models;

public class GamesAndCategories
{
    public int GameId { get; set; }
    public Games Game { get; set; } = null!;

    public int GameCategoryId { get; set; }
    public GamesCategories GameCategory { get; set; } = null!;
}