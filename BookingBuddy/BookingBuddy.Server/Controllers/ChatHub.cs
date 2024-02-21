using Microsoft.AspNetCore.SignalR;

namespace BookingBuddy.Server.Controllers
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, List<UserInfo>> _groupUsers = new Dictionary<string, List<UserInfo>>();
        public async Task JoinGroup(string groupName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            if (_groupUsers.ContainsKey(groupName))
            {
                _groupUsers[groupName].Add(new UserInfo { UserName = userName, ConnectionId = Context.ConnectionId });
            }
            else
            {
                _groupUsers[groupName] = new List<UserInfo> { new UserInfo { UserName = userName, ConnectionId = Context.ConnectionId } };
            }

            await Clients.Group(groupName).SendAsync("UserList", GetUsers(groupName));
            await Clients.Group(groupName).SendAsync("NewUser", $"{userName} entrou no canal");
        }

        public async Task LeaveGroup(string groupName, string userName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            if (_groupUsers.ContainsKey(groupName))
            {
                var user = _groupUsers[groupName].FirstOrDefault(u => u.UserName == userName && u.ConnectionId == Context.ConnectionId);

                if (user != null)
                {
                    _groupUsers[groupName].Remove(user);

                    await Clients.Group(groupName).SendAsync("UserList", GetUsers(groupName));
                    await Clients.Group(groupName).SendAsync("LeftUser", $"{userName} saiu do canal");
                }
            }
        }

        public async Task SendMessage(NewMessage message)
        {
            await Clients.Group(message.GroupName).SendAsync("NewMessage", message);
        }

        public static List<string> GetUsers(string groupName)
        {
            if (_groupUsers.ContainsKey(groupName))
            {
                return _groupUsers[groupName].Select(user => user.UserName).ToList();
            }

            return new List<string>();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var groupName = _groupUsers.FirstOrDefault(x => x.Value.Any(y => y.ConnectionId == Context.ConnectionId)).Key;
            var user = _groupUsers[groupName].FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _groupUsers[groupName].Remove(user);

            await Clients.Group(groupName).SendAsync("UserList", GetUsers(groupName));
            await Clients.Group(groupName).SendAsync("LeftUser", $"{user.UserName} saiu do canal");

            await base.OnDisconnectedAsync(exception);
        }
    }

    public class UserInfo
    {
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
    }

    public record NewMessage(string UserName, string Message, string GroupName);
}
