using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MetAndsInsUn.Database;

namespace MetAndsInsUn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalibracionesController : ControllerBase
    {
        private readonly MetAndInsContext _context;

        public CalibracionesController(MetAndInsContext context)
        {
            _context = context;
        }

        // GET: api/Calibraciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calibraciones>>> GetCalibraciones()
        {
            return await _context.Calibraciones.ToListAsync();
        }

        // GET: api/Calibraciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Calibraciones>> GetCalibraciones(long id)
        {
            var calibraciones = await _context.Calibraciones.FindAsync(id);

            if (calibraciones == null)
            {
                return NotFound();
            }

            return calibraciones;
        }

        // PUT: api/Calibraciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalibraciones(long id, Calibraciones calibraciones)
        {
            if (id != calibraciones.Id)
            {
                return BadRequest();
            }

            _context.Entry(calibraciones).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalibracionesExists(id))
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

        // POST: api/Calibraciones
        [HttpPost]
        public async Task<ActionResult<Calibraciones>> PostCalibraciones(Calibraciones calibraciones)
        {
            _context.Calibraciones.Add(calibraciones);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCalibraciones", new { id = calibraciones.Id }, calibraciones);
        }

        // DELETE: api/Calibraciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Calibraciones>> DeleteCalibraciones(long id)
        {
            var calibraciones = await _context.Calibraciones.FindAsync(id);
            if (calibraciones == null)
            {
                return NotFound();
            }

            _context.Calibraciones.Remove(calibraciones);
            await _context.SaveChangesAsync();

            return calibraciones;
        }

        private bool CalibracionesExists(long id)
        {
            return _context.Calibraciones.Any(e => e.Id == id);
        }
    }
}
