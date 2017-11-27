using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LedgerWeb.Models;

namespace LedgerWeb.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        public ActionResult Random()
        {
            var user = new UserAccount() { Name = "Bob" };

            return View(user);
        }
    }
}