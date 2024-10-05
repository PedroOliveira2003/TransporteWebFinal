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
    [Authorize(Roles = "Funcionario")]
    public class PontosController : Controller
    {
        private readonly Contexto _context;

        public PontosController(Contexto context)
        {
            _context = context;
        }

        // GET: Pontos
        public async Task<IActionResult> Index(string searchString, int? page, int pageSize = 10)
        {
            int pageNumber = page ?? 1;  // Se o número da página não for fornecido, default é 1.
            var pontos = from p in _context.Pontos
                         select p;

            // Aplica o filtro de busca
            if (!String.IsNullOrEmpty(searchString))
            {
                pontos = pontos.Where(p => p.nomeponto.Contains(searchString));
            }

            int totalItems = await pontos.CountAsync(); // Conta o total de itens após o filtro.

            // Aplica a paginação
            var pontosPaginados = await pontos
                .OrderBy(p => p.nomeponto) // Ordena os pontos por nome.
                .Skip((pageNumber - 1) * pageSize) // Pula os registros das páginas anteriores.
                .Take(pageSize) // Pega o número de registros da página atual.
                .ToListAsync();

            // Passa o número de páginas e outros dados para a View
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.SearchString = searchString;

            return View(pontosPaginados);
        }

        // GET: Pontos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pontos == null)
            {
                return NotFound();
            }

            var ponto = await _context.Pontos
                .FirstOrDefaultAsync(m => m.id == id);
            if (ponto == null)
            {
                return NotFound();
            }

            return View(ponto);
        }

        // GET: Pontos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pontos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nomeponto")] Ponto ponto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ponto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ponto);
        }

        // GET: Pontos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pontos == null)
            {
                return NotFound();
            }

            var ponto = await _context.Pontos.FindAsync(id);
            if (ponto == null)
            {
                return NotFound();
            }
            return View(ponto);
        }

        // POST: Pontos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nomeponto")] Ponto ponto)
        {
            if (id != ponto.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ponto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PontoExists(ponto.id))
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
            return View(ponto);
        }

        // GET: Pontos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pontos == null)
            {
                return NotFound();
            }

            var ponto = await _context.Pontos
                .FirstOrDefaultAsync(m => m.id == id);
            if (ponto == null)
            {
                return NotFound();
            }

            return View(ponto);
        }

        // POST: Pontos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pontos == null)
            {
                return Problem("Entity set 'Contexto.Pontos'  is null.");
            }
            var ponto = await _context.Pontos.FindAsync(id);
            if (ponto != null)
            {
                _context.Pontos.Remove(ponto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PontoExists(int id)
        {
            return _context.Pontos.Any(e => e.id == id);
        }
    }
}
