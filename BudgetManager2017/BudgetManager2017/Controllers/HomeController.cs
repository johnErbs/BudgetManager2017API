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
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.Http.Formatting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BudgetManager2017.Controllers
{
    public class HomeController : Controller
    {
       
        WebClient client = new WebClient();
        Dictionary<string, object> logger = new Dictionary<string, object>();
        Dictionary<string, object> jObj = new Dictionary<string, object>();
        static bool SingleDescr;

        private static string imageURL;
        public static string ImageURL
        {
            get { return imageURL; }
            set { imageURL = value; }
        }
        public bool completeJson { get; set; }


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
                completeJson = false;
                return RedirectToAction("Json");
            }
            else if (Command == "Info")
            {
                return RedirectToAction("GeneralInformation");
            }
            else if (Command == "Description")
            {
                return RedirectToAction("DescriptionAsync");
            }
            else if (Command == "Logger")
            {
                return RedirectToAction("Logging");
            }
            else if (Command == "dscr")
            {
                return RedirectToAction("socket");
            }
            else if (Command == "CreateTransactions")
            {
                return RedirectToAction("CreateTransactions");
            }
             
            return View("TimeSelector");
        }

        [HttpGet]
        public ActionResult CreateTransactions()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTransactions(Transaction transactionValue)
        {

            DAL.Descr = transactionValue.Description;
            DAL.Open();
            DAL.CreateTrans(transactionValue);
            DAL.Close();

            string imgUrl = "";
            UpdateIMG(imgUrl);

            return RedirectToAction("socket");
        }

      
        public ActionResult Socket()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Socket(DAL resp, string Command)
        //{
        //    string test = DAL.Content;
        //    //if (Command == "IMG")
        //    //{

        //    //    DAL.Open();
        //    //    DAL.selectLast();
        //    //    DAL.Close();
        //    //    DAL.Open();
        //    //    DAL.UpdateImg();
        //    //    DAL.Close();
        //    //}
        //    return View();
        //}

        public static void UpdateIMG(string imgUrl)
        {
            var factory = new ConnectionFactory() { HostName = "amqp://1doFhxuC:WGgk9kXy_wFIFEO0gwB_JiDuZm2-PrlO@black-ragwort-810.bigwig.lshift.net:10803/SDU53lDhKShK" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "Rapid", type: "direct");

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: "Rapid",
                                  routingKey: "ImageResponse");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    imgUrl = body.ToString();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

            DAL.Open();
            DAL.selectLast();
            DAL.Close();
            DAL.Open();
            DAL.UpdateImg(imgUrl);
            DAL.Close();


            //HttpListener listener = new HttpListener();
            //listener.Prefixes.Add("https://cancercancer.herokuapp.com/");
            //listener.Start();
            //HttpListenerContext ctx = listener.GetContext();
            //imgUrl = ctx.Request.HttpMethod + " " + ctx.Request.Url;
        }

     

        public ActionResult Json()
        {
            object jsonObj;

            if (completeJson==true)
            {
                jsonObj = DAL.Transactions;
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                jsonObj = DAL.Transactionslist;
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GeneralInformation()
        {
            //Logging();
            DAL.Open();
            DAL.Select001();
            DAL.Close();
            ViewBag.Transactions = DAL.Transactions;
            completeJson = true;
            return View();
        }
        

        //[HttpGet]
        //public ActionResult Logging()
        //{
        //    string logged = "User loged in";
        //    postLog(logged);
        //    return View();
        //}

        //[HttpPost]
        //public void postLog(string logged)
        //{
        //    string URI = "https://islogapi.herokuapp.com/";
        //    string action = $"action={logged}";
        //    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        //    string HtmlResult = client.UploadString(URI, action);
        //}

        //[HttpGet]
        //public async Task<ActionResult> DescriptionAsync()
        //{
        //    Queue<string> descriptId = new Queue<string>();
        //    List<string> descriptions = new List<string>();
        //    string content ="";
        //    string[] err = { "Application Error: ImageSearch limit reached.","This is an Error Code:403 Unauthorized access!", "Wait a day or two for reset." };
        //    string[] ok = {"Status Code 200 OK", "DataBase has been updated with new images, which matches descriptions"};
        //    string[] nullSearch = {"There was no descriptions to be searched for.", "Which means that there are no Transactions or database is emty.", "Please try again or create a new transaction in the database"};

        //    ReadDescript(ref descriptions, ref descriptId);

        //    DAL.Open();
        //    foreach (string item in descriptions)
        //    {
        //        string description = item;
        //        await getimageHelperAsync(description);
        //        content = imageURL;
        //        string logged = $"user searched for: {description}";
        //        postLog(logged);
        //        if (content == "Error Code:403")
        //        {
        //            return Json(err, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            string id = descriptId.Dequeue();
        //            DAL.Insert(content, id);
        //            logged = $"ImageUrl: {content}, uploaded to database.";
        //            postLog(logged);
        //        }
        //    }
        //    DAL.Close();

        //    if (content==null)
        //    {
        //      return Json(nullSearch, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(ok, JsonRequestBehavior.AllowGet);
        //}

        //private void ReadDescript(ref List<string> descriptions, ref Queue<string> descriptId)
        //{
           
        //    DAL.Open();
        //    DAL.ReadDescription(ref descriptions, ref descriptId);
        //    DAL.Close();
        //}

        //[HttpGet]
        //public static async Task<string> getimageHelperAsync(string description)
        //{
        //    var client001 = new HttpClient();
        //    //var client001 = new TcpClient();

        //    HttpResponseMessage response = await client001.GetAsync("http://image-search9000.herokuapp.com/Description?Beskrivelse=" + $"{description}");
        //    string result = await response.Content.ReadAsStringAsync();
        //    imageURL = result;
        //    return imageURL;
        //}

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
          
