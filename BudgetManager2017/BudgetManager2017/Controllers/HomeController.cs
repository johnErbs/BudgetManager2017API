﻿using BudgetManager2017.DataAccess;
using BudgetManager2017.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Fluentx.Mvc;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.Http.Formatting;

namespace BudgetManager2017.Controllers
{
    public class HomeController : Controller
    {
        WebClient client = new WebClient();
        Dictionary<string, object> logger = new Dictionary<string, object>();
        Dictionary<string, object> jObj = new Dictionary<string, object>();

        private static string imageURL;
        public static string ImageURL
        {
            get { return imageURL; }
            set { imageURL = value; }
        }

        public static object Beskrivelse { get; private set; }

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
            string action = "action=user logged in";
            string HtmlResult = client.UploadString(URI, action);
        }

        [HttpGet]
        public ActionResult Description()
        {
            Queue<string> descriptId = new Queue<string>();
            List<string> descriptions = new List<string>();

            string content ="";
            string err = "Application Error: ImageSearch limit has reached.";
            string ok = $"Got the imageSearch from words: {descriptions}";

            DAL.Open();
            DAL.ReadDescription(ref descriptions, ref descriptId);
            getimageHelperAsync(content);
            //foreach (string item in descriptions)
            //{
            //    string description = item;
            //    getimageHelperAsync(description);
            //    content = imageURL;
            //    string id = descriptId.Dequeue();
            //    DAL.Insert(content, id);
            //}
            DAL.Close();

            if (content==null)
            {
              return Json(err, JsonRequestBehavior.AllowGet);
            }
            return Json(ok, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public static async Task<string> getimageHelperAsync(string description)
        {
            var client001 = new HttpClient();
            //var client001 = new TcpClient();

            HttpResponseMessage response = await client001.GetAsync("http://image-search9000.herokuapp.com/Description?Beskrivelse=" + $"Hest");
            string result = await response.Content.ReadAsStringAsync();
            imageURL = result;
            return imageURL;
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
          
