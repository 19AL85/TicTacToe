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

        private List<FieldCell> GetField()
        {
            var field = new List<FieldCell>();
            for (int i = 0; i < 9; i++)
            {
                field.Add(new FieldCell { Position = i });
            }
            return field;
        }

        public void Player2DisconnectReset()
        {
            Player1.IsMyTurn = true;
            Player1.Sign = WebConst.ImgCross;

            Player2 = null;
            IsWaiting = true;
            Field = GetField();
        }
        public void Player1DisconectReset()
        {
            Player1 = Player2;
            Player1.IsMyTurn = true;
            Player1.Sign = WebConst.ImgCross;
            Player2 = null;

            IsWaiting = true;
            Field = GetField();
        }

        public void ResetField()
        {
            Field = GetField();
        }

        public void SwapPlayerTurns()
        {
            Player1.IsMyTurn = !Player1.IsMyTurn;
            Player2.IsMyTurn = !Player2.IsMyTurn;
        }

        private bool CheckRows(string sign)
        {
            if (Field[0].Sign == sign && Field[1].Sign == sign && Field[2].Sign == sign)
                return true;
            if (Field[3].Sign == sign && Field[4].Sign == sign && Field[5].Sign == sign)
                return true;
            if (Field[6].Sign == sign && Field[7].Sign == sign && Field[8].Sign == sign)
                return true;

            return false;
        }
        private bool CheckColumns(string sign)
        {
            if (Field[0].Sign == sign && Field[3].Sign == sign && Field[6].Sign == sign)
                return true;
            if (Field[1].Sign == sign && Field[4].Sign == sign && Field[7].Sign == sign)
                return true;
            if (Field[2].Sign == sign && Field[5].Sign == sign && Field[8].Sign == sign)
                return true;

            return false;
        }
        private bool CheckDiagonals(string sign)
        {
            if (Field[0].Sign == sign && Field[4].Sign == sign && Field[8].Sign == sign)
                return true;
            if (Field[2].Sign == sign && Field[4].Sign == sign && Field[6].Sign == sign)
                return true;

            return false;
        }

        public bool CheckWinner(string sign)
        {
            if (CheckRows(sign) || CheckColumns(sign) || CheckDiagonals(sign))
                return true;

            return false;
        }
    }
}
