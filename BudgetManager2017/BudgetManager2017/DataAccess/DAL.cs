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
        private static bool _ListIsCleared { get; set; }
        private static List<Transaction> transactionslist = new List<Transaction>();
        public static List<Transaction> Transactionslist
        {
            get { return transactionslist; }
            set { transactionslist = value; }
        }
        private static List<Transaction> transactions = new List<Transaction>();
        public static List<Transaction> Transactions
        {
            get { return transactionslist; }
            set { transactionslist = value; }
        }
        private static string descr;

        public static string Descr
        {
            get { return descr; }
            set { descr = value; }
        }

        private static int id;

        public static int ID
        {
            get { return id; }
            set { id = value; }
        }
        private static string content;

        public static string Content
        {
            get { return content; }
            set { content = value; }
        }
        private string resp;

        public string Resp
        {
            get { return resp; }
            set { resp = value; }
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
        public static void selectLast()
        {
            SqlCommand LastCmd = new SqlCommand("SELECT TOP 1 * FROM Transactions ORDER BY TransactionID DESC", connection);

            SqlDataReader dr = LastCmd.ExecuteReader();

            try
            {
                while (dr.Read())
                {
                    ID = dr.GetInt32(0);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static void UpdateImg(string imgUrl)
        {
            Content = imgUrl;
            SqlCommand updatecmd = new SqlCommand("UPDATE Transactions SET Images = @content WHERE TransactionID=@id", connection);
            updatecmd.Parameters.Add(CreateParam("@id", ID, SqlDbType.Int));
            updatecmd.Parameters.Add(CreateParam("@content", Content, SqlDbType.VarChar));

            try
            {
                updatecmd.ExecuteNonQuery();
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
                        string img = dataReader.GetString(5);

                        transactionslist.Add(new Transaction { TransactionID = id, Amount = amount, Date = date, Description = description, SubCat = subID, Img = img });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void CreateTrans(Transaction transactionValue)
        {
            string noImg = "http://www.fotonova.dk/wp-content/themes/rebecca/images/noimage_2.gif";

            SqlCommand createCommand = new SqlCommand("INSERT INTO Transactions(Amount, Date, Description, SubCategoryID, Images) Values(@Amount, @Date, @Description, @SubCategoryID, @Images)", connection);
            createCommand.Parameters.Add(CreateParam("@Amount", transactionValue.Amount, SqlDbType.Float));
            createCommand.Parameters.Add(CreateParam("@Date", transactionValue.Date, SqlDbType.DateTime));
            createCommand.Parameters.Add(CreateParam("@Description", transactionValue.Description, SqlDbType.NVarChar));
            createCommand.Parameters.Add(CreateParam("@SubCategoryID", transactionValue.SubCat, SqlDbType.Int));
            createCommand.Parameters.Add(CreateParam("@Images", noImg, SqlDbType.VarChar));
            try
            {
                createCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        internal static void ReadDescription(ref List<string> descriptions, ref Queue<string> descriptId)
        {
            SqlCommand readCommand = new SqlCommand("SELECT * FROM TRANSACTIONS", connection);
            try
            {
                SqlDataReader dataReader = readCommand.ExecuteReader();

                while (dataReader.Read())
                {
                        string id = dataReader["transactionid"].ToString();
                        string descr = dataReader["description"].ToString();
                        descriptId.Enqueue(id);
                        descriptions.Add(descr);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public static void Insert(string content, string id)
        {
                SqlCommand updatecmd = new SqlCommand("UPDATE Transactions SET Images = @content WHERE TransactionID=@id", connection);
                updatecmd.Parameters.Add(CreateParam("@id", id, SqlDbType.Int));
                updatecmd.Parameters.Add(CreateParam("@content", content, SqlDbType.VarChar));

            try
            {
                updatecmd.ExecuteNonQuery();
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void Select001()
        {
            Transactions.Clear();

            SqlCommand readCmd = new SqlCommand("SELECT * FROM TRANSACTIONS", connection);

            try
            {
                SqlDataReader dataReader = readCmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string img;

                        //Addes der til liste fra 1. kollone i TableName.
                    int id = dataReader.GetInt32(0);
                    double amount = dataReader.GetDouble(1);
                    DateTime date = dataReader.GetDateTime(2);
                    string description = dataReader.GetString(3);
                    int subID = dataReader.GetInt32(4);
                    if (dataReader.GetString(5)==null)
                    {
                        img = "https://n6-img-fp.akamaized.net/free-icon/businessman_318-72886.jpg?size=338c&ext=jpg";
                    }
                    else
                    {
                        img = dataReader.GetString(5);
                    }
                  

                    Transactions.Add(new Transaction { TransactionID = id, Amount = amount, Date = date, Description = description, SubCat = subID, Img=img });
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
    
