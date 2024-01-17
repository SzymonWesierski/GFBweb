using GameWeb.Data;
using GameWeb.Entities;
using GameWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GameWeb.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUsers> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUsers> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var gamesList = await _context.Games
            .OrderByDescending(g => g.Rating)
            .ToListAsync() ?? new List<Games> ();

        if (gamesList.Count < 0)
        {
            return NotFound();
        }

        return View(gamesList);
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

        var currentUser = await _userManager.GetUserAsync(User) ?? new ApplicationUsers();

        if (currentUser == null)
        {
            return NotFound();
        }

        var rating = await _context.Ratings
        .FirstOrDefaultAsync(r => r.GameId == id && r.UserId == currentUser.Id);

        if (rating == null)
        {
            rating = new Ratings();
        }

        var viewModel = new DetailsGameViewModel
        {
            Game = game,
            Ratings = rating
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> _RatingForm(DetailsGameViewModel viewModel)
    {
        var currentUser = await _userManager.GetUserAsync(User) ?? new ApplicationUsers();

        if (currentUser == null)
        {
            return NotFound();
        }

        var game = await _context.Games
            .FirstOrDefaultAsync(m => m.Id == viewModel.Game.Id);

        if (game == null)
        {
            return NotFound();
        }

        var ratingDB = await _context.Ratings
            .FirstOrDefaultAsync(r => r.GameId == viewModel.Game.Id && r.UserId == currentUser.Id);

        int TotalStarsAfterOperation = game.TotalStars;

        if (ratingDB == null)
        {
            // Add new rating if not exist
            var rating = new Ratings()
            {
                Value = viewModel.Ratings.Value,
                GameId = viewModel.Game.Id,
                UserId = currentUser.Id,
                User = currentUser,
                Game = game
            };
            
            _context.Add(rating);
            TotalStarsAfterOperation += rating.Value;
            game.NumberOfVotes++;
        }
        else
        { 
            TotalStarsAfterOperation += viewModel.Ratings.Value - ratingDB.Value;
            // Update rating if exist
            ratingDB.Value = viewModel.Ratings.Value;
            _context.Update(ratingDB);
        }

        var result = await _context.SaveChangesAsync();

        if(result > 0)
        {
            game.TotalStars = TotalStarsAfterOperation;
            game.Rating = (double)game.TotalStars / (double)game.NumberOfVotes;
            _context.Update(game);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("GameDetails", new { id = viewModel.Game.Id });
    }
}