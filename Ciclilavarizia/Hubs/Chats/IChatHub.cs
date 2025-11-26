namespace AdventureWorks.Hubs.Chats
{
    public interface IChatHub
    {
        public Task ReceiveMessage(string user, string message);
    }
}
