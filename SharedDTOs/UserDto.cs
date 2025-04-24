using System.ComponentModel.DataAnnotations;

namespace SharedDtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public int Points { get; set; }
        public required string Role { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "User";

        public int Points { get; set; } = 0;
    }

    public class UpdateUserDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; }

        public int Points { get; set; }
    }
}