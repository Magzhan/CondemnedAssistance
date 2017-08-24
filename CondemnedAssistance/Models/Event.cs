using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.Models {
    public class Event : TemplateTable {
        public DateTime Date { get; set; }

        public int EventStatusId { get; set; }
        public EventStatus EventStatus { get; set; }
    }

    public class EventStatus : TemplateTable{
    }
}
