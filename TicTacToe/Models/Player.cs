using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class Player
    {
        public IdentityUser User { get; set; }

        public string UserId { get; set; }
        public string UserEmail { get; set; }

        public string ConnectionId { get; set; }
        public int GameId{ get; set; }
        public char Sign { get; set; }

  
        public bool IsMyTurn { get; set; }

        public Player(IdentityUser user,  bool isMyTurn=false)
        {
            User = user;
            IsMyTurn = IsMyTurn;
        }
        public Player()
        {

        }

    }
}
