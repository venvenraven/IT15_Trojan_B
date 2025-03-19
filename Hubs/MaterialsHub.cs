using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IT15_Trojan_B.Hubs
{
    public class MaterialsHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
