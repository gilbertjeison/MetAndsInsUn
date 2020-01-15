using System;
using System.Collections.Generic;

namespace MetAndsInsUn.Database
{
    public partial class Equipos
    {
        public Equipos()
        {
            Calibraciones = new HashSet<Calibraciones>();
            Programaciones = new HashSet<Programaciones>();
        }

        public long Id { get; set; }
        public string Código { get; set; }
        public string Nombre { get; set; }
        public string Rango { get; set; }
        public long? MedidaRango { get; set; }
        public string MaxRango { get; set; }
        public string DivisiónEscala { get; set; }
        public long? Marca { get; set; }
        public string Tipo { get; set; }
        public string Serie { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public long? Estatus { get; set; }
        public string BuscarPor { get; set; }
        public long? TipoInstrumento { get; set; }
        public long? Area { get; set; }
        public string CheckEliminados { get; set; }
        public string ImagePath { get; set; }
        public int? Frecuencia { get; set; }

        public virtual TiposData AreaNavigation { get; set; }
        public virtual TiposData EstatusNavigation { get; set; }
        public virtual TiposData MarcaNavigation { get; set; }
        public virtual TipoInstrumento TipoInstrumentoNavigation { get; set; }
        public virtual ICollection<Calibraciones> Calibraciones { get; set; }
        public virtual ICollection<Programaciones> Programaciones { get; set; }
    }
}
