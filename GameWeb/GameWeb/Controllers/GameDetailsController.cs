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
public class GameDetailsController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUsers> _userManager;

    public GameDetailsController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUsers> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    // GET: GameDetails/Index/5
    public async Task<IActionResult> Index(int? id)
    {
        if (id == null || _context.Games == null)
        {
            return NotFound();
        }
        var game = await _context.Games
            .Include(t => t.GamesAndCategoriesList)
            .ThenInclude(gc => gc.GameCategory)
            .Include(c => c.CommentsList)
            .ThenInclude(u => u.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (game == null)
        {
            return NotFound();
        }

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
            Rating = rating
        };

        return View(viewModel);
    }

    // POST: GameDetails/Index/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> _RatingFormPartial(RatingFormPartialViewModel viewModel)
    {
        var currentUser = await _userManager.GetUserAsync(User) ?? new ApplicationUsers();

        if (currentUser == null)
        {
            return NotFound();
        }

        var game = await _context.Games
            .FirstOrDefaultAsync(m => m.Id == viewModel.GameId);

        if (game == null)
        {
            return NotFound();
        }

        var ratingDB = await _context.Ratings
            .FirstOrDefaultAsync(r => r.GameId == viewModel.GameId && r.UserId == currentUser.Id);

        int TotalStarsAfterOperation = game.TotalStars;

        if (ratingDB == null)
        {
            // Add new rating if not exist
            var rating = new Ratings()
            {
                Value = viewModel.RatingValue,
                GameId = viewModel.GameId,
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
            TotalStarsAfterOperation += viewModel.RatingValue - ratingDB.Value;
            // Update rating if exist
            ratingDB.Value = viewModel.RatingValue;
            _context.Update(ratingDB);
        }

        var result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            game.TotalStars = TotalStarsAfterOperation;
            game.Rating = (double)game.TotalStars / (double)game.NumberOfVotes;
            _context.Update(game);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", new { id = viewModel.GameId });
    }

    [HttpPost, ActionName("_CommentFormPartial")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> _CommentCreateFormPartial(CommentFormPartialViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            TempData["CommentError"] = "Musisz wprowadzić treść komentarza";
            return RedirectToAction("Index", new { id = viewModel.GameId });
        }
        var currentUser = await _userManager.GetUserAsync(User) ?? new ApplicationUsers();

        if (currentUser == null)
        {
            return NotFound();
        }

        var game = await _context.Games
            .FirstOrDefaultAsync(m => m.Id == viewModel.GameId);

        if (game == null)
        {
            return NotFound();
        }

        // Add new rating if not exist
        var comment = new Comments()
        {
            Content = viewModel.CommentContent,
            GameId = viewModel.GameId,
            UserId = currentUser.Id,
            User = currentUser,
            Game = game
        };

        _context.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", new { id = viewModel.GameId });
    }

    // GET: Home/GameDetails/EditComment/5
    public async Task<IActionResult> EditComment(int id)
    {
        var commentDB = await _context.Comments
            .FirstOrDefaultAsync(m => m.Id == id);

        if (commentDB == null) return NotFound();

        var viewModel = new EditAndDeleteCommentViewModel
        {
            CommentId = commentDB.Id,
            CommentContent = commentDB.Content,
            GameId = commentDB.GameId
        };

        return View(viewModel);
    }

    // POST: Home/GameDetails/EditComment/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditComment(EditAndDeleteCommentViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var commentDB = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == viewModel.CommentId);

            if (commentDB == null) return NotFound();

            commentDB.Content = viewModel.CommentContent;

            _context.Update(commentDB);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = viewModel.GameId });
        }

        return View(viewModel);
       
    }

    // GET: Home/GameDetails/DeleteComment/5
    public async Task<IActionResult> DeleteComment(int id)
    {
        var commentDB = await _context.Comments
            .FirstOrDefaultAsync(m => m.Id == id);

        if (commentDB == null) return NotFound();

        var viewModel = new EditAndDeleteCommentViewModel
        {
            CommentContent = commentDB.Content,
            GameId = commentDB.GameId,
            CommentId = commentDB.Id
        };

        return View(viewModel);
    }

    // POST: Home/GameDetails/DeleteComment/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(EditAndDeleteCommentViewModel viewModel)
    {
        var commentDB = await _context.Comments
             .FirstOrDefaultAsync(m => m.Id == viewModel.CommentId);

        if (commentDB == null) return NotFound();

        _context.Remove(commentDB);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", new { id = viewModel.GameId });
    }
}