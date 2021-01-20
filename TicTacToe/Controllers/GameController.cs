using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicTacToe.Data;
using TicTacToe.Models;
using TicTacToe.Models.ViewModels;
using TicTacToe.SignalR;

namespace TicTacToe.Controllers
{
    public class GameController : Controller
    {
        private readonly GameManager _gameManager;
        private readonly ApplicationDbContext _db;
        private readonly IHubContext<AppHub> _hubContext;



        public GameController(GameManager gameManager, ApplicationDbContext db, IHubContext<AppHub> hubContext)
        {
            _gameManager = gameManager;
            _db = db;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = _db.Users.Find(userId);
            //var player = new Player(user) { UserEmail = user.Email, UserId = user.Id };

            //var game = await _gameManager.JoinToGameAsync(player);
            //if (game.Player1 != null && game.Player2 != null)
            //{
            //    await _hubContext.Clients.Client(game.Player1.ConnectionId).SendAsync("Name", game.Player2.User.Email);
            //}

            //return View(game);
            return View();
        }



        public async Task<IActionResult> GetReady()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            //if (!_gameManager.ReadyUsers.Any(x => x.Id == userId))
            //{
            //    _gameManager.ReadyUsers.Add(_db.Users.Find(userId));
            //    //await _hubContext.Clients.All.SendAsync("ReadyToGame", userEmail, _gameManager.ReadyUsers.Count);

            //}

            return RedirectToAction(nameof(Index));
        }
    }
}
