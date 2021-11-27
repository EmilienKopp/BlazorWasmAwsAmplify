using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWasmAwsAmplify.Services
{
    public class NotificationService
    {
        public string WebSocketEndpoint { get; set; }
        public enum WebSocketStatus { Closed, Open, Error }
        public bool IsConnected { get; set; } = false;
        public HubConnection _connection { get; set; }
        public List<string> Notifications { get; set; }
        public WebSocketStatus ConnectionStatus { get; set; }

        public NotificationService()
        {
            ConnectionStatus = WebSocketStatus.Closed;
            IsConnected = false;
        }

        public async Task ConnectToServer(string URL)
        {
            this.WebSocketEndpoint = URL;
            _connection = new HubConnectionBuilder().WithAutomaticReconnect()
                                                    .WithUrl(WebSocketEndpoint)
                                                    .Build();
            await _connection.StartAsync();

            IsConnected = true;
            ConnectionStatus = WebSocketStatus.Open;

        }



    }
}
