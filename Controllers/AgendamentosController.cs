using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TransporteWeb.Models;

namespace TransporteWeb.Controllers
{
    [Authorize]
    public class AgendamentosController : Controller
    {
        private readonly Contexto _context;

        public AgendamentosController(Contexto context)
        {
            _context = context;
        }

        // GET: Agendamentos
        public async Task<IActionResult> Index(string searchString, int? page, int pageSize = 10)
        {
            int pageNumber = page ?? 1; // Se o número da página não for fornecido, default é 1.
            var agendamentos = from a in _context.Agendamentos
                               .Include(a => a.estudante)
                               .Include(a => a.veiculo)
                               .Include(a => a.ponto)
                               select a;

            // Aplica o filtro de busca
            if (!string.IsNullOrEmpty(searchString))
            {
                agendamentos = agendamentos.Where(a => a.estudante.nome.Contains(searchString) ||
                                                        a.veiculo.nomeveiculo.Contains(searchString) ||
                                                        a.ponto.nomeponto.Contains(searchString));
            }

            int totalItems = await agendamentos.CountAsync(); // Conta o total de itens após o filtro.

            // Aplica a paginação
            var agendamentosPaginados = await agendamentos
                .OrderBy(a => a.data) // Ordena os agendamentos pela data.
                .Skip((pageNumber - 1) * pageSize) // Pula os registros das páginas anteriores.
                .Take(pageSize) // Pega o número de registros da página atual.
                .ToListAsync();

            // Passa o número de páginas e outros dados para a View
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.SearchString = searchString;

            return View(agendamentosPaginados);
        }

        // GET: Agendamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Agendamentos == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.estudante)
                .Include(a => a.veiculo)
                .Include(a => a.ponto)
                .FirstOrDefaultAsync(m => m.id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamentos/Create
        public IActionResult Create()
        {
            ViewData["IdEstudante"] = new SelectList(_context.Estudantes, "id", "nome");
            ViewData["IdVeiculo"] = new SelectList(_context.Veiculos, "id", "nomeveiculo");
            ViewData["IdPonto"] = new SelectList(_context.Pontos, "id", "nomeponto");
            return View();
        }

        // POST: Agendamentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,data,IdEstudante,IdVeiculo,IdPonto,TipoViagem")] Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                // Verifica o número de agendamentos do estudante
                var agendamentosEstudante = await _context.Agendamentos
                    .CountAsync(a => a.IdEstudante == agendamento.IdEstudante);

                if (agendamentosEstudante >= 2)
                {
                    ModelState.AddModelError("", "VOCE POSSUI 2 AGENDAMENTOS. CANCELE UM.");
                    ViewData["IdEstudante"] = new SelectList(_context.Estudantes, "id", "nome", agendamento.IdEstudante);
                    ViewData["IdVeiculo"] = new SelectList(_context.Veiculos, "id", "nomeveiculo", agendamento.IdVeiculo);
                    ViewData["IdPonto"] = new SelectList(_context.Pontos, "id", "nomeponto", agendamento.IdPonto);
                    return View(agendamento);
                }

                // Encontra o veículo correspondente ao agendamento
                Veiculo veiculo = await _context.Veiculos.FindAsync(agendamento.IdVeiculo);

                // Verifica se o veículo foi encontrado
                if (veiculo == null)
                {
                    ModelState.AddModelError("", "Veículo não encontrado.");
                    return View(agendamento);
                }

                // Verifica se há vagas disponíveis
                if (veiculo.vagas <= 0)
                {
                    ModelState.AddModelError("", "ÔNIBUS LOTADO. Não é possível fazer o agendamento.");
                    ViewData["IdEstudante"] = new SelectList(_context.Estudantes, "id", "nome", agendamento.IdEstudante);
                    ViewData["IdVeiculo"] = new SelectList(_context.Veiculos, "id", "nomeveiculo", agendamento.IdVeiculo);
                    ViewData["IdPonto"] = new SelectList(_context.Pontos, "id", "nomeponto", agendamento.IdPonto);
                    return View(agendamento);
                }

                // Diminui o número de vagas
                veiculo.vagas--;

                // Atualiza o veículo e adiciona o agendamento
                _context.Update(veiculo);
                _context.Add(agendamento);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Se o ModelState não for válido, repopula os dados
            ViewData["IdEstudante"] = new SelectList(_context.Estudantes, "id", "nome", agendamento.IdEstudante);
            ViewData["IdVeiculo"] = new SelectList(_context.Veiculos, "id", "nomeveiculo", agendamento.IdVeiculo);
            ViewData["IdPonto"] = new SelectList(_context.Pontos, "id", "nomeponto", agendamento.IdPonto);
            return View(agendamento);
        }

        // GET: Agendamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Agendamentos == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound();
            }
            ViewData["IdEstudante"] = new SelectList(_context.Estudantes, "id", "nome", agendamento.IdEstudante);
            ViewData["IdVeiculo"] = new SelectList(_context.Veiculos, "id", "nomeveiculo", agendamento.IdVeiculo);
            ViewData["IdPonto"] = new SelectList(_context.Pontos, "id", "nomeponto", agendamento.IdPonto);
            return View(agendamento);
        }

        // POST: Agendamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,data,IdEstudante,IdVeiculo,IdPonto,TipoViagem")] Agendamento agendamento)
        {
            if (id != agendamento.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agendamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendamentoExists(agendamento.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstudante"] = new SelectList(_context.Estudantes, "id", "nome", agendamento.IdEstudante);
            ViewData["IdVeiculo"] = new SelectList(_context.Veiculos, "id", "nomeveiculo", agendamento.IdVeiculo);
            ViewData["IdPonto"] = new SelectList(_context.Pontos, "id", "nomeponto", agendamento.IdPonto);
            return View(agendamento);
        }

        // GET: Agendamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Agendamentos == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.estudante)
                .Include(a => a.veiculo)
                .Include(a => a.ponto)
                .FirstOrDefaultAsync(m => m.id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Agendamentos == null)
            {
                return Problem("Entity set 'Contexto.Agendamentos' is null.");
            }
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgendamentoExists(int id)
        {
            return _context.Agendamentos.Any(e => e.id == id);
        }
    }
}
