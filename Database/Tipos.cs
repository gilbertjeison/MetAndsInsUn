using System;
using System.Collections.Generic;

namespace MetAndsInsUn.Database
{
    public partial class Tipos
    {
        public Tipos()
        {
            TiposData = new HashSet<TiposData>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<TiposData> TiposData { get; set; }
    }
}
