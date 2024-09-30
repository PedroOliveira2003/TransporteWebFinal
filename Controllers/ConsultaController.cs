using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransporteWeb.Models;

namespace TransporteWeb.Controllers
{
    [Authorize(Roles = "Funcionario")]
    public class ConsultaController : Controller
    {
        private readonly Contexto contexto;

        public ConsultaController(Contexto context)
        {
            contexto = context;
        }

        public IActionResult Filtrar()
        {
            return View();
        }


        public  IActionResult agenEst(string busca)
        {
            var atEst = contexto.Agendamentos.Include(est=>est.estudante).Include(veic=>veic.veiculo).Where(v=>v.veiculo.nomeveiculo ==busca ).ToList();
            return View(atEst);
        }

    }
}
