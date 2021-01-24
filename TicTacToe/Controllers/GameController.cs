using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicTacToe.Data;
using TicTacToe.Models;
using TicTacToe.SignalR;

namespace TicTacToe.Controllers
{
    public class GameController : Controller
    {

        public IActionResult Index()
        {

            return View();
        }



      
    }
}
