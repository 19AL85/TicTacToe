using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public interface IGameManager
    {
        List<IdentityUser> ReadyUsers { get; set; }
    }
}
