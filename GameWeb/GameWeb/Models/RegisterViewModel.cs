using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;
public class RegisterViewModel
{
    [Required(ErrorMessage = "Podaj e-mail")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage = "Podaj nazwę użytkownika")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Podaj hasło")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Podaj hasło")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Hasło i potwierdź hasło muszą być identyczne")]
    public string ConfirmPassword { get; set; }
}
