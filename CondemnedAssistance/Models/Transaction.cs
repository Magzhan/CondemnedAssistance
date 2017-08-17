using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Transaction {
        [Key]
        public long TransactionId { get; set; }
        public Guid TransactionGuid { get; set; }
    }
}
