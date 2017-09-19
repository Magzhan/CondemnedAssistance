using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.WebSockets {
    public class WebSocketManagerMiddleware {

        private readonly RequestDelegate _next;
        private WebSocketHandler _webSocketHandler;

        public WebSocketManagerMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler) {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context) {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
            // connect to list in socket handler here 
            Dictionary<string, StringValues> query = QueryHelpers.ParseQuery(context.Request.QueryString.Value);

            List<KeyValuePair<string, string>> items = query.SelectMany(q => q.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

            int userId = Convert.ToInt32(items.Where(i => i.Key == "userId").Single().Value);
            int registerId = Convert.ToInt32(items.Where(i => i.Key == "registerId").Single().Value);
            int roleId = Convert.ToInt32(items.Where(i => i.Key == "roleId").Single().Value);

            await _webSocketHandler.OnConnected(userId, registerId, roleId, socket);

            await Receive(socket, async(result, buffer) => {
                if(result.MessageType == WebSocketMessageType.Text) {
                    await _webSocketHandler.ReceiveAsync(socket, result, buffer);
                    return;
                }else if (result.MessageType == WebSocketMessageType.Close) {
                    await _webSocketHandler.OnDisconnected(socket);
                    return;
                }
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage) {
            byte[] buffer = new byte[1024 * 4];

            while(socket.State == WebSocketState.Open) {
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}
