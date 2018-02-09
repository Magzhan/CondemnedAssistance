using CondemnedAssistance.Services.Database;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Transaction", Schema = Schemas.User)]
    public class Transaction {
        [Key]
        public long TransactionId { get; set; }
        public Guid TransactionGuid { get; set; }
    }
}
