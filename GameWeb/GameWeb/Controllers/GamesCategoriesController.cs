using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameWeb.Data;
using GameWeb.Entities;

namespace GameWeb.Controllers
{
    public class GamesCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GamesCategories
        public async Task<IActionResult> Index()
        {
              return _context.GamesCategories != null ? 
                          View(await _context.GamesCategories.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.GamesCategories'  is null.");
        }

        // GET: GamesCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GamesCategories == null)
            {
                return NotFound();
            }

            var gamesCategories = await _context.GamesCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamesCategories == null)
            {
                return NotFound();
            }

            return View(gamesCategories);
        }

        // GET: GamesCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GamesCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName,Created")] GamesCategories gamesCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gamesCategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gamesCategories);
        }

        // GET: GamesCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GamesCategories == null)
            {
                return NotFound();
            }

            var gamesCategories = await _context.GamesCategories.FindAsync(id);
            if (gamesCategories == null)
            {
                return NotFound();
            }
            return View(gamesCategories);
        }

        // POST: GamesCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName,Created")] GamesCategories gamesCategories)
        {
            if (id != gamesCategories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gamesCategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GamesCategoriesExists(gamesCategories.Id))
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
            return View(gamesCategories);
        }

        // GET: GamesCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GamesCategories == null)
            {
                return NotFound();
            }

            var gamesCategories = await _context.GamesCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gamesCategories == null)
            {
                return NotFound();
            }

            return View(gamesCategories);
        }

        // POST: GamesCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GamesCategories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GamesCategories'  is null.");
            }
            var gamesCategories = await _context.GamesCategories.FindAsync(id);
            if (gamesCategories != null)
            {
                _context.GamesCategories.Remove(gamesCategories);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GamesCategoriesExists(int id)
        {
          return (_context.GamesCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
