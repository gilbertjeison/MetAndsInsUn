using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetAndsInsUn.Models
{
    public class UsuariosModel
    {
        public long id { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string nombreCompleto { get; set; }
    }
}
