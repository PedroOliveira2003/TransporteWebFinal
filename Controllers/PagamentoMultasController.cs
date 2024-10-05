using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransporteWeb.Models;

namespace TransporteWeb.Controllers
{
    [Authorize]
    public class PagamentoMultasController : Controller
    {
        private readonly Contexto _context;

        public PagamentoMultasController(Contexto context)
        {
            _context = context;
        }

        // GET: PagamentoMultas/Lista
        public async Task<IActionResult> Lista()
        {
            var pagamentos = await _context.PagamentosMulta
                .Include(p => p.Estudante)
                .ToListAsync();

            return View(pagamentos);
        }

        // GET: PagamentoMultas/EstudantesComMulta
        public async Task<IActionResult> EstudantesComMulta()
        {
            var estudantesComMulta = await _context.Estudantes
                .Where(e => e.Multa > 0) // Filtra os estudantes com multa
                .ToListAsync();

            return View(estudantesComMulta); // Retorna a lista de estudantes com multa para a view
        }

        // GET: PagamentoMultas/Pagar
        public async Task<IActionResult> Pagar(int idEstudante)
        {
            var estudante = await _context.Estudantes.FindAsync(idEstudante);
            if (estudante == null || estudante.Multa <= 0)
            {
                return NotFound();
            }

            var pagamentoViewModel = new PagamentoMulta
            {
                IdEstudante = idEstudante,
                Valor = estudante.Multa
            };

            return View(pagamentoViewModel);
        }

        // POST: PagamentoMultas/Pagar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pagar(PagamentoMulta pagamentoMulta)
        {
            if (ModelState.IsValid)
            {
                // Atualiza a data do pagamento
                pagamentoMulta.DataPagamento = DateTime.Now;

                // Adiciona o pagamento ao contexto
                _context.PagamentosMulta.Add(pagamentoMulta);

                // Atualiza a multa do estudante para 0 após o pagamento
                var estudante = await _context.Estudantes.FindAsync(pagamentoMulta.IdEstudante);
                if (estudante != null)
                {
                    estudante.Multa = 0; // Define a multa como zero após o pagamento
                    _context.Estudantes.Update(estudante);
                }

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();

                TempData["Mensagem"] = "Pagamento realizado com sucesso!";
                return RedirectToAction(nameof(Lista)); // Redireciona para a lista de pagamentos
            }

            // Se o ModelState não for válido, retorna a view com o modelo preenchido
            return View(pagamentoMulta);
        }
    }
}
