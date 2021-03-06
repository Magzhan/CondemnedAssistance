﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.WebSockets {
    public abstract class WebSocketHandler {
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }

        private async Task doSomething() => await Task.Delay(1).ConfigureAwait(true);

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager) {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task OnConnected(int id, int registerId, int roleId, WebSocket socket) {
            await doSomething();
            WebSocketConnectionManager.AddSocket(id, new WebSocketResources { Socket = socket, RegisterId = registerId, RoleId = roleId });
        }

        public virtual async Task OnDisconnected(WebSocket socket) {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetUserId(socket));
        }

        public async Task SendMessageAsync(WebSocket socket, string message) {
            if (socket.State != WebSocketState.Open)
                return;

            byte[] messageInBytes = Encoding.UTF8.GetBytes(message);

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: messageInBytes, offset: 0, count: messageInBytes.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(int receiverId, int registerId, int roleId, string message) {
            await SendMessageAsync(WebSocketConnectionManager.GetSocketByUserId(receiverId), message);
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

        public WebSocket GetCurrentUserSocket(int id) {
            return WebSocketConnectionManager.GetSocketByUserId(id);
        }
    }
}
