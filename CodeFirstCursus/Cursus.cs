using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstCursus
{[Table("Cursussen")]
    public abstract class Cursus
    {
        public int Id { get; set; }
        public string Naam { get; set; }
    }
}
