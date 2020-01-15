using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetAndsInsUn.Models
{
    public class CalibracionesModel
    {
        public long id { get; set; }
        public long idEquipo { get; set; }
        public string fecha { get; set; }
        public string tipo { get; set; }
        public string observaciones { get; set; }
        public string file_path { get; set; }
        public long idUsuario { get; set; }
        public string nombresUsuario { get; set; }
    }
}
