using Microsoft.AspNetCore.SignalR;

namespace LongRunningJobs.Api.Hubs;

public class JobProgressHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnConnectedAsync()
    {
        // Join default group for all job updates
        await Groups.AddToGroupAsync(Context.ConnectionId, "JobUpdates");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "JobUpdates");
        await base.OnDisconnectedAsync(exception);
    }
}