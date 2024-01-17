using GameWeb.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class EditAndDeleteCommentViewModel
{
    public int CommentId{ get; set; }
    public int GameId { get; set; }
    [Required(ErrorMessage = "Musisz podać treść komentarza")]
    public string CommentContent { get; set; }
}