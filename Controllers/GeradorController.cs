using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TransporteWeb.Models;

namespace TransporteWeb.Controllers
{
    public class GeradorController : Controller
    {
        private readonly Contexto contexto;

        public GeradorController(Contexto context)
        {
            contexto = context;
        }
        
        public IActionResult Cursos()
        {
            contexto.Database.ExecuteSqlRaw("delete from cursos");
            contexto.Database.ExecuteSqlRaw("DBCC CHECKIDENT('cursos', RESEED,0)");
            Random randNum = new Random();

            string[] vCurso1 = { "ADS", "BCC", "FISIOTERAPIA", "ENFERMAGEM", "MEDICINA", "ENGENHARIA", "NUTRICAO", };
            string[] vCurso2 = { "FILOSOFIA", "RELACAO", "DIREITO", "ARQUITETURA", "PUBLICIDADE", "QUIMICA", "CONTABEIS", };

            for (int i=0; i<10; i++)
            {
                Curso curso = new Curso();

                curso.nome = (i % 2==0)? vCurso1[i/2] : vCurso2[i/2];   
                contexto.Cursos.Add(curso);
            }
            contexto.SaveChanges();

            return View(contexto.Cursos.OrderBy(o => o.id).ToList());


        }

        public IActionResult Veiculos()
        {
            contexto.Database.ExecuteSqlRaw("delete from veiculos");
            contexto.Database.ExecuteSqlRaw("DBCC CHECKIDENT('veiculos', RESEED,0)");
            Random randNum = new Random();

            string[] vOnibus1 = { "ONIBUS FEMA", "ONIBUS FEMA EXTRA", "ONIBUS UNIP", "ONIBUS UNIP EXTRA", "ONIBUS UNESP"};
            string[] vOnibus2 = { "ONIBUS UNESP EXTRA", "ONIBUS FATEC", "ONIBUS FATEC EXTRA", "ONIBUS UNOPAR", "ONIBUS UNOPAR EXTRA"};
           


            for (int i = 0; i < 10; i++)
            {
                Veiculo veiculo = new Veiculo();

                veiculo.nomeveiculo = (i % 2 == 0) ? vOnibus1[i / 2] : vOnibus2[i / 2];
                veiculo.placa = "AB2"+ (i+1).ToString();
                contexto.Veiculos.Add(veiculo);
            }
            contexto.SaveChanges();

            return View(contexto.Veiculos.OrderBy(o => o.id).ToList());


        }

    }
}
