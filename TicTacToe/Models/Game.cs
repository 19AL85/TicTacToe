using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class Game
    {
        public int Id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public bool IsWaiting { get; set; } = true;

        public char[,] field { get; set; } = {{ ' ', ' ', ' ' },
                                              { ' ', ' ', ' ' },
                                              { ' ', ' ', ' ' }};

    }
}
