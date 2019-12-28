using System;
using System.Collections.Generic;

namespace MetAndsInsUn.Database
{
    public partial class Programaciones
    {
        public long Id { get; set; }
        public long? IdEquipo { get; set; }
        public int? Ano { get; set; }
        public int? Semana { get; set; }
        public long? IdEstado { get; set; }
        public DateTime? Fecha { get; set; }
        public long? IdUsuario { get; set; }
        public string Observaciones { get; set; }

        public virtual Equipos IdEquipoNavigation { get; set; }
        public virtual TiposData IdEstadoNavigation { get; set; }
        public virtual Usuarios IdUsuarioNavigation { get; set; }
    }
}
