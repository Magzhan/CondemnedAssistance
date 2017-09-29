using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.IService {
    public interface IMessageSender {

        string Send(int userId, int senderId, string subject, string message);

    }
}
