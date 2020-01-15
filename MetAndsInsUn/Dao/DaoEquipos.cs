//using MetAndsInsUn.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetAndsInsUn.Dao
{
    public class DaoEquipos
    {
        //public async Task<List<Equipos>> GetEquiposAsync()
        //{
        //    List<Equipos> list = new List<Equipos>();

        //    try
        //    {
        //        await Task.Run(async () =>
        //        {
        //            using (var context = new MetAndInsContext())
        //            {
        //                var query = from e in context.Equipos
        //                            select e;

        //                var data = await query.ToListAsync();

        //                //foreach (var item in data.ToList())
        //                //{
        //                //    list.Add(new KpiViewModel()
        //                //    {
        //                //        Id = item.e.Id,
        //                //        AreaLinea = item.l.nombre,
        //                //        Equipo = item.m.nombre,
        //                //        DiligenciadoPor = item.t.Nombres + " " + item.t.Apellidos,
        //                //        TipoAveria = item.ta.descripcion,
        //                //        IdTipoAveria = (int)item.e.id_tipo_averia,
        //                //        TiempoTotal = (int)item.e.tiempo_total,
        //                //        Fecha = (DateTime)item.e.fecha_ewo,
        //                //        CicloRaiz = (int)item.e.causa_raiz_index,
        //                //        DescCicloRaiz = DesCausaRaiz(item.e.causa_raiz_index)
        //                //    });
        //                //}
        //            }
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.WriteLine("Excepción al consultar Equipos: " + e.ToString());
        //    }

        //    return list;
        //}
    }
}
