using BudgetManager2017.DataAccess;
using BudgetManager2017.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Fluentx.Mvc;

namespace BudgetManager2017.Controllers
{
    public class HomeController : Controller
    {
        
        
        public ActionResult Index()
        {
            ViewBag.Title = "Fetch Transactions";
            return View();
        }
        
        public ActionResult TimeSelector(string Command, Transaction dt)
        {
            if (Command == "search")
            {
                GenerateDB(dt);
                ViewBag.Transactions = DAL.Transactionslist;
                return View("TimeSelector");
            }
            else if (Command == "getJson")
            {
                GenerateDB(dt);
                
                return RedirectToAction("Json");
            }
            else if (Command == "JsonData")
            {
                return RedirectToAction("GeneralInformation");
            }
            else if (Command == "Description")
            {
                return RedirectToAction("Description");
            }
            return View("TimeSelector");
        }
        
        public ActionResult Json()
        {
            var jsonObj = DAL.Transactionslist;
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GeneralInformation()
        {
            Logging();

            DAL.Open();
            DAL.Select001();
            DAL.Close();
            var jsonObj = DAL.Transactions;
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        private void Logging()
        {

        }
        [HttpGet]
        public ActionResult Description()
        {
                List<string> Descriptions = new List<string>();
                DAL.Open();
                DAL.ReadDescription(ref Descriptions);
                DAL.Close();
                var jsonObj = Descriptions;
            var JsonObj = Json(jsonObj);
            PosDescript(JsonObj);

            return Json(JsonObj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PosDescript(object JsonObj)
        {
            Dictionary<string, object> jObj = new Dictionary<string, object>();
            jObj.Add("Description", JsonObj);

            return this.RedirectAndPost("localhost:3000/Description", jObj);
            //http://image-search9000.herokuapp.com/description
        }


            //return Json(jsonObj, JsonRequestBehavior.AllowGet);



        //List<string> Descriptions = new List<string>();
        //DAL.Open();
        //DAL.ReadDescription(ref Descriptions);
        //DAL.Close();
        //var jsonObj = Descriptions;



        private void GenerateDB(Transaction dt)
        {
            string dt001 = Request["dateTime001"];
            string dt002 = Request["dateTime002"];

            dt.Transactions.Add(dt001);
            dt.Transactions.Add(dt002);

            DAL.Open();
            DAL.Select(dt);
            DAL.Close();
        }

    }

}
          
