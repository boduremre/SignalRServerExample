using Microsoft.AspNetCore.SignalR;

namespace SignalRServerExample.Hubs
{
    public class MyHub : Hub<IMessageClient>
    {
        static List<string> clientsList = new List<string>();

        public async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await Clients.All.ReceiveMessage(string.Format("{0}: {1}", Context.ConnectionId, message));
            }
            else
            {
                await Clients.All.ReceiveMessage("Lütfen mesaj giriniz.");
            }
        }

        public override async Task OnConnectedAsync()
        {
            // bağlanan clienta atanan uniqe id bilgisi
            string id = Context.ConnectionId;
            
            //yeni gelen kullanıcı client listesine ekleniyor
            clientsList.Add(id);

            //client listesi
            await Clients.All.Clients(clientsList);

            // client joined
            await Clients.All.UserJoined(id);

            //return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // bağlantısı kopan veya bağlantıyı kapatan clienta atanan uniqe id bilgisi
            string id = Context.ConnectionId;

            //çıkış yapan kullanıcı client listesinden siliniyor
            clientsList.Remove(id);

            //client listesi
            await Clients.All.Clients(clientsList);

            // ayrılan kullanıcının id si gönderiliyor
            await Clients.All.UserLeaved(id);
            //return base.OnDisconnectedAsync(exception);
        }
    }

    // bu interface sayesinde client üzerinde string olarak ismi verilen fonksiyonun
    // tetiklenmesi yerine interface içinde tanımlanan metot isimlerinin tetiklenmesini sağlayacaktır.
    public interface IMessageClient
    {
        Task Clients(List<string> clients);
        Task UserJoined(string connectionId);
        Task UserLeaved(string connectionId);
        Task ReceiveMessage(string message);
    }
}