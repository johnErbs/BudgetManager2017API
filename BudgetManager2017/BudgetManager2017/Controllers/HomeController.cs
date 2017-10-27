using BudgetManager2017.DataAccess;
using BudgetManager2017.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetManager2017.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Fetch Transactions";
            Transaction transaction = new Transaction();
            int transactonID, subcatagoryID;
            double amount;
            DateTime Date;
            String Description;

            transactonID = transaction.TransactionID;
            amount = transaction.Amount;
            Date = transaction.Date;
            Description = transaction.Description;
            subcatagoryID = transaction.SubCat;
            DAL.Open();
            DAL.Select();
            ViewBag.Transactions = DAL.Transactionslist;
            DAL.Close();

            return View();
        }
    }
}
