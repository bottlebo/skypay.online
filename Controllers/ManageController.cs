using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SkyPay.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index(string link)
        {
            return View();
        }
    }
}
