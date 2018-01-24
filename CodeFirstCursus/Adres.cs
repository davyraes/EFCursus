using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace CodeFirstCursus
{
    [ComplexType]
    public class Adres
    {
        [Column("straat")]
        public string Straat { get; set; }
        [Column("HuisNr")]
        public string HuisNr { get; set; }
        [Column("PostCode")]
        public string PostCode { get; set; }
        [Column("Gemeente")]
        public string Gemeente { get; set; }
    }
}
