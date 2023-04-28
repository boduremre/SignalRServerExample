using Microsoft.AspNetCore.SignalR;

namespace SignalRServerExample.Hubs
{
    public class MyHub : Hub
    {
        public async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await Clients.All.SendAsync("ReceiveMessage", string.Format("Gönderdiğiniz Mesaj: {0}",message));
            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage", "Lütfen mesaj giriniz.");
            }
        }

        public override Task OnConnectedAsync()
        {
            // bağlanan clienta atanan uniqe id bilgisi
            string id = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // bağlantısı kopan veya bağlantıyı kapatan clienta atanan uniqe id bilgisi
            string id = Context.ConnectionId;
            return base.OnDisconnectedAsync(exception);
        }
    }
}
