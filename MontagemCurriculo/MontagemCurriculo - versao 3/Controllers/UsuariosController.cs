using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MontagemCurriculo.Models;

namespace MontagemCurriculo.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;

        public UsuariosController(Contexto context)
        {
            _context = context;
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro([Bind("UsuarioID,Email,Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                //adiciona e salva no contexto do banco de dados
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                //bloco de dados de informacoes
                InformacaoLogin informacao = new InformacaoLogin
                {
                    UsuarioID = usuario.UsuarioID,
                    EnderecoIP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Data = DateTime.Now.ToShortDateString(),
                    Horario = DateTime.Now.ToShortTimeString()
                };

                HttpContext.Session.SetInt32("UsuarioID", usuario.UsuarioID);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, usuario.Email)
                };

                //joga o ID do usuario em uma sessao para futuro uso
                var userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                //retorna o index de curriculos
                return RedirectToAction("Index", "Curriculos");
            }
            return View(usuario);
        }

        public JsonResult UsuarioExiste(string Email)
        {
            if (!_context.Usuarios.Any(e => e.Email == Email))
                return Json(true);
            return Json("Email existente");
        }
    }
}
