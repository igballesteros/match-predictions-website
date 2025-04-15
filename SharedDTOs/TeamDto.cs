namespace SharedDtos
{
    public class TeamDto
    {
        public int TeamId { get; set; }
        public required string Teamname { get; set; }
        public string? LogoUrl { get; set; }
        public int TotalWins { get; set; } = 0;
    }
}