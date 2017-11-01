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
        WebClient client = new WebClient();
        Dictionary<string, object> logger = new Dictionary<string, object>();
        Dictionary<string, object> jObj = new Dictionary<string, object>();

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
            else if (Command == "Logger")
            {
                return RedirectToAction("Logging");
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
        
        [HttpGet]
        public ActionResult Logging()
        {
            string logged = "User loged in";
            postLog(logged);
            return View();

        }
        [HttpPost]
        public void postLog(string logged)
        {
            string URI = "https://islogapi.herokuapp.com/";
            string Action = "Action=user logged in";
            //client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string HtmlResult = client.UploadString(URI, Action);
        }

        

[HttpGet]
        public ActionResult Description()
        {
            Dictionary<string, string> descriptId = new Dictionary<string, string>();
            List<string> descriptions = new List<string>();

            DAL.Open();
            DAL.ReadDescription(ref descriptions, ref descriptId);
            DAL.Close();
            var jsonObj = descriptions;//Obs jsonObject kan asignes til enten [descriptId] for key value pair eller til [Descriptions] for en enkelt beskrivelse
            //string concat = String.Join(" ", Descriptions.ToArray());
            PostDescript(descriptions);
            string content="";
            JsonStringBody(content);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void PostDescript(List<string> descriptions)
        {
            string URI = "http://image-search9000.herokuapp.com/description";
            //client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

            foreach (string description in descriptions)
            {
                string Action = description;
                string HtmlResult = client.UploadString(URI, Action);
            }
        }
        
        [HttpGet]
        public string JsonStringBody(string content)
        {
            
            content = jObj.ToString();
            return content;
        }




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
          
