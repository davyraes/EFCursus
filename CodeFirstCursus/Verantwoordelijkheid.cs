using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstCursus
{
    [Table("Verantwoordelijkheden")]
    public class Verantwoordelijkheid
    {
        public int VerantwoordelijkheidId { get; set; }
        public string Naam { get; set; }
        public virtual ICollection<Instructeur> Instructeurs { get; set; }
    }
}
