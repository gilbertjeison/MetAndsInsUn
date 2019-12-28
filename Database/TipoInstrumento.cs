using System;
using System.Collections.Generic;

namespace MetAndsInsUn.Database
{
    public partial class TipoInstrumento
    {
        public TipoInstrumento()
        {
            Equipos = new HashSet<Equipos>();
        }

        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Equipos> Equipos { get; set; }
    }
}
