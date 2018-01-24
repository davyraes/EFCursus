using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstCursus
{
    [Table("Cursisten") ]
    public class Cursist
    {
        [Key]
        public int CursistId { get; set; }
        public String Voornaam { get; set; }
        public string Familienaam { get; set; }
      
        public int? MentorId { get; set; }
        public virtual ICollection<Cursist> Beschermelingen { get; set; }
        [ForeignKey("MentorId")][InverseProperty("Beschermelingen")]
        public virtual Cursist Mentor { get; set; }
    }
}
