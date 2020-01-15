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
    public class UsuariosController : ControllerBase
    {
        private readonly MetAndInsContext _context;

        public UsuariosController(MetAndInsContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuariosModel>>> GetUsuarios()
        {
            List<UsuariosModel> list = new List<UsuariosModel>();

            try
            {
                var query = from td in _context.Usuarios
                            orderby td.Nombres
                            select td;

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
                {
                    list.Add(new UsuariosModel()
                    {
                        id = item.Id,
                        nombreCompleto = item.Nombres +" "+item.Apellidos,
                        apellidos = item.Apellidos,
                        nombres = item.Nombres,
                        usuario = item.Usuario,
                        password = item.Password
                    });
                }
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar usuarios: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuarios(long id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);

            if (usuarios == null)
            {
                return NotFound();
            }

            return usuarios;
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarios(long id, Usuarios usuarios)
        {
            if (id != usuarios.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
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

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUsuarios(Usuarios usuarios)
        {
            _context.Usuarios.Add(usuarios);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarios", new { id = usuarios.Id }, usuarios);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuarios>> DeleteUsuarios(long id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuarios);
            await _context.SaveChangesAsync();

            return usuarios;
        }

        private bool UsuariosExists(long id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
