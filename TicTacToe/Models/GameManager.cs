using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class GameManager : IGameManager
    {
        public List<IdentityUser> ReadyUsers { get; set; }
        public GameManager()
        {
            ReadyUsers = new List<IdentityUser>();
        }
    }
}
