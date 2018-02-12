using CondemnedAssistance.Services.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondemnedAssistance.Models {
    [Table("Profession", Schema = Schemas.App)]
    public class Profession : TemplateTable{
    }
}
