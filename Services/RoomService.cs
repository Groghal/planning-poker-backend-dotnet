using PlanningPoker.Api.Models;

namespace PlanningPoker.Api.Services
{
    public class RoomService
    {
        private static readonly Dictionary<string, Room> _rooms = new();

        public Room CreateRoom(string? customRoomId = null, List<string>? voteOptions = null, string adminPassword = "")
        {
            var roomId = customRoomId ?? $"room-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            
            if (_rooms.ContainsKey(roomId))
            {
                throw new InvalidOperationException("Room ID already exists");
            }

            var defaultVoteOptions = new List<string> { "1", "2", "3", "5", "8", "13" };
            
            var room = new Room
            {
                RoomId = roomId,
                VoteOptions = voteOptions?.Count > 0 ? voteOptions : defaultVoteOptions,
                AdminPassword = adminPassword
            };

            _rooms[roomId] = room;
            return room;
        }

        public bool VerifyAdminPassword(string roomId, string adminPassword)
        {
            var room = GetRoom(roomId);
            return room.AdminPassword == adminPassword;
        }

        public (string UserId, Room Room) JoinRoom(string roomId, string username)
        {
            var room = GetRoom(roomId);
            var userId = $"user-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

            if (room.Users.Values.Any(u => u.Username == username))
            {
                throw new InvalidOperationException("Username already taken in this room");
            }

            room.Users[userId] = new User
            {
                Id = userId,
                Username = username,
                Vote = null
            };

            if (!room.Users.Any())
            {
                room.Host = userId;
            }

            return (userId, room);
        }

        public Room GetRoom(string roomId)
        {
            if (!_rooms.TryGetValue(roomId, out var room))
            {
                throw new KeyNotFoundException("Room not found");
            }
            return room;
        }

        public Room RevealVotes(string roomId)
        {
            var room = GetRoom(roomId);
            room.ShowVotes = true;
            return room;
        }

        public Room ResetVotes(string roomId)
        {
            var room = GetRoom(roomId);
            room.Votes.Clear();
            room.ShowVotes = false;

            foreach (var user in room.Users.Values)
            {
                user.Vote = null;
            }

            return room;
        }

        public void SubmitVote(string roomId, string username, string vote)
        {
            var room = GetRoom(roomId);
            var user = room.Users.Values.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found in room");
            }

            user.Vote = vote;
            room.Votes[username] = vote;
            
            if (room.Votes[username] != vote)
            {
                room.Votes[username] = vote;
            }
        }

        public void DeleteRoom(string roomId)
        {
            if (!_rooms.Remove(roomId))
            {
                throw new KeyNotFoundException("Room not found");
            }
        }
    }
} 