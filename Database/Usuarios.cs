using System;
using System.Collections.Generic;

namespace MetAndsInsUn.Database
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            Calibraciones = new HashSet<Calibraciones>();
            Programaciones = new HashSet<Programaciones>();
        }

        public long Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Calibraciones> Calibraciones { get; set; }
        public virtual ICollection<Programaciones> Programaciones { get; set; }
    }
}
