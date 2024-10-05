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

            // Se a presença for cancelada (presenca == false), atribui multa ao estudante
            if (!presenca)
            {
                // Recupera o agendamento correspondente
                var agendamento = await _context.Agendamentos
                    .Include(a => a.estudante) // Inclui o estudante para acessar os dados dele
                    .FirstOrDefaultAsync(a => a.id == idAgendamento);

                if (agendamento != null)
                {
                    // Atribui a multa ao estudante
                    var estudante = agendamento.estudante;
                    // Defina o valor da multa conforme sua lógica (ex: 50,00)
                    decimal valorMulta = 5m; // Você pode ajustar esse valor conforme necessário

                    // Adiciona o valor da multa
                    estudante.Multa += valorMulta; // Atualiza o valor da multa do estudante
                    _context.Estudantes.Update(estudante); // Marca o estudante para atualização
                }
            }

            // Adiciona a nova confirmação ao contexto
            _context.ConfirmacaoPresencas.Add(confirmacao);

            // Salva as alterações no contexto
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Motorista/EstudantesComMulta
        public async Task<IActionResult> EstudantesComMulta()
        {
            // Busca todos os estudantes que possuem multa
            var estudantesComMulta = await _context.Estudantes
                .Where(e => e.Multa > 0) // Filtra estudantes com multa maior que zero
                .ToListAsync();

            return View(estudantesComMulta); // Retorna a lista para a view
        }
    }
}
