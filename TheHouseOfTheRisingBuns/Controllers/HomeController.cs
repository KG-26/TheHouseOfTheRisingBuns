using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheHouseOfTheRisingBuns.Models;
using TheHouseOfTheRisingBuns.Data;

namespace TheHouseOfTheRisingBuns.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Help()
        {
            ViewData["Message"] = "Your application help page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Thanks for using our site! We love to hear for our users So if you have any questions or feedback regarding our site please don't hesitate to get in contact with a member of our team";

            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult MailingList()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MailingList([Bind("ID,Email")] SignUp signUp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(signUp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }


}
