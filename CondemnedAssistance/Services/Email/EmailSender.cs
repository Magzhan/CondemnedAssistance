using CondemnedAssistance.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Email {
    public class EmailSender : IMessageSender {

        string IMessageSender.Send(int userId, int senderId, string subject, string message) {
            throw new NotImplementedException();
        }
    }
}
