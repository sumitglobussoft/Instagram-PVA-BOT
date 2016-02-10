using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseLibFB.DataControler;


namespace BotGuruz.MantaLeadPro.SqliteControler
{
    public class QueryManager
    {
        #region Tb_Data
        public void AddData(string Site_name, string SiteUrl, string Status)
        {
            string InsertQuery = "insert into Url_tb(Site_name,SiteUrl,Date,Status) values('" + Site_name + "','" + SiteUrl + "','" + System.DateTime.Now.ToString() + "','" + Status + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "Url_tb");
        }

        public void InsertDataIntoAccountCreation(string EmailId,string password,string Username,string firstname,string lastname,string proxyAddress,string proxyport,string proxyusername,string proxyPassword)
        {
            string insertQuery="insert into CreatedAccountTable(EmailId,Password,Username,FirstName,LastName,ProxyAddress,ProxyPort,ProxyUsername,ProxyPassword) values ('"+EmailId+"','"+password+"','"+Username+"','"+firstname+"','"+lastname+"','"+proxyAddress+"','"+proxyport+"','"+proxyusername+"','"+proxyPassword+"') ";
            DataBaseHandler.PerformQuery(insertQuery, "CreatedAccountTable");
        }
        public void InsertDataIntoAccountVerification(string Username, string password,string proxyAddress, string proxyport, string proxyusername, string proxyPassword)
        {
            string insertQuery = "insert into AccountVerification(Username,Password,ProxyAddress,ProxyPort,ProxyUsername,ProxyPassword) values ('" + Username + "','" + password + "','" + proxyAddress + "','" + proxyport + "','" + proxyusername + "','" + proxyPassword + "') ";
            DataBaseHandler.PerformQuery(insertQuery, "AccountVerification");
        }
        public void AddDataGreen(string Site_name, string SiteUrl, string Status, string maincategory)
        {
            string InsertQuery = "insert into Url_tb(Site_name,SiteUrl,Date,Status,MainCategory) values('" + Site_name + "','" + SiteUrl + "','" + System.DateTime.Now.ToString() + "','" + Status + "','" + maincategory +  "')";
            DataBaseHandler.PerformQuery(InsertQuery, "Url_tb");
        }


        public void AddOldversionData(string Site_name, string SiteUrl, string Status,string tab)
        {
            string InsertQuery = "insert into Url_tb(Site_name,SiteUrl,Date,Status,tab) values('" + Site_name + "','" + SiteUrl + "','" + System.DateTime.Now.ToString() + "','" + Status + "','" + tab + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "Url_tb");
        }

        public void Updatedata(string Url,string status)
        {
            try
            {
                string InsertQuery = "Update Url_tb Set Status='" + status + "'where SiteUrl='" + Url + "'";
                DataBaseHandler.PerformQuery(InsertQuery, "Url_tb");
            }
            catch { }
        }

        public DataSet SelectData()
        {
            string SelectQuery = "select * from Tb_Data";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Url_tb");
            return ds;
        }

        public DataSet selectUrl(string Sitename)
        {
            string SelectQuery = "select SiteUrl,MainCategory from Url_tb where Site_name='" + Sitename + "'AND Status=0";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Url_tb");
            return ds;
        }

     
        public void DeleteDatabyCampid(string Sitename)
        {
            string SelectQuery = "Delete from Url_tb where Site_name='" + Sitename + "'";
            DataBaseHandler.PerformQuery(SelectQuery, "Url_tb");
        }

        

      #endregion

        

    }
}
