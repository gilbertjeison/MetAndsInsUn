using System;
using System.Collections.Generic;

namespace MetAndsInsUn.Database
{
    public partial class Calibraciones
    {
        public long Id { get; set; }
        public long? IdEquipo { get; set; }
        public string CalibrationFilePath { get; set; }
        public DateTime? Fecha { get; set; }
        public string Observaciones { get; set; }
        public long? IdUsuario { get; set; }
        public string Tipo { get; set; }

        public virtual Equipos IdEquipoNavigation { get; set; }
        public virtual Usuarios IdUsuarioNavigation { get; set; }
    }
}
