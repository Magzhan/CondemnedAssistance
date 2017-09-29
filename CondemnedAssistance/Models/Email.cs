using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Email {
        [Key]
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }

    public class EmailExchange {
        public long Id { get; set; }
        public long EmailId { get; set; }
        public Email Email { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public bool IsSuccessfullySent { get; set; }
        public string ReceiverEmail { get; set; }
        public DateTime SentDate { get; set; }
    }
}
