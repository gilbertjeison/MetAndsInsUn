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

namespace MetAndsInsUn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposDataController : ControllerBase
    {
        private readonly MetAndInsContext _context;

        public TiposDataController(MetAndInsContext context)
        {
            _context = context;
        }

        // GET: api/TiposData
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiposDataModel>>> GetTiposData()
        {
            List<TiposDataModel> list = new List<TiposDataModel>();

            try
            {

                var query = from td in _context.TiposData
                            orderby td.Descripcion
                            select td;

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
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
                string err = "Excepción al momento de consultar tipos data all join: " + e.ToString();
                Trace.WriteLine(err);
            }
           
            return list;
        }

        // GET: api/TiposData
        [Produces("application/json")]
        [HttpGet("tipos")]
        public async Task<ActionResult<IEnumerable<Tipos>>> GetTipos()
        {
            List<Tipos> list = new List<Tipos>();

            try
            {
                var query = from td in _context.Tipos
                            orderby td.Descripcion
                            select td;

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
                {
                    list.Add(new Tipos()
                    {
                        Id = item.Id,
                        Descripcion = item.Descripcion
                    });
                }

                //Agregar tipo de instrumento
                if (list.Count>0)
                {
                    list.Add(new Tipos() { Id = list.Max(x => x.Id) + 1, Descripcion = "Tipos de instrumentos" });
                }                
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar tipos join: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }


        // GET: api/TiposData
        [Produces("application/json")]
        [HttpGet("bytipo/{id}")]
        public async Task<ActionResult<IEnumerable<TiposDataModel>>> GetTiposDataBytipo(long id)
        {
            List<TiposDataModel> list = new List<TiposDataModel>();

            try
            {
                var query = from td in _context.TiposData
                            where td.IdTipo == id
                            orderby td.Descripcion
                            select td;

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
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
                string err = "Excepción al momento de consultar tipos data join: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        // GET: api/TiposData
        [Produces("application/json")]
        [HttpGet("byidti/{codigo}")]
        public async Task<ActionResult> VerifyTICode(string codigo)
        {
            int code;
            string message;
            TipoInstrumentoModel list = new TipoInstrumentoModel();

            try
            {
                var query = await (from ti in _context.TipoInstrumento
                            where ti.Codigo == codigo                            
                            select ti).FirstOrDefaultAsync();

                if (query != null)
                {
                    list = new TipoInstrumentoModel()
                    {
                        Id = query.Id,
                        Descripcion = query.Descripcion,
                        Codigo = query.Codigo
                    };

                    code = 1;
                    message = "OK";
                }
                else
                {
                    code = 0;
                    message = "OBJETO NO ENCONTRADO";
                }                            
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar tipo de instrumento por código: " + e.ToString();
                Trace.WriteLine(err);
                code = -1;
                message = e.Message;
            }

            return Ok(new {code,message,list});
        }


        [HttpPost("add")]
        public async Task<ActionResult> PostTiposData(IFormCollection data)
        {
            int code;
            string message;

            try
            {       
                //Guardar objeto en la base de datos            
                TiposData td = new TiposData()
                {
                    IdTipo = int.Parse(data["idTipo"]),
                    Descripcion = data["descripcion"]                    
                };

                _context.TiposData.Add(td);
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


        [HttpPost("add_ti")]
        public async Task<ActionResult> PostTipoInstrumento(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                //Guardar objeto en la base de datos            
                TipoInstrumento ti = new TipoInstrumento()
                {
                    Codigo = data["codigo"],
                    Descripcion = data["descripcion"]
                };

                _context.TipoInstrumento.Add(ti);
                await _context.SaveChangesAsync();

                code = 1;
                message = "OK";

                return Ok(new { code, message });
            }
            catch (Exception ex)
            {
                code = -1;
                message = "ERROR " + ex.Message;
                return Ok(new { code, message });
            }
        }

        // POST: api/TiposData
        [HttpPost("edit")]
        public async Task<ActionResult> EditTiposData(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                //Capturar ID del equipo a editar
                long id_td = long.Parse(data["id"]);

                //Buscar el equipo en la base de datos
                TiposData tdEdit = _context.TiposData.Where(i => i.Id == id_td).FirstOrDefault();

                if (tdEdit != null)
                {
                    //mapear nuevos datos            
                    tdEdit.Id = id_td;
                    tdEdit.Descripcion = data["descripcion"];
                    tdEdit.IdTipo = int.Parse(data["idTipo"]);                    

                    _context.Entry(tdEdit).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    code = 1;
                    message = "OK";
                }
                else
                {
                    code = 0;
                    message = "Tipo data a editar inválido";
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

        // POST: api/TiposData
        [HttpPost("edit_ti")]
        public async Task<ActionResult> EditTipoInstrumento(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                //Capturar ID del equipo a editar
                long id_td = long.Parse(data["id"]);

                //Buscar el equipo en la base de datos
                TipoInstrumento tdEdit = _context.TipoInstrumento.Where(i => i.Id == id_td).FirstOrDefault();

                if (tdEdit != null)
                {
                    //mapear nuevos datos            
                    tdEdit.Id = id_td;
                    tdEdit.Descripcion = data["descripcion"];
                    tdEdit.Codigo = data["codigo"];

                    _context.Entry(tdEdit).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    code = 1;
                    message = "OK";
                }
                else
                {
                    code = 0;
                    message = "Tipo instrumento a editar inválido";
                }

                return Ok(new { code, message });
            }
            catch (Exception ex)
            {
                code = -1;
                message = "ERROR " + ex.ToString();
                return Ok(new { code, message });
            }
        }

        // GET: api/TiposData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TiposData>> GetTiposData(long id)
        {
            var tiposData = await _context.TiposData.FindAsync(id);

            if (tiposData == null)
            {
                return NotFound();
            }

            return tiposData;
        }

        // PUT: api/TiposData/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTiposData(long id, TiposData tiposData)
        {
            if (id != tiposData.Id)
            {
                return BadRequest();
            }

            _context.Entry(tiposData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiposDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/TiposData/5
        [HttpPost("delete")]
        public async Task<ActionResult<TiposData>> DeleteTiposData(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                long id_td = long.Parse(data["id"]);
                //Capturar el objeto a eliminar
                var tiposData = await _context.TiposData.FindAsync(id_td);

                if (tiposData != null)
                {
                    _context.TiposData.Remove(tiposData);
                    await _context.SaveChangesAsync();

                    code = 1;
                    message = "OK";
                }
                else
                {
                    code = 0;
                    message = "Tipo data a eliminar inválido";
                }

                return Ok(new { code, message });
            }
            catch (Exception ex)
            {
                code = -1;
                message = "ERROR " + ex.ToString();
                return Ok(new { code, message });
            }
        }

        // DELETE
        [HttpPost("delete_ti")]
        public async Task<ActionResult> DeleteTipoInstrumento(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                long id_td = long.Parse(data["id"]);
                //Capturar el objeto a eliminar
                var ti = await _context.TipoInstrumento.FindAsync(id_td);

                if (ti != null)
                {
                    _context.TipoInstrumento.Remove(ti);
                    await _context.SaveChangesAsync();

                    code = 1;
                    message = "OK";
                }
                else
                {
                    code = 0;
                    message = "Tipo instrumento a eliminar inválido";
                }

                return Ok(new { code, message });
            }
            catch (Exception ex)
            {
                code = -1;
                message = "ERROR " + ex.ToString();
                return Ok(new { code, message });
            }
        }

        private bool TiposDataExists(long id)
        {
            return _context.TiposData.Any(e => e.Id == id);
        }
    }
}
