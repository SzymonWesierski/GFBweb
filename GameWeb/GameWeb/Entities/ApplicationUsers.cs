using Microsoft.AspNetCore.Identity;

namespace GameWeb.Entities;

public class ApplicationUsers : IdentityUser
{
    public ICollection<Comments> CommentsList { get; set; } = new List<Comments>();
    public ICollection<Ratings> RatingsList { get; set; } = new List<Ratings>();
    public string? ImagePath { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
}


