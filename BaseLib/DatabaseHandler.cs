using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;


namespace BaseLib
{
    public class DataBaseHandler
    {
       // public static string CONstr = "Data Source=" + Application.StartupPath + "\\Booking\\DB_BookingScraper.db+ ";//Version=3;";
        public static string CONstr = "Data Source=" + "C:\\InstagramPVABot\\Data\\InstagramPVABot.db" + ";Version=3;";

        public static DataSet SelectQuery(string query, string tablename)
        {
            try
            {
                DataSet DS = new DataSet();
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    AD.Fill(DS, tablename);

                }
                return DS;
            }
            catch
            {
                return new DataSet();
            }
        }

        public static void InsertQuery(string query, string tablename)
        {
            try
            {
            using (SQLiteConnection CON = new SQLiteConnection(CONstr))
            {
                SQLiteCommand CMD = new SQLiteCommand(query, CON);
                SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                DataSet DS = new DataSet();
                AD.Fill(DS, tablename);
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error=" +ex+ ex.StackTrace, MainFrm.Error);
            }
        }

        public static void DeleteQuery(string query, string tablename)
        {
            try
            {
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("DeleteQuery error=" + ex + ex.StackTrace, MainFrm.Error);
            }
        }

        public static void UpdateQuery(string query, string tablename)
        {
            try
            {
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
               // AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine(" UpdateQuery error=" +ex+ ex.StackTrace, MainFrm.Error);
            }
        }
 
        /// <summary>
        /// This method is use for find data form sqlite table 
        /// </summary>
        /// <param name="query">Sqlite query</param>
        /// <param name="tablename">Name of Table</param>
  

        public static void PerformQuery(string query, string tablename)
        {
            try
            {
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("PerformQuery error"+ex + ex.StackTrace, MainFrm.Error);
            }
        }

    }
}
