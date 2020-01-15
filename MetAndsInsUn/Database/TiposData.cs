using System;
using System.Collections.Generic;

namespace MetAndsInsUn.Database
{
    public partial class TiposData
    {
        public TiposData()
        {
            EquiposAreaNavigation = new HashSet<Equipos>();
            EquiposEstatusNavigation = new HashSet<Equipos>();
            EquiposMarcaNavigation = new HashSet<Equipos>();
            Programaciones = new HashSet<Programaciones>();
        }

        public long Id { get; set; }
        public int? IdTipo { get; set; }
        public string Descripcion { get; set; }

        public virtual Tipos IdTipoNavigation { get; set; }
        public virtual ICollection<Equipos> EquiposAreaNavigation { get; set; }
        public virtual ICollection<Equipos> EquiposEstatusNavigation { get; set; }
        public virtual ICollection<Equipos> EquiposMarcaNavigation { get; set; }
        public virtual ICollection<Programaciones> Programaciones { get; set; }
    }
}
