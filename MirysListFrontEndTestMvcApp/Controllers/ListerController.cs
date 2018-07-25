using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MirysListFrontEndTestMvcApp.Controllers
{
    public class ListerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}