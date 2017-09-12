using System;

namespace CondemnedAssistance.Models {
    public class Event : TemplateTable {
        public DateTime Date { get; set; }

        public int EventStatusId { get; set; }
        public EventStatus EventStatus { get; set; }
    }

    public class EventStatus : TemplateTable{
    }
}
