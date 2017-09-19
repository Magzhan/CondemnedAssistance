using CondemnedAssistance.Services.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;

namespace CondemnedAssistance.Hubs {
    public class WebSocketMessageHandler : WebSocketHandler
    {
        public WebSocketMessageHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer) {
            throw new NotImplementedException();
        }
    }
}
