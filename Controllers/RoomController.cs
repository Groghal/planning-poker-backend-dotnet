using Microsoft.AspNetCore.Mvc;
using PlanningPoker.Api.Models;
using PlanningPoker.Api.Services;

namespace PlanningPoker.Api.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost]
        public IActionResult CreateRoom([FromBody] CreateRoomRequest request)
        {
            try
            {
                var room = _roomService.CreateRoom(request.RoomId, request.VoteOptions);
                return CreatedAtAction(nameof(GetRoom), new { roomId = room.RoomId }, 
                    new { roomId = room.RoomId, voteOptions = room.VoteOptions });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{roomId}/join")]
        public IActionResult JoinRoom(string roomId, [FromBody] JoinRoomRequest request)
        {
            try
            {
                var (userId, _) = _roomService.JoinRoom(roomId, request.Username);
                return Ok(new { message = "Joined room", userId });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Room not found" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{roomId}/reveal")]
        public IActionResult RevealVotes(string roomId)
        {
            try
            {
                _roomService.RevealVotes(roomId);
                return Ok(new { message = "Votes revealed" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Room not found" });
            }
        }

        [HttpPost("{roomId}/reset")]
        public IActionResult ResetVotes(string roomId)
        {
            try
            {
                _roomService.ResetVotes(roomId);
                return Ok(new { message = "Round reset" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Room not found" });
            }
        }

        [HttpGet("{roomId}")]
        public IActionResult GetRoom(string roomId)
        {
            try
            {
                var room = _roomService.GetRoom(roomId);
                var response = new
                {
                    users = room.Users,
                    showVotes = room.ShowVotes,
                    host = room.Host,
                    votes = room.ShowVotes ? room.Votes : null
                };
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Room not found" });
            }
        }

        [HttpGet("{roomId}/vote-options")]
        public IActionResult GetVoteOptions(string roomId)
        {
            try
            {
                var room = _roomService.GetRoom(roomId);
                return Ok(room.VoteOptions);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Room not found" });
            }
        }

        [HttpDelete("{roomId}")]
        public IActionResult DeleteRoom(string roomId)
        {
            try
            {
                _roomService.DeleteRoom(roomId);
                return Ok(new { message = "Room deleted successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Room not found" });
            }
        }

        [HttpPost("{roomId}/vote")]
        public IActionResult SubmitVote(string roomId, [FromBody] SubmitVoteRequest request)
        {
            try
            {
                _roomService.SubmitVote(roomId, request.Username, request.Vote);
                return Ok(new { message = "Vote recorded" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

    public class CreateRoomRequest
    {
        public string? RoomId { get; set; }
        public List<string>? VoteOptions { get; set; }
    }

    public class JoinRoomRequest
    {
        public string Username { get; set; } = string.Empty;
    }

    public class SubmitVoteRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Vote { get; set; } = string.Empty;
    }
} 