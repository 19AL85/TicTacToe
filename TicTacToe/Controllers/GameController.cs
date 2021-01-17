using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameManager _gameManager;
        private readonly ApplicationDbContext _db;


        public GameController(IGameManager gameManager, ApplicationDbContext db)
        {
            _gameManager = gameManager;
            _db = db;
        }

        public IActionResult Index()
        {
            //IEnumerable<IdentityUser> users = _db.Users;
            IEnumerable<IdentityUser> users = _gameManager.ReadyUsers;
            return View(users);
        }


        public IActionResult GetReady()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!_gameManager.ReadyUsers.Any(x => x.Id == userId))
                _gameManager.ReadyUsers.Add(_db.Users.Find(userId));

            return RedirectToAction(nameof(Index));
        }
    }
}
