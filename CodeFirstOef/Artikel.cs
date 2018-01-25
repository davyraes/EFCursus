using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOef
{
    [Table("Artikels")]
    public abstract class Artikel
    {
        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        public int? ArtikelgroepId { get; set; }
        public virtual Artikelgroep Artikelgroep { get; set; }
        public virtual ICollection<Leverancier> Leveranciers { get; set; }
    }
}
