namespace PlanningPoker.Api.Models
{
    public class Room
    {
        public string RoomId { get; set; } = string.Empty;
        public Dictionary<string, User> Users { get; set; } = new();
        public Dictionary<string, string> Votes { get; set; } = new();
        public bool ShowVotes { get; set; }
        public string Host { get; set; } = string.Empty;
        public List<string> VoteOptions { get; set; } = new() { "1", "2", "3", "5", "8", "13" };
    }
} 