using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class GameManager
    {
        public List<Game> Games { get; set; }
        public int GameIdCounter { get; private set; } = 0;

        public GameManager()
        {
            Games = new List<Game>();
        }

        private Game CreateGame(Player player)
        {
            player.GameId= ++GameIdCounter;
            var game = new Game {Id= GameIdCounter, Player1 = player};
            return game;
        }

        public async Task<Game> JoinToGameAsync(Player player)
        {
            return await Task.Run(()=> JoinToGame(player));
        }

        public Game JoinToGame(Player player)
        {
            var game = Games.FirstOrDefault(x => x.IsWaiting == true);
            if (game != null)
            {
                
                player.GameId = game.Id;
                player.Sign = WebConst.ImgRing;

                //if (game.Player1 == null)
                //{
                //    player.Sign = 'X';
                //    game.Player1 = player;

                //}
                //else
                //{
                //    player.Sign = '0';
                //    game.Player2 = player;
                //}
                game.Player2 = player;
                game.IsWaiting = false;
            }
            else
            {
                player.Sign = WebConst.ImgCross;
                player.IsMyTurn = true;
                game=CreateGame(player);
                Games.Add(game);
            }
            //while (game.Player2 == null)
            //{

            //}
            return game;
        }

        


    }
}
