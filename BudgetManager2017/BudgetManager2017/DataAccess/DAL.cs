﻿using BudgetManager2017.Models;
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
        internal static void Select(Transaction transaction)
        {
            transactionslist.Clear();
            _ListIsCleared = true;

            SqlCommand readCmd = new SqlCommand("SELECT * FROM TRANSACTIONS WHERE DATE >= @dateTime001 AND DATE < @dateTime002", connection);
            //Params
            readCmd.Parameters.Add(CreateParam("@dateTime001", transaction.Transactions[0], SqlDbType.DateTime));
            readCmd.Parameters.Add(CreateParam("@dateTime002", transaction.Transactions[1], SqlDbType.DateTime));

            try
            {
                SqlDataReader dataReader = readCmd.ExecuteReader();

                while (dataReader.Read())
                {
                    if (_ListIsCleared)
                    {
                        //Addes der til liste fra 1. kollone i TableName.
                        int id = dataReader.GetInt32(0);
                        double amount = dataReader.GetDouble(1);
                        DateTime date = dataReader.GetDateTime(2);
                        string description = dataReader.GetString(3);
                        int subID = dataReader.GetInt32(4);

                       transactionslist.Add(new Transaction { TransactionID = id, Amount = amount, Date = date, Description = description, SubCat = subID });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
    