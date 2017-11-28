using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LedgerWeb.Models;
using System.Web.Caching;

namespace LedgerWeb.Controllers
{
    public class UserAccountController : Controller
    {

        public Cache cache = new Cache();

        public ActionResult UserIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string Password, string Password2)
        {
            // Checks if the passwords are the same
            if (!Password.Equals(Password2))
            {
                return RedirectToAction("Register", "Home");
            }

            // Checks if the username already exists
            if (cache[username] != null)
            {
                return RedirectToAction("Register", "Home");
            }

            // Creates new user
            UserAccount newUser = new UserAccount(username, Password);

            // Adds new user object to cache
            cache.Insert(username, newUser, null);

            // Put current user username in sessions to access later
            Session["CURRENTUSER"] = newUser;

            return View("UserIndex");
        }

        [HttpPost]
        public ActionResult LoggingIn(string username, string password)
        {
            UserAccount user;

            // Checks if user information is avaiable
            try
            {
                // Grab user information if in system
                user = (UserAccount) cache.Get(username);
                if (user == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }
            
            // Checks if they got the right password
            if (!password.Equals(user.password))
            {
                return RedirectToAction("Login", "Home");
            }

            // Records current user for later use
            Session["CURRENTUSER"] = user;

            return View("UserIndex");
        }




        public ActionResult Deposit()
        {
            return View();
        }

        public ActionResult Withdrawal()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(string amount)
        {
            UserAccount user = (UserAccount)Session["CURRENTUSER"];
            try
            {
                // Conversion from string to decimal
                decimal deposit = decimal.Parse(amount, System.Globalization.NumberStyles.Currency);

                // Ensure input is a positive value
                if (deposit < 0)
                {
                    throw new Exception();
                }

                // Add input to user balance
                user.balance += deposit;

                // Gather transaction information and add to history
                string transaction = user.DecimalToString(deposit) + " deposited";
                user.Report(transaction);
            }
            catch (Exception)
            {

                return View("UserIndex");
            }
            return View("UserIndex");
        }

        [HttpPost]
        public ActionResult Withdrawal(string amount)
        {
            UserAccount user = (UserAccount)Session["CURRENTUSER"];
            try
            {
                // Conversion from string to decimal
                decimal withdrawal = decimal.Parse(amount, System.Globalization.NumberStyles.Currency);

                // Ensure input is a positive value
                if (withdrawal < 0)
                {
                    throw new Exception();
                }

                // Add input to user balance
                user.balance -= withdrawal;

                // Gather transaction information and add to history
                string transaction = user.DecimalToString(withdrawal) + " withdrawn";
                user.Report(transaction);
            }
            catch (Exception)
            {
                return View("UserIndex");
            }
            return View("UserIndex");
        }



        public ActionResult CheckBalance()
        {
            UserAccount user = (UserAccount)Session["CURRENTUSER"];
            ViewBag.message = user.DecimalToString(user.balance);
            return View();
        }

        public ActionResult History()
        {
            UserAccount user = (UserAccount)Session["CURRENTUSER"];
            List<string> list = user.transactionsList;
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}