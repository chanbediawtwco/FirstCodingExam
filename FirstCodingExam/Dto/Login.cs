using System.ComponentModel.DataAnnotations;

namespace FirstCodingExam.Dto;

public partial class Login
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
