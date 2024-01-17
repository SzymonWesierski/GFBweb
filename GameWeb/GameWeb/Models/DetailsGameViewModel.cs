using GameWeb.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class DetailsGameViewModel
{
    public Games? Game { get; set; }
    public Ratings? Ratings { get; set; }
}