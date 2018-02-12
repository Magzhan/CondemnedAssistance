using CondemnedAssistance.Services.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Controller", Schema = Schemas.App)]
    public class Controller : TemplateTable {
    }

    [Table("Action", Schema = Schemas.App)]
    public class Action : TemplateTable {
        public int ControllerId { get; set; }
        public Controller Controller { get; set; }
    }
}
