using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.SignalR
{
    public class AppHub : Hub
    {
        //public async Task ReadyToGame(string mail, string count)
        //{
        //    await this.Clients.All.SendAsync("ReadyToGame", mail, count);
        //}
        GameManager _gameManager;
        ApplicationDbContext _db;

        public AppHub(GameManager gameManager, ApplicationDbContext db)
        {
            _gameManager = gameManager;
            _db = db;
        }

        public async override Task OnConnectedAsync()
        {
            if (this.Context.User.Identity.IsAuthenticated)
            {
                var userId = this.Context.UserIdentifier;
                var user = _db.Users.Find(userId);
                var player = new Player { ConnectionId = Context.ConnectionId, User = user, UserId = user.Id, UserEmail = user.Email };
                var game = _gameManager.JoinToGame(player);

                if (game.Player2 != null)
                {
                    await Clients.Client(game.Player1.ConnectionId).SendAsync("JoinToGame", new { player1 = game.Player1, player2 = game.Player2 });
                    await Clients.Caller.SendAsync("JoinToGame", new { player1 = game.Player1, player2 = game.Player2 });
                }
                else
                    await Clients.Caller.SendAsync("JoinToGame", new { player1 = game.Player1, player2=new { userEmail = "Waiting" } });
            }

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if (this.Context.User.Identity.IsAuthenticated)
            {
                var userId = this.Context.UserIdentifier;
                var game = _gameManager.Games.FirstOrDefault(x => x.Player1.User.Id == userId || x.Player2.User.Id == userId);

                //if(game.Player1==null && game.Player2==null)
                //_gameManager.Games.Remove(game);

                if (game.Player1.User.Id == userId)
                {
                    //await Clients.Client(game.Player2.ConnectionId).SendAsync("Notify", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
                    await Clients.Client(game.Player2.ConnectionId).SendAsync("RedirectToHome", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
                    game.Player1 = null;
                    game.IsWaiting = true;
                }
                else
                {
                    //await Clients.Client(game.Player1.ConnectionId).SendAsync("Notify", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
                    await Clients.Client(game.Player1.ConnectionId).SendAsync("RedirectToHome", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
                    game.Player2 = null;
                    game.IsWaiting = true;
                }
                _gameManager.Games.Remove(game);
                //var user = _gameManager.ReadyUsers.FirstOrDefault(x => x.Id == userId);
                //_gameManager.ReadyUsers.Remove(user);

                //var userEmail = this.Context.User.FindFirstValue(ClaimTypes.Email);

            }

            await base.OnDisconnectedAsync(exception);
        }


        //public async override Task OnDisconnectedAsync(Exception exception)
        //{
        //    if (this.Context.User.Identity.IsAuthenticated)
        //    {
        //        var userId = this.Context.UserIdentifier;
        //        var game = _gameManager.Games.FirstOrDefault(x => x.Player1.User.Id == userId || x.Player2.User.Id == userId);

        //        //if(game.Player1==null && game.Player2==null)
        //            //_gameManager.Games.Remove(game);

        //        if (game.Player1.User.Id == userId)
        //        {
        //            //await Clients.Client(game.Player2.ConnectionId).SendAsync("Notify", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
        //            await Clients.Client(game.Player2.ConnectionId).SendAsync("RedirectToHome", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
        //            game.Player1 = null;
        //            game.IsWaiting = true;
        //        }
        //        else
        //        {
        //            //await Clients.Client(game.Player1.ConnectionId).SendAsync("Notify", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
        //            await Clients.Client(game.Player1.ConnectionId).SendAsync("RedirectToHome", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
        //            game.Player2 = null;
        //            game.IsWaiting = true;
        //        }
        //        _gameManager.Games.Remove(game);
        //        //var user = _gameManager.ReadyUsers.FirstOrDefault(x => x.Id == userId);
        //        //_gameManager.ReadyUsers.Remove(user);

        //        //var userEmail = this.Context.User.FindFirstValue(ClaimTypes.Email);

        //    }

        //    await base.OnDisconnectedAsync(exception);
        //}


        //public async override Task OnConnectedAsync()
        //{



        //    if (this.Context.User.Identity.IsAuthenticated)
        //    {
        //        var userId = this.Context.UserIdentifier;
        //        var game = _gameManager.Games.FirstOrDefault(x => x.Player1.User.Id == userId || x.Player2.User.Id == userId);
        //        Player player;

        //        if (game.Player1.User.Id == userId)
        //        {
        //            player = game.Player1;
        //        }
        //        else
        //        {
        //            player = game.Player2;

        //        }

        //        player.ConnectionId = Context.ConnectionId;
        //        //var user = _db.Users.Find(userId);
        //        //_gameManager.Players.Add(new Player(user, true));

        //    }


        //    await base.OnConnectedAsync();
        //}
    }
}
