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

        public async Task MakeMove(Dictionary<string, string> data)
        {
            if (this.Context.User.Identity.IsAuthenticated)
            {
                var userId = this.Context.UserIdentifier;
                Int32.TryParse(data["gameId"], out int id);
                Int32.TryParse(data["cell"], out int position);
                var game = _gameManager.Games.FirstOrDefault(x => x.Id == id);
                Player player;

                if (game.Player1.UserId == userId)
                    player = game.Player1;
                else
                    player = game.Player2;

                if (!game.IsWaiting && player.IsMyTurn)
                {
                    var cell = game.Field.FirstOrDefault(x => x.Position == position);

                    if (game.Player1.UserId == userId)
                    {
                        if (cell.IsFree)
                        {
                            cell.Sign = game.Player1.Sign;
                            cell.IsFree = false;
                            await Clients.Client(game.Player2.ConnectionId).SendAsync("MakeMove", new { sign = game.Player1.Sign, cell = data["cell"] });
                            await Clients.Caller.SendAsync("MakeMove", new { sign = game.Player1.Sign, cell = data["cell"] });

                            if (game.CheckWinner(game.Player1.Sign))
                            {
                                await Clients.Client(game.Player2.ConnectionId).SendAsync("GameOver", $"{game.Player1.UserEmail} wins!!");
                                await Clients.Caller.SendAsync("GameOver", "You Win!");
                                game.ResetField();
                            }
                            game.SwapPlayerTurns();
                            await Clients.Client(game.Player2.ConnectionId).SendAsync("SetActivePlayer", new { player1=game.Player1.IsMyTurn, player2 = game.Player2.IsMyTurn });
                            await Clients.Caller.SendAsync("SetActivePlayer", new { player1 = game.Player1.IsMyTurn, player2 = game.Player2.IsMyTurn });

                        }
                        else
                        {
                            await Clients.Caller.SendAsync("AlertMessage", "Wrong Move");
                        }
                    }
                    else
                    {
                        if (cell.IsFree)
                        {
                            cell.Sign = game.Player2.Sign;
                            cell.IsFree = false;
                            await Clients.Client(game.Player1.ConnectionId).SendAsync("MakeMove", new { sign = game.Player2.Sign, cell = data["cell"] });
                            await Clients.Caller.SendAsync("MakeMove", new { sign = game.Player2.Sign, cell = data["cell"] });

                            if (game.CheckWinner(game.Player2.Sign))
                            {
                                await Clients.Client(game.Player1.ConnectionId).SendAsync("GameOver", $"{game.Player2.UserEmail} wins!!");
                                await Clients.Caller.SendAsync("GameOver", "You Win!");
                                game.ResetField();
                            }
                            game.SwapPlayerTurns();
                            await Clients.Client(game.Player1.ConnectionId).SendAsync("SetActivePlayer", new { player1 = game.Player1.IsMyTurn, player2 = game.Player2.IsMyTurn });
                            await Clients.Caller.SendAsync("SetActivePlayer", new { player1 = game.Player1.IsMyTurn, player2 = game.Player2.IsMyTurn });
                        }
                        else
                        {
                            await Clients.Caller.SendAsync("AlertMessage", "Wrong Move");
                        }
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("AlertMessage", "Not yor move");
                }
            }

        }

        public async override Task OnConnectedAsync()
        {
            if (this.Context.User.Identity.IsAuthenticated)
            {
                var userId = this.Context.UserIdentifier;
                var user = _db.Users.Find(userId);
                var player = new Player { ConnectionId = Context.ConnectionId, UserId = user.Id, UserEmail = user.Email };
                var game = _gameManager.JoinToGame(player);

                if (game.Player2 != null)
                {
                    await Clients.Client(game.Player1.ConnectionId).SendAsync("JoinToGame", new
                    {
                        player1 = game.Player1,
                        player2 = game.Player2,
                        gameId = game.Id,
                        msg = $"{game.Player2.UserEmail} connected"
                    });
                    await Clients.Caller.SendAsync("JoinToGame", new
                    {
                        player1 = game.Player1,
                        player2 = game.Player2,
                        gameId = game.Id,
                        msg = $"Game has begun"
                    });

                    await Clients.Client(game.Player1.ConnectionId).SendAsync("SetActivePlayer", new { player1 = game.Player1.IsMyTurn, player2 = game.Player2.IsMyTurn });
                    await Clients.Caller.SendAsync("SetActivePlayer", new { player1 = game.Player1.IsMyTurn, player2 = game.Player2.IsMyTurn });
                }
                else
                {
                    await Clients.Caller.SendAsync("JoinToGame", new
                    {
                        player1 = game.Player1,
                        player2 = new { userEmail = "Waiting" },
                        gameId = game.Id,
                        msg = "...Waiting opponent"
                    });
                    //await Clients.Caller.SendAsync("SetActivePlayer", new { player1 = game.Player1.IsMyTurn, player2 = game.Player2.IsMyTurn });
                }
                   
            }

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if (this.Context.User.Identity.IsAuthenticated)
            {
                var userId = this.Context.UserIdentifier;
                var game = _gameManager.Games.FirstOrDefault(x => x.Player1?.UserId == userId || x.Player2?.UserId == userId);

                if (game.IsWaiting)
                    _gameManager.Games.Remove(game);
                else
                {
                    if (game.Player1.UserId == userId)
                    {
                        await Clients.Client(game.Player2.ConnectionId).SendAsync("AlertMessage", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
                        game.ResetPlayer2();
                    }
                    else
                    {
                        await Clients.Client(game.Player1.ConnectionId).SendAsync("AlertMessage", $"{this.Context.User.FindFirstValue(ClaimTypes.Email)} disconnected");
                        game.ResetPlayer1();
                    }

                    await Clients.Client(game.Player1.ConnectionId).SendAsync("JoinToGame", new
                    {
                        player1 = game.Player1,
                        player2 = new { userEmail = "Waiting" },
                        gameId = game.Id,
                        msg = ""
                    });
                }

            }

            await base.OnDisconnectedAsync(exception);
        }



    }
}
