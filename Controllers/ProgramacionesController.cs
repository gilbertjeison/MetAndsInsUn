using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MetAndsInsUn.Database;
using System.Diagnostics;
using MetAndsInsUn.Models;
using MetAndsInsUn.Utils;

namespace MetAndsInsUn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramacionesController : ControllerBase
    {
        private readonly MetAndInsContext _context;

        public ProgramacionesController(MetAndInsContext context)
        {
            _context = context;
        }

        // GET: api/Equipos
        [Produces("application/json")]
        [HttpGet("distinct_years")]
        public async Task<ActionResult<IEnumerable<int?>>> GetDistinctYears()
        {
            List<int?> list = new List<int?>();

            try
            {
                var query = (from e in _context.Programaciones                           
                            select e.Ano).Distinct();

                list = await query.ToListAsync();

               
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar los distintos años: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        [Produces("application/json")]
        [HttpGet("calculate_weeks/{year}")]
        public async Task<IEnumerable<WeeksCal>> GetCalculateWeeks(int year)
        {
            return await Task.Run(() =>
            {
                return SomeHelpers.GenerateWeeks(year);
            });
        }

        [Produces("application/json")]
        [HttpGet("distinct_areas")]
        public async Task<ActionResult<IEnumerable<TiposDataModel>>> GetDistinctAreas()
        {
            List<TiposDataModel> list = new List<TiposDataModel>();

            try
            {
                var query = (from p in _context.Programaciones
                             join e in _context.Equipos
                             on p.IdEquipo equals e.Id
                             join td in _context.TiposData
                             on e.Area equals td.Id
                             orderby td.Descripcion
                             select td).Distinct();

                var data = await query.ToListAsync();

                foreach (var item in data)
                {
                    list.Add(new TiposDataModel()
                    {
                        Id = item.Id,
                        Descripcion = item.Descripcion,
                        IdTipo = item.IdTipo
                    });
                }
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar los distintos años: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        // GET: api/Programaciones
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgramacionesModel>>> GetProgramaciones()
        {
            List<ProgramacionesModel> list = new List<ProgramacionesModel>();

            try
            {
                var query = from p in _context.Programaciones
                            join e in _context.Equipos
                            on p.IdEquipo equals e.Id
                            join td in _context.TiposData
                            on p.IdEstado equals td.Id
                            join u in _context.Usuarios
                            on p.IdUsuario equals u.Id
                            
                            select new { e, p, td, u };

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
                {
                    list.Add(new ProgramacionesModel()
                    {
                        id = item.p.Id,
                        idEquipo = item.e.Id,
                        descEquipo = item.e.Nombre,
                        ano = item.p.Ano.Value,
                        semana = item.p.Semana.Value,
                        fecha = item.p.Fecha.Value,
                        idEstado = item.td.Id,
                        descEstado = item.td.Descripcion,
                        idUsuario = item.u.Id,
                        nombreUsuario = item.u.Nombres + " " + item.u.Apellidos,
                        observaciones = item.p.Observaciones,
                        frecuencia = item.e.Frecuencia.Value,
                        codigoEquipo = item.e.Código,
                        idArea = item.e.Area.Value,
                        mes = SomeHelpers.FirstDateOfWeekISO8601(item.p.Ano.Value,item.p.Semana.Value).Month
                    });
                }
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar programaciones join: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        // GET: api/Programaciones
        [Produces("application/json")]
        [HttpGet("{area}/{year}")]
        public async Task<ActionResult<IEnumerable<ProgramacionesModel>>> GetProgramaciones(long area, int year)
        {
            List<ProgramacionesModel> list = new List<ProgramacionesModel>();

            try
            {
                var query = from p in _context.Programaciones
                            join e in _context.Equipos
                            on p.IdEquipo equals e.Id
                            join td in _context.TiposData
                            on p.IdEstado equals td.Id
                            join u in _context.Usuarios
                            on p.IdUsuario equals u.Id
                            where e.Area == area
                            && p.Ano == year
                            select new { e, p, td, u };

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
                {
                    list.Add(new ProgramacionesModel()
                    {
                        id = item.p.Id,
                        idEquipo = item.e.Id,
                        descEquipo = item.e.Nombre,
                        ano = item.p.Ano.Value,
                        semana = item.p.Semana.Value,
                        fecha = item.p.Fecha.Value,
                        idEstado = item.td.Id,
                        descEstado = item.td.Descripcion,
                        idUsuario = item.u.Id,
                        nombreUsuario = item.u.Nombres + " " + item.u.Apellidos,
                        observaciones = item.p.Observaciones,
                        frecuencia = item.e.Frecuencia.Value,
                        codigoEquipo = item.e.Código,
                        idArea = item.e.Area.Value,
                        mes = SomeHelpers.FirstDateOfWeekISO8601(item.p.Ano.Value, item.p.Semana.Value).Month
                    });
                }
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar programaciones by area year: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        // GET: api/Programaciones
        [Produces("application/json")]
        [HttpGet("{idEquipo}/{year}/{week}")]
        public async Task<JsonResult> ExistsInDate(long idEquipo, int year, int week)
        {
            bool exists = false;

            try
            {
                var query = from p in _context.Programaciones                           
                            where p.IdEquipo == idEquipo
                            && p.Semana == week
                            && p.Ano == year
                            select p;

                var data = await query.ToListAsync();

                if (data.Count > 0)
                {
                    exists = true;
                }
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar programaciones by area year: " + e.ToString();
                Trace.WriteLine(err);
            }

            return new JsonResult(new { exists });
        }

        [HttpPost("add")]
        public async Task<ActionResult> PostProgramacion(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                //Guardar objeto en la base de datos            
                Programaciones p = new Programaciones()
                {
                    Ano = int.Parse(data["ano"]),
                    Fecha = DateTime.Now,
                    IdEquipo = long.Parse(data["idEquipo"]),
                    IdEstado = long.Parse(data["idEstado"]),
                    IdUsuario = long.Parse(data["idUsuario"]),
                    Observaciones = data["observaciones"],
                    Semana = int.Parse(data["semana"]),                    
                };

                _context.Programaciones.Add(p);
                await _context.SaveChangesAsync();

                code = 1;
                message = "OK";

                return Ok(new { code, message });
            }
            catch (Exception ex)
            {
                code = 1;
                message = "ERROR " + ex.Message;
                return Ok(new { code, message });
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult> EditProgramacion(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                //Capturar ID del equipo a editar
                long id = long.Parse(data["id"]);

                //Buscar el objetivo
                Programaciones p = _context.Programaciones.Where(i => i.Id == id).FirstOrDefault();

                if (p != null)
                {
                    //mapear nuevos datos            
                    p.Id = id;
                    p.Ano = int.Parse(data["ano"]);
                    p.Fecha = DateTime.Now;
                    p.IdEquipo = long.Parse(data["idEquipo"]);
                    p.IdEstado = long.Parse(data["idEstado"]);
                    p.IdUsuario = long.Parse(data["idUsuario"]);
                    p.Observaciones = data["observaciones"];
                    p.Semana = int.Parse(data["semana"]);       

                    _context.Entry(p).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    code = 1;
                    message = "OK";
                }
                else
                {
                    code = 0;
                    message = "Programación a editar inválida";
                }

                return Ok(new { code, message });
            }
            catch (Exception ex)
            {
                code = 1;
                message = "ERROR " + ex.ToString();
                return Ok(new { code, message });
            }
        }
    }
}
