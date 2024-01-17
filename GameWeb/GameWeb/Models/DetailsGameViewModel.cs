using GameWeb.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class DetailsGameViewModel
{
    public Games? Game { get; set; }
    public Ratings? Rating { get; set; }
    public Comments? Comment { get; set; }
}