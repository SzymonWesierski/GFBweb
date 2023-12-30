﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameWeb.Data;
using GameWeb.Models;
using GameWeb.Entities;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Authorization;

namespace GameWeb.Controllers;

[Authorize]
public class GamesController : Controller
{
    private readonly ApplicationDbContext _context;

    public GamesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Games
    public async Task<IActionResult> Index()
    {
          return _context.Games != null ? 
                      View(await _context.Games.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Games'  is null.");
    }

    // GET: Games/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Games == null)
        {
            return NotFound();
        }

        var games = await _context.Games
            .FirstOrDefaultAsync(m => m.Id == id);
        if (games == null)
        {
            return NotFound();
        }

        return View(games);
    }

    // GET: Games/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new GamesCreateViewModel
        {
            Game = new Games(),
            GamesCategories = await _context.GamesCategories.ToListAsync() ?? new List<GamesCategories>(),
            SelectedCategoryIds = new List<int>()
        };

        return View(viewModel);
    }

    // POST: Games/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GamesCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.GamesCategories = await _context.GamesCategories.ToListAsync() ?? new List<GamesCategories>();

            return View(viewModel);
        }

        _context.Add(viewModel.Game);
        await _context.SaveChangesAsync();

        if (viewModel.SelectedCategoryIds != null)
        {
            foreach (var categoryId in viewModel.SelectedCategoryIds)
            {
                viewModel.Game.GamesAndCategoriesList.Add(new GamesAndCategories
                {
                    GameId = viewModel.Game.Id,
                    GameCategoryId = categoryId
                });
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Games/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Games == null) return NotFound();

        var game = await _context.Games
        .Include(t => t.GamesAndCategoriesList)
        .FirstOrDefaultAsync(m => m.Id == id) ?? new Games();

        if (game == null) return NotFound();

        var gamesCategories = await _context.GamesCategories.ToListAsync() ?? new List<GamesCategories>();

        if(gamesCategories == null) return NotFound();

        var SelectedCategoryIds = new List<int>();

        foreach (var row in game.GamesAndCategoriesList)
        {
            SelectedCategoryIds.Add(row.GameCategoryId);
        }

        var viewModel = new GamesCreateViewModel()
        {
            Game = game,
            GamesCategories = gamesCategories,
            SelectedCategoryIds = SelectedCategoryIds
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Created,Title")] Games gameModel, [Bind("SelectedCategoryIds")] GamesCreateViewModel viewModel)
    {
        viewModel.Game = gameModel;

        if (viewModel.Game == null)
        {
            return BadRequest("Invalid model state. Game is null.");
        }

        if (id != viewModel.Game.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Load the existing Game with its relationships from DB
                var existingGame = await _context.Games
                    .Include(t => t.GamesAndCategoriesList)
                    .FirstOrDefaultAsync(m => m.Id == id) ?? new Games();

                if (existingGame == null)
                {
                    return NotFound();
                }

                // Update the Game (except relationships)
                _context.Entry(existingGame).CurrentValues.SetValues(viewModel.Game);

                // Remove relationships that are no longer selected
                existingGame.GamesAndCategoriesList.RemoveAll(tc => !viewModel.SelectedCategoryIds.Contains(tc.GameCategoryId));

                // Add new relationships with categories
                foreach (var categoryId in viewModel.SelectedCategoryIds)
                {
                    if (!existingGame.GamesAndCategoriesList.Any(tc => tc.GameCategoryId == categoryId))
                    {
                        existingGame.GamesAndCategoriesList.Add(new GamesAndCategories { GameCategoryId = categoryId });
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GamesExists(viewModel.Game.Id))
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

        foreach (var modelStateKey in ModelState.Keys)
        {
            var modelStateVal = ModelState[modelStateKey];
            foreach (var error in modelStateVal.Errors)
            {
                Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
            }
        }

        viewModel.GamesCategories = await _context.GamesCategories.ToListAsync() ?? new List<GamesCategories>();

        return View(viewModel);
    }


    // GET: Games/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Games == null)
        {
            return NotFound();
        }

        var games = await _context.Games
            .FirstOrDefaultAsync(m => m.Id == id);
        if (games == null)
        {
            return NotFound();
        }

        return View(games);
    }


    // POST: Games/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Games == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Games'  is null.");
        }

        var gameFromDb = await _context.Games
        .Include(t => t.GamesAndCategoriesList)
        .FirstOrDefaultAsync(t => t.Id == id);

        if (gameFromDb != null)
        {
            // Remove associated records from the join table
            _context.GamesAndCategories.RemoveRange(gameFromDb.GamesAndCategoriesList);

            // Remove the task
            _context.Games.Remove(gameFromDb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return NotFound();
        }
    }

    private bool GamesExists(int id)
    {
      return (_context.Games?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
