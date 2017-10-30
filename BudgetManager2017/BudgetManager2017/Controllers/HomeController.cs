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
        
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Fetch Transactions";
            return View();
        }
        [HttpPost]
        public ActionResult TimeSelector(Transaction dt)
        {
            string dt001 = Request["dateTime001"];
            string dt002 = Request["dateTime002"];

            dt.Transactions.Add(dt001);
            dt.Transactions.Add(dt002);

            DAL.Open();
            DAL.Select(dt);
            ViewBag.Transactions = DAL.Transactionslist;
            DAL.Close();

            return View("TimeSelector");
        }
        
    }

}
