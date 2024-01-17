using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class CommentFormPartialViewModel
{
    [Required(ErrorMessage ="Musisz podać treść komentarza")]
    public string CommentContent { get; set; }
    public int GameId {  get; set; }

}
