using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetAndsInsUn.Models
{
    public class EquiposModel
    {
        public long Id { get; set; }
        public string Código { get; set; }
        public string Nombre { get; set; }
        public string Rango { get; set; }
        public long? MedidaRango { get; set; }
        public string MaxRango { get; set; }
        public string DivisiónEscala { get; set; }
        public long MarcaId { get; set; }
        public string MarcaDesc { get; set; }
        public string Tipo { get; set; }
        public string Serie { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public long EstatusId { get; set; }
        public string EstatusDesc { get; set; }
        public string BuscarPor { get; set; }
        public long TipoInstrumentoId { get; set; }
        public string TipoInstrumentoDesc { get; set; }
        public long AreaId { get; set; }
        public string AreaDesc { get; set; }
        public string imagePath { get; set; }
        public string codNombre { get; set; }


    }

}
