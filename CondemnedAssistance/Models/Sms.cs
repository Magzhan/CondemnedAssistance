using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Sms {
        [Key]
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }

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
