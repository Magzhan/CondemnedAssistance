using CondemnedAssistance.Services.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Help", Schema = Schemas.App)]
    public class Help : TemplateTable {
    }
}
