using CondemnedAssistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CondemnedAssistance.ViewModels {
    public class EventModel {
        public int UserId { get; set; }

        public string CurrentUserStatus { get; set; }

        public List<Event> Events { get; set; }

        public EventModel() {
            Events = new List<Event>();
        }
    }

    public class EventCreateModel : TemplateTable{

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public int EventStatusId { get; set; }

        public List<EventStatus> EventStatuses { get; set; }

        public EventCreateModel() {
            EventStatuses = new List<EventStatus>();
        }
    }
}
