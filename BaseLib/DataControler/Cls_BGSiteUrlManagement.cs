using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;
using System.Text;
using System.Data;
using System.Threading;

namespace BotGuruz.MantaLeadPro.DataControler
{
    class Cls_BGSiteUrlManagement
    {
        public class SuperPagesStateCat
        {
            public string City_SiteName { get; set; }
            public string City_Url { get; set; }
            public string City_Cat { get; set; }
            public string City_StateCat { get; set; }
            public string City_CityCat { get; set; }
            public string City_Status { get; set; }
            public DateTime city_Date { get; set; }
        
        }

        MySqlConnection Conn = new MySqlConnection(BG_Db_Class.getConnectionString());



        //***************************************************Australia***************************************
        //Australia Yellow Pages Category

        #region --For InsertSiteCatUrlTableAusYp
        public int InsertSiteCatUrlTableAusYp(string SiteName, string SiteUrl, string Category, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {
                   if (conn.State != ConnectionState.Open)
                       try
                       {
                           conn.Open();
                       }
                       catch (MySqlException ex)
                       {
                           throw (ex);
                       }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_AusYPCatUrl(SiteName,Url,Category,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSiteCatUrlAusYp --
        public IEnumerable<dynamic> GetSiteCatUrlAusYp()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
              
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_AusYPCatUrl WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateCatUrlAusYp
        public int UpdateCatUrlAusYp(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_AusYPCatUrl SET Status = '" + Status + "' ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For InsertSiteSubCatUrlTableAusYp
        public int InsertSiteSubCatUrlTableAusYp(string SiteName, string SiteUrl, string Category, string SubCategory, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_AusYPSubCatUrl(SiteName,Url,Category,SubCategory,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@SubCategory,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, SubCategory = SubCategory, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion
         
        #region --For GetSiteSubCatUrlAusYp --
        public IEnumerable<dynamic> GetSiteSubCatUrlAusYp(string Cat)
        {
           

            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT Url,Category,SubCategory FROM Bus_Data_AusYPSubCatUrl WHERE Status = 0 and Category = '" + Cat + "'";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSubCatUrlAusYp
        public int UpdateSubCatUrlAusYp(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_AusYPSubCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //**************************************************Canada*********************************************************//
        //Canada Yellow Pages Category
        #region --For InsertCatStatusTableCan
        public int InsertCatStatusTableCan(string SiteName, string SiteUrl, string Cat, string Status,DateTime CrawlDate)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {
                   if (conn.State != ConnectionState.Open)
                       try
                       {
                           conn.Open();
                       }
                       catch (MySqlException ex)
                       {
                           throw (ex);
                       }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_CanadaYPCatUrl(SiteName,Url,Category,Status,date) VALUES(@SiteName,@SiteUrl,@Cat,@Status,@date)", new { SiteName = SiteName, SiteUrl = SiteUrl, Cat = Cat, Status = Status, date = CrawlDate, });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For BulkInsertSpStateCatTable
        public void BulkInsertSpStateCatTable(System.Data.DataTable table)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    var lst = new List<SuperPagesStateCat>();
                    foreach (System.Data.DataRow item in table.Rows)
                    {

                        lst.Add(
                            new SuperPagesStateCat()
                            {
                                City_SiteName = (item.ItemArray[0].ToString()),
                                City_Url = (item.ItemArray[1].ToString()),
                                City_Cat = (item.ItemArray[2].ToString()),
                                City_StateCat = (item.ItemArray[3].ToString()),
                                City_CityCat = (item.ItemArray[4].ToString()),
                                city_Date = Convert.ToDateTime(item.ItemArray[5].ToString()),
                                City_Status = (item.ItemArray[6].ToString()),
                                
                               

                            });
                    }

                    MySqlTransaction trans = conn.BeginTransaction();


                    conn.Execute(@"INSERT INTO Bus_Data_SuperPagesCityCatUrl(SiteName,Url,Category,StateCategory,CityCategory,Status,Date)"
                               + " VALUES(@City_SiteName,@City_Url,@City_Cat,@City_StateCat,@City_CityCat,@City_Status,@city_Date)", lst);

                    trans.Commit();
                    lst.Clear();
                    Log("---------------------------------------BULK INSERTED SUCCESSFULLY----------------------------------------");
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For GetCatUrlCan --
        public IEnumerable<dynamic> GetCatUrlCan()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_CanadaYPCatUrl WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateCatStatusTableCan
        public int UpdateCatStatusTableCan(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_CanadaYPCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //Canada Yellow Pages SubCategory
        #region --For InsertSubCatStatusTableCan
        public int InsertSubCatStatusTableCan(string SiteName, string SiteUrl, string Cat, string SubCat,string Status, DateTime CrawlDate)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {
                   if (conn.State != ConnectionState.Open)
                       try
                       {
                           conn.Open();
                       }
                       catch (MySqlException ex)
                       {
                           throw (ex);
                       }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_CanadaYPSubCatUrl(SiteName,Url,Category,SubCategory,Status,date) VALUES(@SiteName,@SiteUrl,@Cat,@SubCat,@Status,@date)", new { SiteName = SiteName, SiteUrl = SiteUrl, Cat = Cat, SubCat = SubCat, Status = Status, date = CrawlDate });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSubCatUrlCan --
        public IEnumerable<dynamic> GetSubCatUrlCan()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {
                   if (conn.State != ConnectionState.Open)
                       try
                       {
                           conn.Open();
                       }
                       catch (MySqlException ex)
                       {
                           throw (ex);
                       }

                    string strquery = "SELECT * FROM Bus_Data_CanadaYPSubCatUrl WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSubCatStatusTableCan
        public int UpdateSubCatStatusTableCan(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {
                   if (conn.State != ConnectionState.Open)
                       try
                       {
                           conn.Open();
                       }
                       catch (MySqlException ex)
                       {
                           throw (ex);
                       }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_CanadaYPSubCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //Canada Yellow Pages BusCategory
        #region --For InsertBusCatStatusTableCan
        public int InsertBusCatStatusTableCan(string SiteName, string SiteUrl, string Cat, string SubCat, string BusCat,string Status, DateTime CrawlDate)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_CanadaYPBusTypeCatUrl(SiteName,Url,Category,SubCategory,BusCategory,Status,date) VALUES(@SiteName,@SiteUrl,@Cat,@SubCat,@BusCat,@Status,@date)", new { SiteName = SiteName, SiteUrl = SiteUrl, Cat = Cat, SubCat = SubCat, BusCat = BusCat, Status = Status, date = CrawlDate });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetBusCatUrlCan --
        public IEnumerable<dynamic> GetBusCatUrlCan()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_CanadaYPBusTypeCatUrl WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateBusCatStatusTableCan
        public int UpdateBusCatStatusTableCan(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_CanadaYPBusTypeCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //Canada Yellow Pages StateCategory
        #region --For InsertStateCatStatusTableCan
        public int InsertStateCatStatusTableCan(string SiteName, string SiteUrl, string Cat, string SubCat, string BusCat, string StateCat,string Status, DateTime CrawlDate)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_CanadaYPStateCatUrl(SiteName,Url,Category,SubCategory,BusCategory,StateCategory,Status,date) VALUES(@SiteName,@SiteUrl,@Cat,@SubCat,@BusCat,@StateCat,@Status,@date)", new { SiteName = SiteName, SiteUrl = SiteUrl, Cat = Cat, SubCat = SubCat, BusCat = BusCat, StateCat = StateCat, Status = Status, date = CrawlDate });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetStateCatUrlCan --
        public IEnumerable<dynamic> GetStateCatUrlCan()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_CanadaYPStateCatUrl WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateStateCatStatusTableCan
        public int UpdateStateCatStatusTableCan(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_CanadaYPStateCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //Canada Yellow Pages CityCategory
        #region --For InsertCityUrlStatusTableCan
        #region Insert Threads Controllers

        static int count_InsertThreads = 0;
        static readonly object lockr_InsertThreads = new object();

        #endregion
        public int InsertCityUrlStatusTableCan(string SiteName, string SiteUrl, string Cat, string SubCat, string BusCat, string StateCat, string CityCat,string Status, DateTime CrawlDate)
        {
            
            try
            {
                lock (lockr_InsertThreads)
                {
                    count_InsertThreads++;
                    if (count_InsertThreads > 5)
                    {
                        Monitor.Wait(lockr_InsertThreads);
                    }
                }

                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_CanadaYPCityCatUrl2(SiteName,Url,Category,SubCategory,BusCategory,StateCategory,CityCategory,Status,date) VALUES(@SiteName,@SiteUrl,@Cat,@SubCat,@BusCat,@StateCat,@CityCat,@Status,@date)", new { SiteName = SiteName, SiteUrl = SiteUrl, Cat = Cat, SubCat = SubCat, BusCat = BusCat, StateCat = StateCat, CityCat = CityCat, Status = Status, date = CrawlDate, });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                lock (lockr_InsertThreads)
                {
                    count_InsertThreads--;

                    Monitor.Pulse(lockr_InsertThreads);
                }
            }

        }
        #endregion

        #region --For GetSCityCatUrlCan --
        public IEnumerable<dynamic> GetSCityCatUrlCan()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_CanadaYPCityCatUrl WHERE status = 0 limit 50000"; ;
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateCityCatStatusTableCan
        public int UpdateCityCatStatusTableCan(string SiteUrl, DateTime CrawlDate, int Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_CanadaYPCityCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //Canada Yellow Pages FinalUrl
        #region --For InsertFinalUrlStatusTableCan
        public int InsertFinalUrlStatusTableCan(string SiteName, string SiteUrl, string Cat, string SubCat, string BusCat, string StateCat, string CityCat, string Status, DateTime CrawlDate)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_CanadaYFinalUrl(SiteName,FinalUrl,Category,SubCategory,BusCategory,StateCategory,CityCategory,Status,date) VALUES(@SiteName,@SiteUrl,@Cat,@SubCat,@BusCat,@StateCat,@CityCat,@Status,@date)", new { SiteName = SiteName, SiteUrl = SiteUrl, Cat = Cat, SubCat = SubCat, BusCat = BusCat, StateCat = StateCat, CityCat = CityCat, Status = Status, date = CrawlDate, });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSFinalCatUrlCan --
        public IEnumerable<dynamic> GetSFinalCatUrlCan()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                 {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_CanadaYFinalUrl WHERE Status = 0 LIMIT 500000";
                    //string strquery = "SELECT * FROM Bus_Data_CanadaYFinalUrl WHERE Status = 0 order by id desc LIMIT 0,200000";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateFinalCatStatusTableCan
        public int UpdateFinalCatStatusTableCan(string SiteUrl, DateTime CrawlDate, int Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_CanadaYFinalUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where FinalUrl =  '" + SiteUrl.Replace("'", "''") + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //************************************************** SUPER PAGES **********************************************************//
        //SuperPages SuperPagesCategoryUrl
        #region --For InsertSuperPagesCategoryUrl
        public int InsertSuperPagesCategoryUrl(string SiteName, string Url, string Category, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_SuperPagesCatUrl(SiteName,Url,Category,Date,Status) VALUES(@SiteName,@Url,@Category,@date,@Status)", new { SiteName = SiteName, Url = Url, Category = Category, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion
       
        #region --For GetSuperPagesCategoryUrl --
        public IEnumerable<dynamic> GetSuperPagesCategoryUrl()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_SuperPagesCatUrl WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
     
        #region --For UpdateSuperPagesCategoryUrl
        public int UpdateSuperPagesCategoryUrl(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_SuperPagesCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //SuperPages SuperPagesStateCategoryUrl
        #region --For InsertSuperPagesStateCategoryUrl
        public int InsertSuperPagesStateCategoryUrl(string SiteName, string Url, string Category, string StateCategory,DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {
                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_SuperPageStateCatUrl(SiteName,Url,Category,StateCategory,Date,Status) VALUES(@SiteName,@Url,@Category,@StateCategory,@date,@Status)", new { SiteName = SiteName, Url = Url, Category = Category, StateCategory = StateCategory, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSuperPagesStateCategoryUrl --
        public IEnumerable<dynamic> GetSuperPagesStateCategoryUrl()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_SuperPageStateCatUrl WHERE Status = 0 limit 50000";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSuperPagesStateCategoryUrl
        public int UpdateSuperPagesStateCategoryUrl(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_SuperPageStateCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //SuperPagesCityCategoryUrl
        #region --For InsertSuperPagesCityCategoryUrl
        public int InsertSuperPagesCityCategoryUrl(string SiteName, string Url, string Category, string StateCategory, string CityCategory, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_SuperPagesCityCatUrl(SiteName,Url,Category,StateCategory,CityCategory,Date,Status) VALUES(@SiteName,@Url,@Category,@StateCategory,@CityCategory,@date,@Status)", new { SiteName = SiteName, Url = Url, Category = Category, StateCategory = StateCategory,CityCategory = CityCategory, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSuperPagesCityCategoryUrl --
        public IEnumerable<dynamic> GetSuperPagesCityCategoryUrl()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }
                    string strquery = "SELECT * FROM Bus_Data_SuperPagesCityCatUrl where status = 0 Limit 50000";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSuperPagesCityCategoryUrl
        public int UpdateSuperPagesCityCategoryUrl(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_SuperPagesCityCatUrl SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //InsertSiteUrlStatusTableSp
        #region --For InsertSiteUrlStatusTableSp
        public int InsertSiteUrlStatusTableSP(string SiteName, string SiteUrl, string Category,string SubCategory,string SubCategory2, DateTime CrawlDate, int Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_SuperPagesYP_UrlStatus(SiteName,SiteUrl,Category,SubCategory,SubCategory2,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@SubCategory,@SubCategory2,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, SubCategory = SubCategory, SubCategory2 = SubCategory2, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSuperPagesFinalUrl --
        public IEnumerable<dynamic> GetSuperPagesFinalUrl()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT SiteUrl,Category,SubCategory,SubCategory2 FROM Bus_Data_SuperPagesYP_UrlStatus WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSuperPagesFinalUrl
        public int UpdateSuperPagesFinalUrl(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_SuperPagesYP_UrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where SiteUrl =  '" + SiteUrl.Replace("'", "''") + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //***********************************************************UAE YellowPages********************************************//
        #region --For GetSiteCatUrlUAEYp --
        public IEnumerable<dynamic> GetSiteCatUrlUAEYp()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT Category FROM Bus_Data_UAEYP_CatUrlStatus WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSiteCatUrlStatusTableUAEYp
        public int UpdateSiteCatUrlStatusTableUAEYp(string cat, int Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {
                   if (conn.State != ConnectionState.Open)
                       try
                       {
                           conn.Open();
                       }
                       catch (MySqlException ex)
                       {
                           throw (ex);
                       }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_UAEYP_CatUrlStatus SET Status = " + Status + " where Category =  '" + cat + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion


        #region --For InsertSiteUrlSubCatStatusTableUAEYP
        public int InsertSiteUrlSubCatStatusTableUAEYP(string SiteName, string SiteUrl, string Category, DateTime CrawlDate, int Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {
                   if (conn.State != ConnectionState.Open)
                       try
                       {
                           conn.Open();
                       }
                       catch (MySqlException ex)
                       {
                           throw (ex);
                       }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_UAEYP_SubCatUrlStatus(SiteName,SiteUrl,Category,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSiteSubCatUrlUAEYp --
        public IEnumerable<dynamic> GetSiteSubCatUrlUAEYp()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT * FROM Bus_Data_UAEYP_SubCatUrlStatus WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                  
                }

            }
            catch (Exception ex)
            {
               return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSiteUrlSubCatStatusTableUAEYp
        public int UpdateSiteUrlSubCatStatusTableUAEYp(string SiteUrl, DateTime CrawlDate, int Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {
                    conn.Open();
                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_UAEYP_SubCatUrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where SiteUrl =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion


        #region --For InsertSiteUrlStatusTableUAEYp
        public int InsertSiteUrlStatusTableUAEYP(string SiteName, string SiteUrl, string Category, DateTime CrawlDate, int Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_UAEYP_UrlStatus(SiteName,SiteUrl,Category,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category,date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSiteUrlUAEYp --
        public IEnumerable<dynamic> GetSiteUrlUAEYp()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                try
                {
                    {

                        if (conn.State != ConnectionState.Open)
                            try
                            {
                                conn.Open();
                            }
                            catch (MySqlException ex)
                            {
                                throw (ex);
                            }

                        string strquery = "SELECT * FROM Bus_Data_UAEYP_UrlStatus WHERE Status = 0";
                        IEnumerable<dynamic> result = conn.Query(strquery);
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSiteUrlStatusTableUAEYp
        public int UpdateSiteUrlStatusTableUAEYp(string SiteUrl, DateTime CrawlDate, int Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_UAEYP_UrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where SiteUrl =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //***********************************************************Swiss Local********************************************//
        #region --For InsertSiteCatUrlTableSwissDir
        public int InsertSiteCatUrlTableSwissDir(string SiteName, string SiteUrl, string Category, string SubCategory, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
               {

                   if (conn.State != ConnectionState.Open)
                       try
                       {
                           conn.Open();
                       }
                       catch (MySqlException ex)
                       {
                           throw (ex);
                       }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_SwissCatUrlStatus(SiteName,Url,Category,SubCategory,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@SubCategory,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, SubCategory = SubCategory, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSiteCatUrlTableSwissDir --
        public IEnumerable<dynamic> GetSiteCatUrlTableSwissDir()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT Url,Category,SubCategory FROM Bus_Data_SwissCatUrlStatus WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSiteCatUrlTableSwissDir
        public int UpdateSiteCatUrlTableSwissDir(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_SwissCatUrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For InsertSiteSubCatUrlTableSwissDir
        public int InsertSiteSubCatUrlTableSwissDir(string SiteName, string SiteUrl, string Category, string SubCategory, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_SwissSubCatUrlStatus(SiteName,Url,Category,SubCategory,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@SubCategory,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, SubCategory = SubCategory, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetSiteSubCatUrlTableSwissDir --
        public IEnumerable<dynamic> GetSiteSubCatUrlTableSwissDir(string cat)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT Url,Category,SubCategory FROM Bus_Data_SwissSubCatUrlStatus WHERE Status = 0 and Category = '" + cat +"'";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateSiteSubCatUrlTableSwissDir
        public int UpdateSiteSubCatUrlTableSwissDir(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_SwissSubCatUrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion


        //***********************************************************Brabys Scraper********************************************//
        #region --For InsertBrabysCatUrlStatus
        public int InsertBrabysCatUrlStatus(string SiteName, string SiteUrl, string Category, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_BrabysCatUrlStatus(SiteName,Url,Category,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetBrabysCatUrlStatus --
        public IEnumerable<dynamic> GetBrabysCatUrlStatus()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    string strquery = "SELECT Url,Category FROM Bus_Data_BrabysCatUrlStatus WHERE Status = 0";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateBrabysCatUrlStatus
        public int UpdateBrabysCatUrlStatus(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_BrabysCatUrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        //***********************************************************Brabys Scraper********************************************//
        #region --For InsertBrabysFinalUrlStatus
        public int InsertBrabysFinalUrlStatus(string SiteName, string SiteUrl, string Category, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_BrabysfinalUrlStatus(SiteName,Url,Category,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, date = CrawlDate, Status = Status });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region --For GetBrabysFinalUrlStatus --
        public IEnumerable<dynamic> GetBrabysFinalUrlStatus(string Cat)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch { }

                    string strquery = "SELECT Url,Category FROM Bus_Data_BrabysfinalUrlStatus WHERE Status = 0 and Category = '" + Cat + "'";
                    IEnumerable<dynamic> result = conn.Query(strquery);
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region --For UpdateBrabysFinalUrlStatus
        public int UpdateBrabysFinalUrlStatus(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
            try
            {
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            throw (ex);
                        }

                    int rowAffected = conn.Execute(@"UPDATE Bus_Data_BrabysfinalUrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion


        //***********************************************************Yellow pages********************************************//
        #region --For InsertSiteCatUrlTableYPUsa
        public int InsertSiteCatUrlTableYPUsa(string SiteName, string SiteUrl, string Category, string SubCategory, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                try
                {
                    {

                        if (conn.State != ConnectionState.Open)
                            try
                            {
                                conn.Open();
                            }
                            catch (MySqlException ex)
                            {
                                throw (ex);
                            }

                        int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_YPUsaCatUrlStatus(SiteName,Url,Category,SubCategory,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@SubCategory,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, SubCategory = SubCategory, date = CrawlDate, Status = Status });
                        return rowAffected;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    conn.Close();
                }

        }
        #endregion

        #region --For GetSiteCatUrlTableYPUsa --
        public IEnumerable<dynamic> GetSiteCatUrlTableYPUsa()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                try
                {
                    {

                        if (conn.State != ConnectionState.Open)
                            try
                            {
                                conn.Open();
                            }
                            catch (MySqlException ex)
                            {
                                throw (ex);
                            }

                        string strquery = "SELECT Url,Category,SubCategory FROM Bus_Data_YPUsaCatUrlStatus WHERE Status = 0";
                        IEnumerable<dynamic> result = conn.Query(strquery);
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    conn.Close();
                }
        }
        #endregion

        #region --For UpdateSiteCatUrlTableYPUsa
        public int UpdateSiteCatUrlTableYPUsa(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                try
                {
                    {

                        if (conn.State != ConnectionState.Open)
                            try
                            {
                                conn.Open();
                            }
                            catch (MySqlException ex)
                            {
                                throw (ex);
                            }

                        int rowAffected = conn.Execute(@"UPDATE Bus_Data_YPUsaCatUrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                        return rowAffected;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    conn.Close();
                }

        }
        #endregion

        #region --For InsertSiteFinalUrlTableYPUsa
        public int InsertSiteFinalUrlTableYPUsa(string SiteName, string SiteUrl, string Category, string SubCategory, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                try
                {
                    {

                        if (conn.State != ConnectionState.Open)
                            try
                            {
                                conn.Open();
                            }
                            catch (MySqlException ex)
                            {
                                throw (ex);
                            }

                        int rowAffected = conn.Execute(@"INSERT INTO Bus_Data_YPUsaFinalUrlStatus(SiteName,Url,Category,SubCategory,date,Status) VALUES(@SiteName,@SiteUrl,@Category,@SubCategory,@date,@Status)", new { SiteName = SiteName, SiteUrl = SiteUrl, Category = Category, SubCategory = SubCategory, date = CrawlDate, Status = Status });
                        return rowAffected;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    conn.Close();
                }

        }
        #endregion

        #region --For GetSiteFinalUrlTableYPUsa --
        public IEnumerable<dynamic> GetSiteFinalUrlTableYPUsa()
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                try
                {
                    {

                        if (conn.State != ConnectionState.Open)
                            try
                            {
                                conn.Open();
                            }
                            catch (MySqlException ex)
                            {
                                throw (ex);
                            }

                        string strquery = "SELECT Url,Category,SubCategory FROM Bus_Data_YPUsaFinalUrlStatus WHERE Status = 0";
                        IEnumerable<dynamic> result = conn.Query(strquery);
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    conn.Close();
                }
        }
        #endregion

        #region --For UpdateSiteFinalUrlTableYPUsa
        public int UpdateSiteFinalUrlTableYPUsa(string SiteUrl, DateTime CrawlDate, string Status)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                try
                {
                    {

                        if (conn.State != ConnectionState.Open)
                            try
                            {
                                conn.Open();
                            }
                            catch (MySqlException ex)
                            {
                                throw (ex);
                            }

                        int rowAffected = conn.Execute(@"UPDATE Bus_Data_YPUsaFinalUrlStatus SET Status = " + Status + " ,date = '" + CrawlDate + "' where Url =  '" + SiteUrl + "'");
                        return rowAffected;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    conn.Close();
                }

        }
        #endregion

        #region Log(string log)
        public static MyEvents logger = new MyEvents();
        private void Log(string log)
        {
            try
            {
                EventsArgs loggerEventsArgs = new EventsArgs(log);
                logger.LogText(loggerEventsArgs);
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

       
    }
}
