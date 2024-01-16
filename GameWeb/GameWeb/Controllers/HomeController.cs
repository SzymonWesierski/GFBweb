using GameWeb.Data;
using GameWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GameWeb.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return _context.Games != null ?
                      View(await _context.Games.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Games'  is null.");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult IndexGuest()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // GET: Home/GameDetails/5
    public async Task<IActionResult> GameDetails(int? id)
    {
        if (id == null || _context.Games == null)
        {
            return NotFound();
        }
        var game = await _context.Games
            .Include(t => t.GamesAndCategoriesList)
            .ThenInclude(gc => gc.GameCategory)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (game == null)
        {
            return NotFound();
        }

        game.CategoryList = game.GamesAndCategoriesList.Select(gc => gc.GameCategory).ToList();

        return View(game);
    }
}