using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ChurchData.Classes;



namespace Tripadvisor.Classes
{
    public class QueryManager
    {
        #region Tb_Data
        //public void AddData(string Category,string Sub_Categorie, string  HREF, string Name, string Address, string Image, string Latitude, string Longitude, string Rating, string Review, string Website1, string Neighborhoods, string PhoneNumber,string Reviews)
        //{
        //    string InsertQuery = "insert into Tb_Data(Category,Sub_Categories,HREF,Name,Address,Image,Latitude,Longitude, Rating, Reviews,Website1,Neighborhood,PhoneNumber,Reviews) values('" + Category + "','" + Sub_Categorie + "','" + HREF + "','" + Name + "','" + Address + "','" + Image + "','" + Latitude + "','" + Longitude + "','" + Rating + "','" + Review + "','" + Website1 + "','" + Neighborhoods + "','" + PhoneNumber + "','" + Reviews + "')";
        //    DataBaseHandler.PerformQuery(InsertQuery, "Tb_Data");
        //}

        public DataSet SelectData(string Url)
        {
            string SelectQuery = "select * from Tb_Church";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Tb_Church");
            return ds;
        }

        public void DeleteData()
        {
            string DeleteQuery = "delete from Beauty";
            DataBaseHandler.PerformQuery(DeleteQuery, "Beauty");
        }
        #endregion

        public void UpdateStatus()
        {
            string DeleteQuery = "Update  TheFork Set STATUS=0";
            DataBaseHandler.PerformQuery(DeleteQuery, "TheFork");
 
        }

        public void AddUrlMainUrl(string URL)
        {
            try
            {
                int Status = 0;
                string InsertQuery = "insert into TheFork(URL,STATUS) values('" + URL + "','" + Status + "')";
                DataBaseHandler.InsertQuery(InsertQuery, "TheFork");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                // AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error"+ex + ex.StackTrace, MainFrm.Error);
            }
        }

        #region Tb_Url
        public void AddUrl(string URL)
        {
            try
            {
                int Status = 0;
                string InsertQuery = "insert into Beauty(URL,Status) values('" + URL + "','" + Status + "')";
                DataBaseHandler.InsertQuery(InsertQuery, "Beauty");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                // AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error"+ex + ex.StackTrace, MainFrm.Error);
            }
        }

        public DataSet SelectUrl()
        {
            string SelectQuery = "select URL from TheFork where STATUS=0";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "TheFork");
            return ds;
        }
        public DataSet SelectMainUrl()
        {
            string SelectQuery = "select Url from MainUrl where Status=0";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "MainUrl");
            return ds;
        }

        public DataSet SelectEmails()
        {
            string SelectQuery = "select * from DineoutShopData";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "DineoutShopData");
            return ds;
        }

        public DataSet SelectAllData()
        {
            string SelectQuery = "select * from DineoutShopData";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "DineoutShopData");
            return ds;
        }

        public void UpdateUrlstatus()
        {
            try
            {
                string UpdateQuery = "update RestaurantUrl set Status='" + "0" + "'";
                DataBaseHandler.UpdateQuery(UpdateQuery, "RestaurantUrl");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error" +ex+ ex.StackTrace, MainFrm.Error);
            }

        }
        public void UpdateMainUrlstatus(string Url)
        {
            try
            {
                string UpdateQuery = "update MainUrl set Status='" + "1" + "' where Url= '" + Url + "'";
                DataBaseHandler.UpdateQuery(UpdateQuery, "MainUrl");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error" +ex+ ex.StackTrace, MainFrm.Error);
            }

        }

        public void DeletePage()
        {
            try
            {
                string DeleteQuery = "delete from DineoutShopData";
                DataBaseHandler.PerformQuery(DeleteQuery, "DineoutShopData");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error"+ex + ex.StackTrace, MainFrm.Error);
            }
        }
        public void DeleteAllData()
        {
            try
            {
                string DeleteQuery = "delete from TheFork";
                DataBaseHandler.PerformQuery(DeleteQuery, "TheFork");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error"+ex + ex.StackTrace, MainFrm.Error);
            }
        }

        public void UpdateUrlstatus(string url)
        {
            try
            {
                string UpdateQuery = "update TheFork set STATUS='" + "1" + "' where URL= '" + url + "'";
                DataBaseHandler.UpdateQuery(UpdateQuery, "TheFork");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error" +ex+ ex.StackTrace, MainFrm.Error);
            }
        }

        public void InsertData(string str_ShopUrl, string str_ShopId, string str_ShopName, string str_ShopAddress, string str_ShopArea, string str_ShopCity, string str_ShopSubCategory, string str_ShopTeliphone, string str_ShopFax, string str_ShopEmail, string str_ShopBusinessHours, string str_ShopLatitude, string str_ShopLongitude, string str_ShopImageLink,string str_ShopWebsite)
        {
            try
            {
                string InsertQuery = "insert into DineoutShopData(ShopUrl, ShopId, ShopName, ShopAddress, ShopArea, ShopCity, ShopSubCategory, ShopTelephone, ShopFax, ShopEmail, ShopBusinessHours, ShopLatitude, ShopLongitude, ShopImageLink,ShopWebsite) values('" + str_ShopUrl + "','" + str_ShopId + "','" + str_ShopName + "','" + str_ShopAddress + "','" + str_ShopArea + "','" + str_ShopCity + "','" + str_ShopSubCategory + "','" + str_ShopTeliphone + "','" + str_ShopFax + "','" + str_ShopEmail + "','" + str_ShopBusinessHours + "','" + str_ShopLatitude + "','" + str_ShopLongitude + "','" + str_ShopImageLink +  "','"+str_ShopWebsite+"')";
                DataBaseHandler.InsertQuery(InsertQuery, "DineoutShopData");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                // AlibabaScaper.GlobusFileHelper.AppendStringToTextfileNewLine("InsertQuery error"+ex + ex.StackTrace, MainFrm.Error);
            }
        }



        #endregion

        //internal void AddUrl(string str)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
