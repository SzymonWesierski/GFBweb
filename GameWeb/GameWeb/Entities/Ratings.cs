using System.ComponentModel.DataAnnotations;

namespace GameWeb.Entities;

public class Ratings
{
    [Key]
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    [Range(1, 10, ErrorMessage = "Please enter a valid number (1-10).")]
    [Required(ErrorMessage = "Musisz podać warotość oceny")]
    public int Value { get; set; } = 0;

    // Foreign Key to Users
    public string UserId { get; set; } = string.Empty;
    public ApplicationUsers User { get; set; } = new ApplicationUsers();

    // Foreign Key to Games
    public int GameId { get; set; }
    public Games Game { get; set; } = new Games();
}
