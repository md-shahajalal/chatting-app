using System.ComponentModel.DataAnnotations;

namespace chat_app.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(8, MinimumLength =3)]
        public string Password { get; set; } = String.Empty;
    }
}
