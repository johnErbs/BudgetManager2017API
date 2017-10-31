using BudgetManager2017.DataAccess;
using BudgetManager2017.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace BudgetManager2017.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<Transaction> Get()
        {
            DAL.Open();
            DAL.Select001();
            DAL.Close();
            Transaction[] jsonObj = DAL.Transactions.ToArray();
            //return View("GeneralInformation");
            return jsonObj;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
