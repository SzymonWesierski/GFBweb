using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class LoginViewModel
{
    [Required(ErrorMessage ="Podaj nazwę użytkownika")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Podaj hasło")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
