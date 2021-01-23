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

        public List<FieldCell> Field { get; set; }
        //public char[] field { get; set; } = new char[8];

        public Game()
        {
            Field = GetField();
        }

        public List<FieldCell> GetField()
        {
            var field = new List<FieldCell>();
            for (int i = 0; i < 9; i++)
            {
                field.Add(new FieldCell { Position = i });
            }
            return field;
        }

        public void ResetPlayer1()
        {
            Player1.IsMyTurn = true;
            Player1.Sign = WebConst.ImgCross;

            Player2 = null;
            IsWaiting = true;
            Field = GetField();
        }
        public void ResetPlayer2()
        {
            Player1 = Player2;
            Player1.IsMyTurn = true;
            Player1.Sign = WebConst.ImgCross;
            Player2 = null;

            IsWaiting = true;
            Field = GetField();
        }

        public void SwapPlayerTurns()
        {
            Player1.IsMyTurn = !Player1.IsMyTurn;
            Player2.IsMyTurn = !Player2.IsMyTurn;
        }
    }
}
