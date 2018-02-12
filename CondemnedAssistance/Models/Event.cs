using CondemnedAssistance.Services.Database;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Event", Schema = Schemas.App)]
    public class Event : TemplateTable {
        public DateTime Date { get; set; }

        public int EventStatusId { get; set; }
        public EventStatus EventStatus { get; set; }

        public int UserId { get; set; }
        //public User User { get; set; }
    }

    [Table("EventStatus", Schema = Schemas.App)]
    public class EventStatus : TemplateTable{
    }
}
