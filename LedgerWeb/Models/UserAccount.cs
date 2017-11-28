using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LedgerWeb.Models
{
    public class UserAccount
    {
        public string username { get; set; }
        public string password { get; set; }
        public decimal balance { get; set; }
        public List<string> transactionsList { get; set; }

        public UserAccount(string name, string pswd)
        {
            this.username = name;
            this.password = pswd;
            this.balance = 0;
            this.transactionsList = new List<string>();
        }

        public string DecimalToString(decimal num)
        {
            return ("$" + num.ToString("#,##0.00"));
        }

        public void Report(string msg)
        {
            // Simply format strings to show necessary information and combine them together
            string transaction = "Transaction: " + msg;
            string date = "Date: " + DateTime.Now.ToString("HH:mm:ss MM-dd-yyyy");
            string curBalance = "Balance: " + DecimalToString(this.balance);
            string report = transaction + "          " +
                date + "         " + curBalance;

            this.transactionsList.Add(report);
        }
    }
}