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
using System.IO;
using MetAndsInsUn.Utils;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using MetAndsInsUn.Filters;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;

namespace MetAndsInsUn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly MetAndInsContext _context;

        private IHostingEnvironment _env;

        public EquiposController(MetAndInsContext context, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Equipos
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquiposModel>>> GetEquiposRelJoin()
        {
            List<EquiposModel> list = new List<EquiposModel>();

            try
            {                
                var query = from e in _context.Equipos
                            join ti in _context.TipoInstrumento
                            on e.TipoInstrumento equals ti.Id
                            join m in _context.TiposData
                            on e.Marca equals m.Id
                            join a in _context.TiposData
                            on e.Area equals a.Id
                            join es in _context.TiposData
                            on e.Estatus equals es.Id
                            select new { e, m, es, ti, a };

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
                {
                    list.Add(new EquiposModel()
                    {
                        Id = item.e.Id,
                        AreaDesc = item.a.Descripcion,
                        AreaId = item.a.Id,
                        BuscarPor = item.e.BuscarPor,
                        Código = item.e.Código,
                        DivisiónEscala = item.e.DivisiónEscala,
                        EstatusDesc = item.es.Descripcion,
                        EstatusId = item.es.Id,
                        FechaIngreso = item.e.FechaIngreso,
                        MarcaDesc = item.m.Descripcion,
                        MarcaId = item.m.Id,
                        MaxRango = item.e.MaxRango,
                        MedidaRango = item.e.MedidaRango,
                        Nombre = item.e.Nombre,
                        Rango = item.e.Rango,
                        Serie = item.e.Serie,
                        Tipo = item.e.Tipo,
                        TipoInstrumentoDesc = item.ti.Descripcion,
                        TipoInstrumentoId = item.ti.Id,
                        imagePath = item.e.ImagePath

                    });
                }                
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar equipos join: " + e.ToString();
                Trace.WriteLine(err);                
            }

            return list;
        }



        [Produces("application/json")]
        [HttpGet("equipos_combo/{area}")]
        public async Task<ActionResult<IEnumerable<EquiposModel>>> GetEquiposCombo(long area)
        {
            List<EquiposModel> list = new List<EquiposModel>();

            try
            {
                var query = from e in _context.Equipos
                            where e.Area == area
                            select e;

                var data = await query.ToListAsync();

                foreach (var item in data.ToList())
                {
                    list.Add(new EquiposModel()
                    {
                        Id = item.Id,                     
                        AreaId = item.Area.Value,                       
                        Código = item.Código,                       
                        MarcaId = item.Marca.Value,                        
                        Nombre = item.Nombre,    
                        TipoInstrumentoId = item.TipoInstrumento.Value,
                        codNombre = item.Código + " - " + item.Nombre
                    });
                }
            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar equipos combo: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        // GET: api/Equipos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipos>> GetEquipos(long id)
        {
            var equipos = await _context.Equipos.FindAsync(id);

            if (equipos == null)
            {
                return NotFound();
            }

            return equipos;
        }

        // PUT: api/Equipos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipos(long id, Equipos equipos)
        {
            if (id != equipos.Id)
            {
                return BadRequest();
            }

            _context.Entry(equipos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquiposExists(id))
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

        // POST: api/Equipos
        [HttpPost("add")]        
        public async Task<ActionResult> PostEquipos(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                //Obtener la imagen de los datos recibidos
                var formFile = data.Files.FirstOrDefault();
                                               
                //Verificar que la imagen sea valida
                if (formFile != null)
                {
                    //Gestionar imagen
                    //Ruta de la imagen
                    var webRoot = _env.WebRootPath;
                    var PathWithFolderName = Path.Combine(webRoot, "Imagenes");

                    //Crear carpeta si no existe
                    if (!Directory.Exists(PathWithFolderName))
                    {
                        // Try to create the directory.
                        Directory.CreateDirectory(PathWithFolderName);
                    }

                    //Combinar la ubicación destino y el nombre de la imagen
                    string filePath = PathWithFolderName + "/" + formFile.FileName;

                    //Guardar imagen en el directorio dado
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }

                //Guardar objeto en la base de datos            
                Equipos e = new Equipos()
                {
                    Area = long.Parse(data["AreaId"]),
                    BuscarPor = "84-00",
                    CheckEliminados = "",
                    Código = data["código"],
                    DivisiónEscala = data["DivisiónEscala"],
                    Estatus = long.Parse(data["estatusId"]),
                    FechaIngreso = DateTime.Parse(data["fechaAdd"]),
                    ImagePath = formFile == null ? "" : formFile.FileName,
                    Marca = long.Parse(data["MarcaId"]),
                    MaxRango = data["MaxRango"],
                    Nombre = data["nombre"],
                    Rango = data["Rango"],
                    Serie = data["serie"],
                    Tipo = data["Tipo"],
                    TipoInstrumento = long.Parse(data["tipoInstrumentoId"]),
                    Frecuencia = 12
                };

                _context.Equipos.Add(e);
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

        // POST: api/Equipos
        [HttpPost("edit")]
        public async Task<ActionResult> EditEquipos(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                //Capturar ID del equipo a editar
                long id_equipo = long.Parse(data["Id"]);

                //Buscar el equipo en la base de datos
                Equipos eqEdit;    
                eqEdit = _context.Equipos.Where(i => i.Id == id_equipo).FirstOrDefault();                

                if (eqEdit != null)
                {
                    //Obtener la imagen de los datos recibidos
                    var formFile = data.Files.FirstOrDefault();

                    //Verificar que la imagen sea valida
                    if (formFile != null)
                    {
                        //Gestionar imagen
                        //Ruta de la imagen
                        var webRoot = _env.WebRootPath;
                        var PathWithFolderName = Path.Combine(webRoot, "Imagenes");

                        //Crear carpeta si no existe
                        if (!Directory.Exists(PathWithFolderName))
                        {
                            // Try to create the directory.
                            Directory.CreateDirectory(PathWithFolderName);
                        }                       

                        //Guardar imagen en el directorio dado, si ya existe, se remplaza
                        //Tener en cuenta que si la imagen cambia, se elimina la anterior
                        //Ver si las imagenes son diferentes
                        if (!formFile.FileName.Trim().Equals(eqEdit.ImagePath.Trim()))
                        {
                            //Eliminar imagen anterior
                            System.IO.File.Delete(PathWithFolderName + "/" + eqEdit.ImagePath);
                        }

                        //Combinar la ubicación destino y el nombre de la imagen
                        string filePath = PathWithFolderName + "/" + formFile.FileName;

                        //Guardar la nueva imagen
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        eqEdit.ImagePath = formFile.FileName;
                    }

                    //mapear nuevos datos            
                    eqEdit.Id = id_equipo;
                    eqEdit.Area = long.Parse(data["areaId"]);
                    eqEdit.Código = data["código"];
                    eqEdit.DivisiónEscala = data["DivisiónEscala"];
                    eqEdit.Estatus = long.Parse(data["estatusId"]);
                    eqEdit.FechaIngreso = DateTime.Parse(data["fechaAdd"]);                    
                    eqEdit.Marca = long.Parse(data["MarcaId"]);
                    eqEdit.MaxRango = data["MaxRango"];
                    eqEdit.Nombre = data["nombre"];
                    eqEdit.Rango = data["Rango"];
                    eqEdit.Serie = data["serie"];
                    eqEdit.Tipo = data["Tipo"];
                    eqEdit.TipoInstrumento = long.Parse(data["tipoInstrumentoId"]);

                    _context.Entry(eqEdit).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    code = 1;
                    message = "OK";
                }
                else
                {
                    code = 0;
                    message = "Equipo a editar inválido";
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

        // GET api/<controller>/image
        [HttpGet("image")]
        public IActionResult Get(string image)
        {
            var path = Path.Combine(_env.WebRootPath, "Imagenes", $"{image.Trim()}");
            var imageFileStream = System.IO.File.OpenRead(path);
            return File(imageFileStream, "image/jpeg");
        }

        [HttpGet("file")]
        public IActionResult GetFile(string file)
        {
            var path = Path.Combine(_env.WebRootPath, "Calibraciones", $"{file.Trim()}");
            var imageFileStream = System.IO.File.OpenRead(path);
            return File(imageFileStream, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        //[HttpPost]
        //public async Task<ActionResult<Equipos>> PostEquipos(Equipos equipos)
        //{
        //    _context.Equipos.Add(equipos);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEquipos", new { id = equipos.Id }, equipos);
        //}

        // DELETE: api/Equipos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Equipos>> DeleteEquipos(long id)
        {
            var equipos = await _context.Equipos.FindAsync(id);
            if (equipos == null)
            {
                return NotFound();
            }

            _context.Equipos.Remove(equipos);
            await _context.SaveChangesAsync();

            return equipos;
        }

        private bool EquiposExists(long id)
        {
            return _context.Equipos.Any(e => e.Id == id);
        }

        // GET: api/Equipos
        [Produces("application/json")]
        [HttpGet("cals/{idEquipo}")]
        public async Task<ActionResult<IEnumerable<CalibracionesModel>>> GetCalibraciones(long idEquipo)
        {
            List<CalibracionesModel> list = new List<CalibracionesModel>();

            try
            {
                var query = from c in _context.Calibraciones
                            join u in _context.Usuarios
                            on c.IdUsuario equals u.Id
                            where c.IdEquipo == idEquipo
                            select new CalibracionesModel()
                            {
                                id = c.Id,
                                fecha = c.Fecha.Value.ToShortDateString(),
                                file_path = c.CalibrationFilePath,
                                idEquipo = c.IdEquipo.Value,
                                idUsuario = c.IdUsuario.Value,
                                nombresUsuario = u.Nombres +" "+ u.Apellidos,
                                observaciones = c.Observaciones,
                                tipo = c.Tipo
                            };

                list = await query.ToListAsync();


            }
            catch (Exception e)
            {
                string err = "Excepción al momento de consultar calibraciones: " + e.ToString();
                Trace.WriteLine(err);
            }

            return list;
        }

        // POST: api/Equipos
        [HttpPost("addCal")]
        public async Task<ActionResult> PostCalibraciones(IFormCollection data)
        {
            int code;
            string message;

            try
            {
                //Obtener la imagen de los datos recibidos
                var formFile = data.Files.FirstOrDefault();

                //Verificar que la imagen sea valida
                if (formFile != null)
                {
                    //Gestionar imagen
                    //Ruta de la imagen
                    var webRoot = _env.WebRootPath;
                    var PathWithFolderName = Path.Combine(webRoot, "Calibraciones");

                    //Crear carpeta si no existe
                    if (!Directory.Exists(PathWithFolderName))
                    {
                        // Try to create the directory.
                        Directory.CreateDirectory(PathWithFolderName);
                    }

                    //Combinar la ubicación destino y el nombre de la imagen
                    string filePath = PathWithFolderName + "/" + Path.GetFileNameWithoutExtension(formFile.FileName) + "_" + SomeHelpers.GetDateString() + Path.GetExtension(formFile.FileName);

                    //Guardar imagen en el directorio dado
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }

                //Guardar objeto en la base de datos            
                Calibraciones c = new Calibraciones()
                {
                    IdEquipo = long.Parse(data["idEquipo"]),
                    IdUsuario = long.Parse(data["idUsuario"]),
                    Fecha = DateTime.Now,
                    CalibrationFilePath = Path.GetFileNameWithoutExtension(formFile.FileName) + "_"+ SomeHelpers.GetDateString() + Path.GetExtension(formFile.FileName),
                    Observaciones = data["observaciones"],
                    Tipo = data["tipo"]
                };

                _context.Calibraciones.Add(c);
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

        // DELETE: api/Equipos/1
        [HttpPost("calsdel/{id}")]
        public async Task<ActionResult<TiposData>> DeleteCalibracion(long id)
        {
            int code;
            string message;

            try
            {
                
                //Capturar el objeto a eliminar
                var cal = await _context.Calibraciones.FindAsync(id);

                if (cal != null)
                {
                    //Ruta del archivo
                    var webRoot = _env.WebRootPath;
                    var PathWithFolderName = Path.Combine(webRoot, "Calibraciones");

                    //Eliminar archivo de calibración
                    System.IO.File.Delete(PathWithFolderName + "/" + cal.CalibrationFilePath);
                    
                    _context.Calibraciones.Remove(cal);
                    await _context.SaveChangesAsync();

                    code = 1;
                    message = "OK";
                }
                else
                {
                    code = 0;
                    message = "Calibración a eliminar inválida";
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
    }
}
