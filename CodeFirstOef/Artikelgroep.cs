using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstOef
{
    [Table("ArtikelGroepen")]
    public class Artikelgroep
    {       
        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        public virtual ICollection<Artikel> Artikels { get; set; }
    }
}
