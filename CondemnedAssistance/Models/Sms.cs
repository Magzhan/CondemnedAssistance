using CondemnedAssistance.Services.Database;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Sms", Schema = Schemas.User)]
    public class Sms {
        [Key]
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }

    [Table("SmsExchange", Schema = Schemas.User)]
    public class SmsExchange {
        public long Id { get; set; }
        public long SmsId { get; set; }
        public Sms Sms { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public bool IsSuccessfullySent { get; set; }
        public string ReceiverPhone { get; set; }
        public DateTime SentDate { get; set; }
    }
}
