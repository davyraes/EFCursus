using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstCursus
{
    [Table("Campussen")]public class Campus
    {
        public virtual ICollection<Instructeur> Instructeurs { get; set; }
        public int CampusId { get; set; }
        [Required][StringLength(50)]public string Naam { get; set; }
        public Adres Adres { get; set; }
    }
}
