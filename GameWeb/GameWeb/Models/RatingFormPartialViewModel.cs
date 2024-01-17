using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class RatingFormPartialViewModel
{
    public int RatingValue { get; set; }
    public int GameId {  get; set; }

}
