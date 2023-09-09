using System.ComponentModel.DataAnnotations;

namespace BrMpGame.Features.Accounts.Registration;

public class RegisterRequest
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Имя")]
    public string UserName { get; set; } = null!;
}