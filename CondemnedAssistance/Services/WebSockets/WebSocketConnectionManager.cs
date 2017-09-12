using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.WebSockets {
    public class WebSocketConnectionManager {
        private ConcurrentDictionary<int, WebSocketResources> _sockets = new ConcurrentDictionary<int, WebSocketResources>();

        public WebSocket GetSocketByUserId(int id) {
            return _sockets[id].Socket;
        }

        public ConcurrentDictionary<int, WebSocketResources> GetAllSockets() {
            return _sockets;
        }

        public int GetUserId(WebSocket socket) {
            return _sockets.SingleOrDefault(s => s.Value.Socket == socket).Key;
        }

        public void AddSocket(int id, WebSocketResources socket) {
            _sockets.TryAdd(id, socket);
        }

        public async Task RemoveSocket(int id) {
            WebSocketResources socket;
            _sockets.TryRemove(id, out socket);

            await socket.Socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by Web Socket Manager",
                cancellationToken: CancellationToken.None);
        }
    }

    public class WebSocketResources {
        public WebSocket Socket { get; set; }

        public int RegisterId { get; set; }

        public int RoleId { get; set; }
    }
}
