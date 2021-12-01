using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorWasmAwsAmplify.Services
{
    public enum NotificationType
    {
        Order,
        CustomerControl,
        Incoming
    }
    public class Notification
    {
        public string Date { get; set; }
        public string Content { get; set; }
        public bool Displayed { get; set; } = true;
        public NotificationType Type { get; set; }

        public Notification(string date, string content)
        {
            Date = date;
            Content = content;
        }
    }

    public enum ToastLevel { Info, Success, Warning, Error }

    public class NotificationService : IDisposable
    {
        public string WebSocketEndpoint { get; set; }
        public ClientWebSocket Socket { get; set; }
        public bool IsConnected { get; set; } = false;
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public CancellationTokenSource CancellationSource { get; set; }

        public event Action<string, ToastLevel, bool> OnShow;
        public event Action OnHide;
        private System.Timers.Timer Countdown;

        public NotificationService()
        {
            IsConnected = false;
            CancellationSource = new CancellationTokenSource();
            Socket = new ClientWebSocket();
        }//ctor

        public NotificationService(string endpoint)
        {
            IsConnected = false;
            WebSocketEndpoint = endpoint;
            Socket = new ClientWebSocket();
            CancellationSource = new CancellationTokenSource();
        }//ctor

        public NotificationService(string endpoint, bool connect)
        {
            WebSocketEndpoint = endpoint;
            CancellationSource = new CancellationTokenSource();
            Socket = new ClientWebSocket();
            if (connect)
            {
                this.Socket.ConnectAsync(new Uri(endpoint), CancellationSource.Token);
            }
            IsConnected = connect;
        }//ctor

        public async Task Connect(string endpoint)
        {
            WebSocketEndpoint = endpoint;
            await this.Socket.ConnectAsync(new Uri(endpoint), CancellationSource.Token);
            IsConnected = true;
        }//Connect

        public async Task Broadcast(string notif)
        {
            Notifications.Add(new Notification(DateTime.Now.ToString("yyyy/MM/dd HH:mm"),notif));
            var buffer = new byte[1024 * 4];
            buffer = Encoding.UTF8.GetBytes(notif);
            await this.Socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }//Broadcast

        public void ClearAll()
        {
            Notifications.Clear();
        }//ClearAll

        public async Task Receive()
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            var received = await Socket.ReceiveAsync(buffer, CancellationSource.Token);
            string receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);

            Notification notification = new Notification(DateTime.Now.ToString("yyyy/MM/dd HH:mm"), receivedAsText);

            if (receivedAsText.Contains("CUSTOMER INFO"))
            {
                notification.Type = NotificationType.CustomerControl;
            }
            else if (receivedAsText.Contains("来客"))
            {
                notification.Type = NotificationType.Incoming;
            }
            else
            {
                notification.Type = NotificationType.Order;
            }

            Notifications.Add(notification);
        }//Receive

        public async Task Disconnect()
        {
            CancellationSource.Cancel();
            IsConnected = false;
            await this.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
        }//Disconnect


        /*TOAST MANAGEMENT */
        public void ShowToast(string message, ToastLevel level, bool isInfiniteTimer)
        {
            OnShow?.Invoke(message, level, isInfiniteTimer);
            StartCountdown(isInfiniteTimer);
        }

        private void StartCountdown(bool isInfiniteTimer)
        {
            SetCountdown(isInfiniteTimer);

            if (Countdown.Enabled)
            {
                Countdown.Stop();
                Countdown.Start();
            }
            else
            {
                Countdown.Start();
            }
        }

        private void SetCountdown(bool infinite)
        {
            if (Countdown == null)
            {
                Countdown = new System.Timers.Timer(15000);
                if (!infinite) Countdown.Elapsed += HideToast;
                Countdown.AutoReset = infinite;
            }
        }

        private void HideToast(object source, ElapsedEventArgs args)
        {
            OnHide?.Invoke();
        }


        public void Dispose()
        {
            Countdown?.Dispose();
        }

    }
}
