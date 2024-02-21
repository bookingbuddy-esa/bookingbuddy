using Microsoft.AspNetCore.SignalR;

namespace BookingBuddy.Server.Controllers
{
    public class ChatHub : Hub
    {
        private readonly Dictionary<string, List<string>> _groupUsers = new Dictionary<string, List<string>>();

        public async Task JoinGroup(string groupName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            if (_groupUsers.ContainsKey(groupName))
            {
                _groupUsers[groupName].Add(userName);
            }
            else
            {
                _groupUsers[groupName] = new List<string> { userName };
            }

            await Clients.Group(groupName).SendAsync("NewUser", $"{userName} entrou no canal");
        }

        public async Task LeaveGroup(string groupName, string userName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            if (_groupUsers.ContainsKey(groupName))
            {
                _groupUsers[groupName].Remove(userName);
                await Clients.Group(groupName).SendAsync("LeftUser", $"{userName} saiu do canal");
            }
        }

        public async Task SendMessage(NewMessage message)
        {
            await Clients.Group(message.GroupName).SendAsync("NewMessage", message);
        }

        public List<string> GetUsers(string groupName)
        {
            if (_groupUsers.ContainsKey(groupName))
            {
                return _groupUsers[groupName];
            }

            return new List<string>();
        }
    }

    public record NewMessage(string UserName, string Message, string GroupName);
}