using Microsoft.AspNetCore.SignalR;

namespace PlanningPoker.Api.Hubs;

public class PokerHub : Hub
{
    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task SendSmile(string roomId, string toUsername, string emoji = "😃")
    {
        // Broadcast to everyone in the room so all clients render the same animation/emoji.
        var safeEmoji = string.IsNullOrWhiteSpace(emoji) ? "😃" : emoji;
        await Clients.Group(roomId).SendAsync("ReceiveSmile", toUsername, safeEmoji);
    }
}
