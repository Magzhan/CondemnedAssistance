using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Event", Schema = "app")]
    public class Event : TemplateTable {
        public DateTime Date { get; set; }

        public int EventStatusId { get; set; }
        public EventStatus EventStatus { get; set; }

        public int UserId { get; set; }
    }

    [Table("EventStatus", Schema = "app")]
    public class EventStatus : TemplateTable{
    }
}
