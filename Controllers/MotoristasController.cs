using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransporteWeb.Models;

namespace TransporteWeb.Controllers
{
    [Authorize(Roles = "Motorista")]
    public class MotoristasController : Controller
    {
        private readonly Contexto _context;

        public MotoristasController(Contexto context)
        {
            _context = context;
        }

        // GET: Motorista/Agendamentos
        public async Task<IActionResult> Index()
        {
            // Recupera todos os agendamentos com as entidades relacionadas
            var agendamentos = await _context.Agendamentos
                .Include(a => a.estudante)
                .Include(a => a.veiculo)
                .Include(a => a.ponto)
                .ToListAsync();

            // Busca todas as confirmações de presença com as entidades relacionadas
            var confirmacoes = await _context.ConfirmacaoPresencas
                .Include(c => c.Agendamento)
                .ThenInclude(a => a.estudante) // Inclui estudante para acessar o nome
                .ToListAsync();

            // Cria o ViewModel com os agendamentos e confirmações
            var viewModel = new MotoristaAgendamentoViewModel
            {
                Agendamentos = agendamentos,
                Confirmacoes = confirmacoes
            };

            return View(viewModel);
        }

        // POST: Motorista/ConfirmarPresenca
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarPresenca(int idAgendamento, bool presenca)
        {
            // Verifica se já existe uma confirmação para o agendamento
            var existente = await _context.ConfirmacaoPresencas
                .FirstOrDefaultAsync(c => c.IdAgendamento == idAgendamento);

            if (existente != null)
            {
                // Se já existe uma confirmação, não permite alteração
                TempData["Mensagem"] = "A presença já foi confirmada e não pode ser alterada.";
                return RedirectToAction(nameof(Index));
            }

            // Cria uma nova confirmação de presença
            var confirmacao = new ConfirmacaoPresenca
            {
                IdAgendamento = idAgendamento,
                PresencaConfirmada = presenca
            };
            _context.ConfirmacaoPresencas.Add(confirmacao);

            // Salva as alterações no contexto
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
