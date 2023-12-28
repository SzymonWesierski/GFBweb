using System.ComponentModel.DataAnnotations;

namespace GameWeb.Entities;

public class Comments
{
    [Key]
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Musisz podać treść komentarza")]
    public string Content { get; set; } = string.Empty;
    public int LikesCounter { get; set; } = 0;
    public int DislikesCounter { get; set; } = 0;

    // Foreign Key to Users
    public string UserId { get; set; } = string.Empty;
    public ApplicationUsers User { get; set; } = new ApplicationUsers();

    // Foreign Key to Games
    public int GameId { get; set; }
    public Games Game { get; set; } = new Games();
}
