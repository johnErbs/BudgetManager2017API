using BudgetManager2017.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BudgetManager2017.DataAccess
{
    public class DAL
    {
        static public SqlConnection connection = null;
        private static List<Transaction> transactionslist = new List<Transaction>();
        private static bool _ListIsCleared { get; set; }
        public static List<Transaction> Transactionslist
        {
            get { return transactionslist; }
            set { transactionslist = value; }
        }

        public static void Open()
        {
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TransactionDB"].ConnectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Close()
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void Select()
        {
            SqlCommand command1 = new SqlCommand("SELECT * FROM Transactions", connection);
            try
            {
                using (SqlDataReader DR = command1.ExecuteReader())
                {
                    while (DR.Read())     //Mens DR Læser...
                    {
                        //Her addes der til liste fra 1. kollone i TableName.
                        int id = DR.GetInt32(0);
                        double amount = DR.GetDouble(1);
                        DateTime date = DR.GetDateTime(2);
                        string description = DR.GetString(3);
                        int subID = DR.GetInt32(4);


                        for (int i = 0; i < transactionslist.Count; i++)
                        {

                            if (transactionslist[i].TransactionID == id)
                            {
                                transactionslist.Clear();
                                _ListIsCleared = true;
                            }
                        }
                        if (transactionslist.Count == 0)
                        {
                            _ListIsCleared = true;
                        }
                        if (_ListIsCleared)
                        {
                            transactionslist.Add(new Transaction { TransactionID = id, Amount = amount, Date = date, Description = description, SubCat = subID });
                        }
                    }

                    _ListIsCleared = false;
                }
            }

            catch (SqlException ex)
            {

            }
        }
        public static SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }
    }
}
    
