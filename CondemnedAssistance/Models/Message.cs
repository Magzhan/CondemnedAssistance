using System;

namespace CondemnedAssistance.Models {
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
    }

    public class MessageExchange {
        public long Id { get; set; }

        public long MessageId { get; set; }
        public Message Message { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public int HelpId { get; set; }
        public Help Help { get; set; }
    }
}
