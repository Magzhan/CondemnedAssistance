using CondemnedAssistance.Services.Database;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Message", Schema = Schemas.App)]
    public class Message {
        public long Id { get; set; }
        public string Text { get; set; }

        public bool IsSent { get; set; }
        public DateTime SentDate { get; set; }

        public bool IsReceived { get; set; }
        public DateTime ReceivedDate { get; set; }

        public bool IsRead { get; set; }
        public DateTime ReadDate { get; set; }

        public int SenderId { get; set; }
        //public User Sender { get; set; }
    }

    [Table("MessageExchange", Schema = Schemas.App)]
    public class MessageExchange {
        public long Id { get; set; }

        public long MessageId { get; set; }
        public Message Message { get; set; }

        public int SenderId { get; set; }
        //public User Sender { get; set; }

        public int ReceiverId { get; set; }
        //public User Receiver { get; set; }

        public int HelpId { get; set; }
        public Help Help { get; set; }
    }
}
