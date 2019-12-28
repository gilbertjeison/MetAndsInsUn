using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetAndsInsUn.Models
{
    public class ProgramacionesModel
    {
        public long id { get; set; }
        public string codigoEquipo { get; set; }
        public long idEquipo { get; set; }
        public string descEquipo { get; set; }
        public int ano { get; set; }
        public int semana { get; set; }
        public DateTime fecha { get; set; }
        public long idEstado { get; set; }
        public string descEstado { get; set; }
        public long idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string observaciones { get; set; }
        public int frecuencia { get; set; }
        public long idArea { get; set; }
        public int mes { get; set; }
    }
}
