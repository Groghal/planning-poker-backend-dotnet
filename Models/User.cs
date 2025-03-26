namespace PlanningPoker.Api.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Vote { get; set; }
    }
} 