namespace webapi_pred.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public int Points { get; set; }
    }
}