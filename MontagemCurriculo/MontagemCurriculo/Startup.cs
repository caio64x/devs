using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MontagemCurriculo.Models;
using Rotativa.AspNetCore;

namespace MontagemCurriculo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //� nesse cara aqui que mexe
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //configur�cao de servicos e comunicacoes com banco de dados
            services.AddControllersWithViews();
            //servi�o de contexto do banco de dados
            services.AddDbContext<Contexto>(opcoes => opcoes.UseSqlServer(Configuration.GetConnectionString("Conexao")));
            //sessao dura 1 dia
            services.AddSession(opcoes =>
            {
                opcoes.IdleTimeout = TimeSpan.FromDays(1);
            });
            //autentica login
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opcoes =>
                {
                    opcoes.LoginPath = "/Usuarios/Login";
                });
            //injecao de dependencia para login
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        //� nesse cara que mexe para gerar o banco de dados
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ///////comunicacao com banco e cria��o do banco e tabelas
            //using (var escopo = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
            //{
            //    var contexto = escopo.ServiceProvider.GetRequiredService<Contexto>();
            //    //cria sem migrations
            //    contexto.Database.EnsureCreated();
            //}
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthorization();

#pragma warning disable CS0618 // O tipo ou membro � obsoleto
            RotativaConfiguration.Setup(env: (Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);
#pragma warning restore CS0618 // O tipo ou membro � obsoleto

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Usuarios}/{action=Registro}/{id?}");
            });


        }
    }
}
