using Microsoft.AspNetCore.SignalR;

namespace AdventureWorks.Hubs.Chats
{
    public class ChatHub : Hub<IChatHub>
    {
        public async Task SendMessage(string user, string message)
                => await Clients.All.ReceiveMessage(user, message);

        public async Task SendMessageToCaller(string user, string message)
            => await Clients.Caller.ReceiveMessage(user, message);

    }
}
