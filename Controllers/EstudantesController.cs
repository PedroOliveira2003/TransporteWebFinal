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
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Estudantes.Include(e => e.curso);
            return View(await contexto.ToListAsync());
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
