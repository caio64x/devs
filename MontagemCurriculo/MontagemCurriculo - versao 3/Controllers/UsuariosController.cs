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
using MontagemCurriculo.ViewModels;

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
               
                //bloco de dados de informacoes
                InformacaoLogin informacao = new InformacaoLogin
                {
                    UsuarioID = usuario.UsuarioID,
                    EnderecoIP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Data = DateTime.Now.ToShortDateString(),
                    Horario = DateTime.Now.ToShortTimeString()
                };

                _context.Add(informacao);
                await _context.SaveChangesAsync();

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

        //força deslogar em caso de confusao 
        [HttpHead]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                if (_context.Usuarios.Any(u => u.Email == login.Email && u.Senha == login.Senha))
                {
                    int ID = _context.Usuarios.Where(u => u.Email == login.Email && u.Senha == login.Senha).Select(u => u.UsuarioID).Single();

                    InformacaoLogin informacao = new InformacaoLogin
                    {
                        UsuarioID = ID,
                        EnderecoIP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        Data = DateTime.Now.ToShortDateString(),
                        Horario = DateTime.Now.ToShortTimeString()
                    };

                    _context.Add(informacao);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetInt32("UsuarioID", ID);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, login.Email)
                    };

                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Curriculos");
                }
            }

            return View(login);
        }



        public JsonResult UsuarioExiste(string Email)
        {
            if (!_context.Usuarios.Any(e => e.Email == Email))
                return Json(true);
            return Json("Email existente");
        }
    }
}
