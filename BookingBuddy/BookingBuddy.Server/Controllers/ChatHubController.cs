using Microsoft.AspNetCore.SignalR;

namespace BookingBuddy.Server.Controllers
{
    // TODO: isto tem de ser refeito/melhorado quando for altura de comunicação entre membros de grupo
    // passar a usar o username do Identity em vez de uma string passada por parâmetro
    /// <summary>
    /// Controlador para o chat.
    /// </summary>
    public class ChatHubController : Hub
    {
        private static Dictionary<string, List<UserInfo>> _groupUsers = new Dictionary<string, List<UserInfo>>();

        /// <summary>
        /// Permite que um utilizador entre num grupo.
        /// </summary>
        /// <param name="groupName">O nome do grupo.</param>
        /// <param name="userName">O nome de utilizador que está a entrar no grupo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
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

        /// <summary>
        /// Permite que um utilizador saia de um grupo.
        /// </summary>
        /// <param name="groupName">O nome do grupo.</param>
        /// <param name="userName">O nome de utilizador que está a sair do grupo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
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

        /// <summary>
        /// Envia uma nova mensagem para um grupo específico.
        /// </summary>
        /// <param name="message">Os dados da nova mensagem a ser enviada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task SendMessage(NewMessage message)
        {
            await Clients.Group(message.GroupName).SendAsync("NewMessage", message);
        }

        /// <summary>
        /// Obtém a lista de utilizadores de um grupo específico.
        /// </summary>
        /// <param name="groupName">O nome do grupo.</param>
        /// <returns>Uma lista que contém os nomes dos utilizadores do grupo.</returns>
        public static List<string> GetUsers(string groupName)
        {
            if (_groupUsers.ContainsKey(groupName))
            {
                return _groupUsers[groupName].Select(user => user.UserName).ToList();
            }

            return new List<string>();
        }

        /// <summary>
        /// Executa quando um utilizador se desconecta.
        /// </summary>
        /// <param name="exception">A exceção (se houver) que causou a desconexão.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
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

    /// <summary>
    /// Representa os dados de um utilizador.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Nome do utilizador.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Identificador da conexão.
        /// </summary>
        public string ConnectionId { get; set; }
    }

    /// <summary>
    /// Representa os dados de uma nova mensagem para um grupo.
    /// </summary>
    /// <param name="UserName">O nome do utilizador que enviou a mensagem.</param>
    /// <param name="Message">O conteúdo da mensagem.</param>
    /// <param name="GroupName">O nome do grupo para o qual a mensagem será enviada.</param>
    public record NewMessage(string UserName, string Message, string GroupName);
}