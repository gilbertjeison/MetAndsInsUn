using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MetAndsInsUn.Database;
using MetAndsInsUn.Models;
using System.Diagnostics;

namespace MetAndsInsUn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoInstrumentoesController : ControllerBase
    {
        private readonly MetAndInsContext _context;

        public TipoInstrumentoesController(MetAndInsContext context)
        {
            _context = context;
        }

        // GET: api/TipoInstrumentoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoInstrumentoModel>>> GetTipoInstrumento()
        {
            List<TipoInstrumentoModel> list = new List<TipoInstrumentoModel>();

            try
            {

                var query = from td in _context.TipoInstrumento
                            orderby td.Descripcion
                            select td;

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
                {
                    list.Add(new TipoInstrumentoModel()
                    {
                        Id = item.Id,
                        Codigo = item.Codigo,
                        Descripcion = item.Descripcion
                    });
                }
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar tipos instrumento all join: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        // GET: api/TipoInstrumentoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoInstrumento>> GetTipoInstrumento(long id)
        {
            var tipoInstrumento = await _context.TipoInstrumento.FindAsync(id);

            if (tipoInstrumento == null)
            {
                return NotFound();
            }

            return tipoInstrumento;
        }

        // PUT: api/TipoInstrumentoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoInstrumento(long id, TipoInstrumento tipoInstrumento)
        {
            if (id != tipoInstrumento.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoInstrumento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoInstrumentoExists(id))
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

        // POST: api/TipoInstrumentoes
        [HttpPost]
        public async Task<ActionResult<TipoInstrumento>> PostTipoInstrumento(TipoInstrumento tipoInstrumento)
        {
            _context.TipoInstrumento.Add(tipoInstrumento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoInstrumento", new { id = tipoInstrumento.Id }, tipoInstrumento);
        }

        // DELETE: api/TipoInstrumentoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoInstrumento>> DeleteTipoInstrumento(long id)
        {
            var tipoInstrumento = await _context.TipoInstrumento.FindAsync(id);
            if (tipoInstrumento == null)
            {
                return NotFound();
            }

            _context.TipoInstrumento.Remove(tipoInstrumento);
            await _context.SaveChangesAsync();

            return tipoInstrumento;
        }

        private bool TipoInstrumentoExists(long id)
        {
            return _context.TipoInstrumento.Any(e => e.Id == id);
        }
    }
}
