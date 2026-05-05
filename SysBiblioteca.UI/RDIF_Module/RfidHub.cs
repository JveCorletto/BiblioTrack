using Microsoft.AspNetCore.SignalR;

namespace SysBiblioteca.UI.RDIF_Module
{
    public class RfidHub : Hub
    {
        public async Task SendTag(string tag)
        {
            await Clients.All.SendAsync("ReceiveTag", tag);
        }
    }

}