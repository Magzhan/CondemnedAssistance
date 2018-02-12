using CondemnedAssistance.Services.Database;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Email", Schema = Schemas.User)]
    public class Email {
        [Key]
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }

    [Table("EmailExchange", Schema = Schemas.User)]
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
