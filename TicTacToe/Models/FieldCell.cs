using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class FieldCell
    {
        public int Position { get; set; }
        public string Sign { get; set; }
        public bool IsFree { get; set; } = true;

    }
}
