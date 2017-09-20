using System;
using System.ComponentModel.DataAnnotations;

namespace CondemnedAssistance.Models {
    public class Template {
    }

    public class TemplateTable : TrackingTemplate {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }

    public class TrackingTemplate {
        public int RequestUser { get; set; }
        public DateTime RequestDate { get; set; }
    }

    public class HistoryTemplate : TrackingTemplate{
        public long TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public DatabaseActionTypes ActionType { get; set; }
    }

    public enum DatabaseActionTypes {
        Insert = 1, Update = 2, Delete = 3
    }
}
