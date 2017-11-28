using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetManager2017.Models
{
    public class Organization
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


    }
}