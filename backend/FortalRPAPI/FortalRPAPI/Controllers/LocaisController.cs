using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FortalRPAPI.Data;
using FortalRPAPI.Models;

namespace FortalRPAPI.Controllers
{
    [ApiController]
    public class LocaisController : Controller
    {
        private readonly AppDbContext _context;

        public LocaisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetLocais")]
        public ICollection<Locais> GetLocais()
        {
            return _context.Locais.ToList();
        }


        [HttpGet("GetLocaisByName")]
        public ActionResult<List<Locais>> GetLocaisByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
                return BadRequest("O nome deve ter pelo menos 3 letras.");

            var locais = _context.Locais
                                 .Where(x => x.Name.Contains(name))
                                 .ToList();

            return Ok(locais);
        }

        [HttpGet("GetLocaisByTipo")]
        public ActionResult<List<Locais>> GetLocaisByTipo([FromQuery] int tipo)
        {
            var locais = _context.Locais.Where(x => x.TipoLocal == tipo).ToList();
            return Ok(locais);
        }


        [HttpPost("PostLocal")]
        public ActionResult<Locais> PostLocal([FromForm] Locais locais, [FromForm] string usuario, [FromForm] string senha)
        {
            var login = _context.Administradores
                .FirstOrDefault(a => a.Usuario == usuario && a.Senha == senha);

            if (login == null)
                return Unauthorized("Login falhou!");

            _context.Locais.Add(locais);
            _context.SaveChanges();

            return Ok(locais);
        }


        [HttpPut("UpdateLocal")]
        public ActionResult<Locais> UpdateLocal([FromForm] Locais locais, [FromForm] string usuario, [FromForm] string senha)
        {
            var login = _context.Administradores
                .FirstOrDefault(a => a.Usuario == usuario && a.Senha == senha);

            if (login == null)
                return Unauthorized("Login falhou!");

            var localExistente = _context.Locais.FirstOrDefault(l => l.Id == locais.Id);
            if (localExistente == null)
                return NotFound("Local não encontrado!");

            localExistente.Name = locais.Name;
            localExistente.Desc = locais.Desc;
            localExistente.Cat = locais.Cat;
            localExistente.Color = locais.Color;
            localExistente.Lat = locais.Lat;
            localExistente.Lon = locais.Lon;
            localExistente.Desc = locais.Desc;

            _context.SaveChanges();

            return Ok(localExistente);
        }

        [HttpDelete("DeleteLocal")]
        public ActionResult<string> DeleteLocal([FromForm] int id, [FromForm] string usuario, [FromForm] string senha)
        {
            var login = _context.Administradores
                .FirstOrDefault(a => a.Usuario == usuario && a.Senha == senha);

            if (login == null)
                return Unauthorized("Login falhou!");
            var LocalExistente = _context.Locais.FirstOrDefault(x => x.Id == id);
            if (LocalExistente == null) return NotFound($"O local com o ID: {id} não existe!");
            _context.Remove(LocalExistente);
            _context.SaveChanges();
            return Ok($"O local com o ID: {id} foi deletado!");
        }
       
    }
}
