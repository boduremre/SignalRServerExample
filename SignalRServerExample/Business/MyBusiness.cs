using Microsoft.AspNetCore.SignalR;
using SignalRServerExample.Hubs;

namespace SignalRServerExample.Business
{
    public class MyBusiness
    {
        readonly IHubContext<MyHub> _hubContext;

        public MyBusiness(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext; 
        }

        public async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", string.Format("Gönderdiğiniz Mesaj: {0}", message));
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Lütfen mesaj giriniz.");
            }
        }
    }
}
