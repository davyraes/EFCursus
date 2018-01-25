using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOef
{
    [Table("Leveranciers")]
    public class Leverancier
    {
        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        public virtual ICollection<Artikel> Artikels { get; set; }
    }
}
