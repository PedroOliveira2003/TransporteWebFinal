using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TransporteWeb.Models;

namespace TransporteWeb.Controllers
{
    [Authorize(Roles = "Funcionario")]
    public class EstudantesController : Controller
    {
        private readonly Contexto _context;

        public EstudantesController(Contexto context)
        {
            _context = context;
        }

        // GET: Estudantes
        public async Task<IActionResult> Index(string searchString, int? page, int pageSize = 10)
        {
            int pageNumber = page ?? 1;  // Se o número da página não for fornecido, default é 1.
            var estudantes = from e in _context.Estudantes.Include(e => e.curso)
                             select e;

            // Aplica o filtro de busca
            if (!String.IsNullOrEmpty(searchString))
            {
                estudantes = estudantes.Where(s => s.nome.Contains(searchString));
            }

            int totalItems = await estudantes.CountAsync(); // Conta o total de itens após o filtro.

            // Aplica a paginação
            var estudantesPaginados = await estudantes
                .OrderBy(s => s.nome) // Ordena os estudantes por nome.
                .Skip((pageNumber - 1) * pageSize) // Pula os registros das páginas anteriores.
                .Take(pageSize) // Pega o número de registros da página atual.
                .ToListAsync();

            // Passa o número de páginas e outros dados para a View
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.SearchString = searchString;

            return View(estudantesPaginados);
        }

        // GET: Estudantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Estudantes == null)
            {
                return NotFound();
            }

            var estudante = await _context.Estudantes
                .Include(e => e.curso)
                .FirstOrDefaultAsync(m => m.id == id);
            if (estudante == null)
            {
                return NotFound();
            }

            return View(estudante);
        }

        // GET: Estudantes/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "id", "nome");
            return View();
        }

        // POST: Estudantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nome,telefone,IdCurso")] Estudante estudante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "id", "nome", estudante.IdCurso);
            return View(estudante);
        }

        // GET: Estudantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Estudantes == null)
            {
                return NotFound();
            }

            var estudante = await _context.Estudantes.FindAsync(id);
            if (estudante == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "id", "nome", estudante.IdCurso);
            return View(estudante);
        }

        // POST: Estudantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nome,telefone,IdCurso")] Estudante estudante)
        {
            if (id != estudante.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudanteExists(estudante.id))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "id", "nome", estudante.IdCurso);
            return View(estudante);
        }

        // GET: Estudantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Estudantes == null)
            {
                return NotFound();
            }

            var estudante = await _context.Estudantes
                .Include(e => e.curso)
                .FirstOrDefaultAsync(m => m.id == id);
            if (estudante == null)
            {
                return NotFound();
            }

            return View(estudante);
        }

        // POST: Estudantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Estudantes == null)
            {
                return Problem("Entity set 'Contexto.Estudantes'  is null.");
            }
            var estudante = await _context.Estudantes.FindAsync(id);
            if (estudante != null)
            {
                _context.Estudantes.Remove(estudante);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudanteExists(int id)
        {
          return _context.Estudantes.Any(e => e.id == id);
        }
    }
}
