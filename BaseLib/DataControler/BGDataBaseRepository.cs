using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Dapper;
using System.Text;
using System.Diagnostics;
using MySql.Data;
using System.Data;
using System.Threading;
using BotGuruz.Lib;

namespace BotGuruz.MantaLeadPro.DataControler
{
    public class BGDataBaseRepository
    {
        public static DataSet oDS_Main = new DataSet();

        public class BusinessData
        {
            #region global variables
            public long SiteId { get; set; }
            public string url { get; set; }
            public long CatId { get; set; }
            public string catName { get; set; }
            public long SCatId { get; set; }
            public string ScatName { get; set; }
            public string Scat2 { get; set; }
            public string Scat3 { get; set; }
            public string Scat4 { get; set; }
            public string Scat5 { get; set; }
            public string ScrapId { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string BusinName { get; set; }
            public string Slogan { get; set; }
            public string address { get; set; }
            public string zipcode { get; set; }
            public string MPhoneNo { get; set; }
            public string FaxNo { get; set; }
            public string OtherPhoneNo { get; set; }
            public string TollFreeNo { get; set; }
            public string BusDesc { get; set; }
            public string SiteUrl { get; set; }
            public string SiteEmail { get; set; }
            public string Long { get; set; }
            public string Lat { get; set; }
            public string BusDtl1 { get; set; }
            public string BusDtl2 { get; set; }
            public string BusDtl3 { get; set; }
            public string BusDtl4 { get; set; }
            public string BusDtl5 { get; set; }
            public string BusDtl6 { get; set; }
            public string BusDtl7 { get; set; }
            public string BusDtl8 { get; set; }
            public string BusDtl9 { get; set; }
            public string BusDtl10 { get; set; }
            public string BusDtl11 { get; set; }
            public string BusDtl12 { get; set; }
            public string BusDtl13 { get; set; }
            public string Route1 { get; set; }
            public string Route2 { get; set; }
            public string Route3 { get; set; }
            public string Route4 { get; set; }
            public string Route5 { get; set; }
            public string logourl { get; set; }
            public DateTime CrawlDate { get; set; }

            #endregion
        }


        public static string ErrorLogPath = BotGuruz.Lib.Globals.path_AppDataFolderMantaScraper + "\\DataBaseExceptionErrorLog.txt";
        MySqlConnection Conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring());

        #region --For IsSiteExist --
        public bool IsSiteExist(string site_name)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT Site_Id,Site_Name  FROM Site_Table WHERE Site_Name  ='" + site_name + "'";
                    var result = conn.Query<SiteTable>(query).First();
                    if (result != null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                //conn.Close();
            }



        }
        #endregion

        #region --For GetSiteMasterDetail --
        public void GetSiteMasterDetail()
        {
            try
            {
                //DataSet oDS = new DataSet();
                if (oDS_Main.Tables.Contains("Site_Table"))
                {
                    oDS_Main.Tables["Site_Table"].Rows.Clear();
                }
                MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString());
                conn.Open();

                //***************For Site Table **************************************************
                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM Site_Table", conn);
                MySqlDataAdapter oSiteDataAdapter1 = new MySqlDataAdapter(cmd1);
                MySqlCommandBuilder oSiteTableCmdBuilder1 = new MySqlCommandBuilder(oSiteDataAdapter1);
                oSiteDataAdapter1.FillSchema(oDS_Main, SchemaType.Source);
                oSiteDataAdapter1.Fill(oDS_Main, "Site_Table");
                DataTable SiteTable = oDS_Main.Tables["Site_Table"];
                SiteTable.TableName = "Site_Table";
                Log("SiteTable Loading Please wait");
            }
            catch (Exception ex)
            {
                //return null;
            }
            finally
            {
                //conn.Close();
                Log("Site Master Loaded");
            }

        }
        #endregion

        #region --For GetCategoryMasterDetail --
        public void GetCategoryMasterDetail()
        {
            try
            {
                //DataSet oDS = new DataSet();

                if (oDS_Main.Tables.Contains("Category_Table"))
                {
                    oDS_Main.Tables["Category_Table"].Rows.Clear();
                }
                MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString());
                conn.Open();


                //***************For Category Table **************************************************
                MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM Category_Table ORDER BY Site_Id", conn);
                MySqlDataAdapter oSiteDataAdapter2 = new MySqlDataAdapter(cmd2);
                MySqlCommandBuilder oSiteTableCmdBuilder2 = new MySqlCommandBuilder(oSiteDataAdapter2);
                oSiteDataAdapter2.FillSchema(oDS_Main, SchemaType.Source);
                oSiteDataAdapter2.Fill(oDS_Main, "Category_Table");
                DataTable CatTable = oDS_Main.Tables["Category_Table"];
                CatTable.TableName = "Category_Table";
                Log("Category Loading Please wait");
            }
            catch (Exception ex)
            {
                //return null;
            }
            finally
            {
                //conn.Close();
                Log("Category Master Loaded");
            }

        }
        #endregion

        #region --For GetSubCategoryMasterDetail --
        public void GetSubCategoryMasterDetail()
        {
            try
            {
                //DataSet oDS = new DataSet();
                if (oDS_Main.Tables.Contains("SubCategory_Table"))
                {
                    oDS_Main.Tables["SubCategory_Table"].Rows.Clear();
                }
                MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString());
                conn.Open();


                //***************For Sub Category Table **************************************************
                MySqlCommand cmd3 = new MySqlCommand("SELECT * FROM SubCategory_Table Order By Site_Id", conn);
                MySqlDataAdapter oSiteDataAdapter3 = new MySqlDataAdapter(cmd3);
                MySqlCommandBuilder oSiteTableCmdBuilder3 = new MySqlCommandBuilder(oSiteDataAdapter3);
                oSiteDataAdapter3.FillSchema(oDS_Main, SchemaType.Source);
                oSiteDataAdapter3.Fill(oDS_Main, "SubCategory_Table");
                DataTable SubCatTable = oDS_Main.Tables["SubCategory_Table"];
                SubCatTable.TableName = "SubCategory_Table";
                Log("SubCategory Loading Please wait");
            }
            catch (Exception ex)
            {
                //return null;
            }
            finally
            {
                //conn.Close();
                Log("SubCategory Master Loaded");
            }

        }
        #endregion

        #region --For GetSiteDetail --
        public IEnumerable<dynamic> GetSiteDetail(string Site_Name)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    conn.Open();
                    string strquery = "SELECT * FROM Site_Table WHERE Site_Name  ='" + Site_Name + "'";
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
                //conn.Close();
            }
        }
        #endregion

        #region --For InsertSiteTable
        public int InsertSiteTable(string SiteName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    conn.Open();
                    int rowAffected = conn.Execute(@"INSERT INTO Site_Table(Site_Name) VALUES(@Site_Name)", new { Site_Name = SiteName });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                //conn.Close();
            }

        }
        #endregion

        public static int InsertSiteTable1(string categoryName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    int rowAffected = conn.Execute(@"INSERT INTO mainCategory (CategoryName ) VALUES (@categoryName)", new { categoryName = categoryName });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                //conn.Close();
            }

        }

        #region --For IsCategoryExist--
        public bool IsCategoryExist(long site_id, string CatName)
        {
            try
            {

                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT Category_Id,Category_Name  FROM Category_Table WHERE Site_Id  = " + site_id + " and Category_Name = '" + CatName + "'";
                    var result = conn.Query<CategoryTable>(query).First();
                    if (result != null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }
        #endregion

        #region --For GetCatDetail --
        public IEnumerable<dynamic> GetCatDetail(long Site_Id, string Cat_Name)
        {
            try
            {

                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    conn.Open();
                    string strquery = "SELECT * FROM Category_Table WHERE Site_Id = '" + Site_Id + "' and Category_Name  ='" + Cat_Name + "'";
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
                //conn.Close();
            }
        }
        #endregion

        #region --For InsertCategoryTable
        public int InsertCategoryTable(string CatName, long SiteId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    conn.Open();
                    int rowAffected = conn.Execute(@"INSERT INTO Category_Table(Category_Name,Site_Id) VALUES(@Cat_Name, @Site_Id)", new { Cat_Name = CatName, Site_Id = SiteId });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                //conn.Close();
            }

        }
        #endregion

        #region --For IsSubCategoryExist--
        public bool IsSubCategoryExist(long site_id, long cat_it, string SubCatName)
        {
            try
            {

                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT SubCategory_Id,SubCategory_Name FROM SubCategory_Table WHERE Site_Id  = " + site_id + " and Category_Id = " + cat_it + " and SubCategory_Name = '" + SubCatName + "'";
                    var result = conn.Query<SubCategoryTable>(query).First();
                    if (result != null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }
        #endregion

        #region --For IsBusDtlExist --
        public bool IsBusDtlExist(long Site_Id, long Cat_Id, long SubCatId, string ScrapId, string BusName)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {

                    conn.Open();
                    string query = "SELECT * FROM Business_Data_Table WHERE Site_Id = " + Site_Id + " and Category_Id  =" + Cat_Id + " and SubCategory_Id = " + SubCatId + " and ScrapId = '" + ScrapId + "' and Business_Name = '" + BusName + "'";
                    var result = conn.Query<SubCategoryTable>(query).First();
                    if (result != null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                //conn.Close();
            }

        }
        #endregion

        #region --For InsertSubCategoryTable
        public int InsertSubCategoryTable(string SCatName, long CatId, long SiteId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {

                    conn.Open();
                    int rowAffected = conn.Execute(@"INSERT INTO SubCategory_Table(SubCategory_Name,Category_Id,Site_Id) VALUES(@SCat_Name, @Cat_Id,@Site_Id)", new { SCat_Name = SCatName, Cat_Id = CatId, Site_Id = SiteId });
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                //conn.Close();
            }

        }
        #endregion

        #region --For GetSubCatDetail --
        public IEnumerable<dynamic> GetSubCatDetail(long Site_Id, long Cat_Id, string SubCatName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getConnectionString()))
                {
                    conn.Open();
                    string strquery = "SELECT * FROM SubCategory_Table WHERE Site_Id = " + Site_Id + " and Category_Id  =" + Cat_Id + " and SubCategory_Name = '" + SubCatName + "'";
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
                //conn.Close();
            }
        }
        #endregion

        #region --For InsertBusinesDtlTable (Old DataBase)
        public int InsertBusinesDtlTable(long SiteId, long CatId, long SCatId, string Scat2, string Scat3, string Scat4, string Scat5, string ScrapId, string City, string State, string BusinName, string Slogan, string address, string zipcode, string MPhoneNo, string FaxNo, string OtherPhoneNo, string TollFreeNo, string BusDesc, string SiteUrl, string SiteEmail, string Long, string Lat, string BusDtl1, string BusDtl2, string BusDtl3, string BusDtl4, string BusDtl5, string BusDtl6, string BusDtl7, string BusDtl8, string BusDtl9, string BusDtl10, string BusDtl11, string BusDtl12, string BusDtl13, string Route1, string Route2, string Route3, string Route4, string Route5, string logourl, DateTime CrawlDate)
        {
            using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                try
                {


                    {
                        conn.Open();
                        int rowAffected = conn.Execute(@"INSERT INTO Business_Data_TableMain(Site_Id,Category_Id,SubCategory_Id,SubCategory2,SubCategory3,SubCategory4,SubCategory5,ScrapId,City,State,Business_Name,Slogen,Address,ZipCode, " +
                                                               "MainPhoneNo,FaxNo,OtherPhoneNo,TollFreeNo,BusinessDesciption,SiteUrl,SiteEmail,Longitude,Latitude,BusinessDtl1,BusinessDtl2, " +
                                                               "BusinessDtl3,BusinessDtl4,BusinessDtl5,BusinessDtl6,BusinessDtl7,BusinessDtl8,BusinessDtl9,BusinessDtl10,BusinessDtl11, " +
                                                               "BusinessDtl12,BusinessDtl13,Route1,Route2,Route3,Route4,Route5,logourl,CrawlDate) VALUES(@Site_Id,@Cat_Id,@SCat_Id,@Scat2,@Scat3,@Scat4,@Scat5,@Scrp_Id,@City,@State,@BsnName,@Slogun,@Addr, " +
                                                               "@Zipcode,@MPh_No,@FaxNo,@Othr_PhNo,@Toll_freeNo,@BusDesc,@SiteUrl,@SiteEmail,@Long,@Lat,@BusDtl1,@BusDtl2,@BusDtl3,@BusDtl4,@BusDtl5, " +
                                                               "@BusDtl6,@BusDtl7,@BusDtl8,@BusDtl9,@BusDtl10,@BusDtl11,@BusDtl12,@BusDtl13,@Route1,@Route2,@Route3,@Route4,@Route5,@logourl,@Crawl_Date)",
                                                               new
                                                               {
                                                                   Site_Id = SiteId,
                                                                   Cat_Id = CatId,
                                                                   SCat_Id = SCatId,
                                                                   Scat2 = Scat2,
                                                                   Scat3 = Scat3,
                                                                   Scat4 = Scat4,
                                                                   Scat5 = Scat5,
                                                                   Scrp_Id = ScrapId,
                                                                   City = City,
                                                                   State = State,
                                                                   BsnName = BusinName,
                                                                   Slogun = Slogan,
                                                                   Addr = address,
                                                                   Zipcode = zipcode,
                                                                   MPh_No = MPhoneNo,
                                                                   FaxNo = FaxNo,
                                                                   Othr_PhNo = OtherPhoneNo,
                                                                   Toll_freeNo = TollFreeNo,
                                                                   BusDesc = BusDesc,
                                                                   SiteUrl = SiteUrl,
                                                                   SiteEmail = SiteEmail,
                                                                   Long = Long,
                                                                   Lat = Lat,
                                                                   BusDtl1 = BusDtl1,
                                                                   BusDtl2 = BusDtl2,
                                                                   BusDtl3 = BusDtl3,
                                                                   BusDtl4 = BusDtl4,
                                                                   BusDtl5 = BusDtl5,
                                                                   BusDtl6 = BusDtl6,
                                                                   BusDtl7 = BusDtl7,
                                                                   BusDtl8 = BusDtl8,
                                                                   BusDtl9 = BusDtl9,
                                                                   BusDtl10 = BusDtl10,
                                                                   BusDtl11 = BusDtl11,
                                                                   BusDtl12 = BusDtl12,
                                                                   BusDtl13 = BusDtl13,
                                                                   Route1 = Route1,
                                                                   Route2 = Route2,
                                                                   Route3 = Route3,
                                                                   Route4 = Route4,
                                                                   Route5 = Route5,
                                                                   logourl = logourl,
                                                                   Crawl_Date = CrawlDate
                                                               });
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

        #region --For InsertBusinesDtlMasterTable(New DataBase)

        #region Insert Threads Controllers

        static int count_InsertThreads = 0;
        static readonly object lockr_InsertThreads = new object();

        #endregion

        #region MANTA LEADS PRO DB


        private static readonly object locker_MySQLDatabse = new object();

        /// <summary>
        /// inserting cateogry name and url in primary category table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static int InsertData(string name, string Url)
        {
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        int rowAffected = conn.Execute(@"INSERT into primarycategory (cateogryName , url) VALUES ( '" + name + "'  , '" + Url + "' )");
                        return rowAffected;
                    } 
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                {
                    Console.WriteLine(ex.Message + " --> URL : " + Url);
                }
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData(): primary category : ---> " + ex.Message, ErrorLogPath);
                return 0;
            }
        }

        /// <summary>
        /// inserting url and cateogry name in secondary table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="PrimaryID"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int InsertData(string name, int PrimaryID, string url)
        {
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        int rowAffected = conn.Execute(@"INSERT into secondarycateogry (secondaryCateogryName , primaryId , url ) VALUES ( '" + name + "' , " + PrimaryID + "  , '" + url + "')");
                        return rowAffected;
                    } 
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                {
                    Console.WriteLine(ex.Message + " --> URL : " + url);
                }
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : secondary category ---> " + ex.Message, ErrorLogPath);
                return 0;
            }
        }

        /// <summary>
        /// inserting url and category names in tertiary table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="PrimaryID"></param>
        /// <param name="SecondaryId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int InsertData(string name, int PrimaryID, int SecondaryId, string url)
        {
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        int rowAffected = conn.Execute(@"INSERT into tertiarycateogry (name , primaryId , secondaryId , url) VALUES ( '" + name.Replace("'", "") + "' , " + PrimaryID + "  , " + SecondaryId + " , '" + url + "')");
                        return rowAffected;
                    } 
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                {
                    Console.WriteLine(ex.Message + " --> URL : " + url);
                }
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : tertiary category ---> " + ex.Message, ErrorLogPath);
                return 0;
            }
        }

        /// <summary>
        /// inserting url , tertiaryid in urlstore
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tertiartid"></param>
        /// <returns></returns>
        public static int InsertUrlData(string name, int tertiartid)
        {
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        int rowAffected = conn.Execute(@"INSERT into urlstore (url , tertiaryId ) VALUES ( '" + name + "' , " + tertiartid + " )");
                        return rowAffected;
                    } 
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : urlstore ---> " + ex.Message, ErrorLogPath);
                return 0;
            }
        }

        /// <summary>
        /// Select data from any table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectData(string tableName)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT * FROM " + tableName + " limit 0,100";
                        result = conn.Query(query);
                        return result;
                    } 
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Select data from primaryCategory table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectData(string tableName,string[] primaryId)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = string.Format(@"SELECT * FROM " + tableName + " WHERE id IN({0})", string.Join(",", primaryId.ToArray()).TrimEnd(','));
                        //string query = "SELECT * FROM " + tableName + " limit 0,100";
                        result = conn.Query(query);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Select data from any table based on PrimaryId values
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDataPrimaryId(string tableName, string[] primaryId)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = string.Format(@"SELECT * FROM " + tableName + " WHERE primaryId IN ({0})", string.Join(",", primaryId.ToArray()).TrimEnd(','));
                        //string query = "SELECT * FROM " + tableName + " limit 0,100";
                        result = conn.Query(query);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Select data from urlstore table to get distinct value
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDistinctData(string tableName)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT DISTINCT StateUrlId FROM " + tableName + " limit 0,100";
                        result = conn.Query(query);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Select data from urlstore table to get distinct value
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDistinctData2(string tableName)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT DISTINCT secondaryId FROM " + tableName + " limit 0,100";
                        result = conn.Query(query);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Select data from stateurl table to get distinct value
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDistinctData1(string tableName)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT DISTINCT tertiaryId FROM " + tableName + " limit 0,100";
                        result = conn.Query(query);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Select count of StateUrlId from urlstore table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int StatusCountUrlStore(string tableName,string StateUrlId)
        {
            int resultCount = 0;
            int id = Convert.ToInt32(StateUrlId);
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT COUNT(*) FROM " + tableName + "WHERE status=0 AND StateUrlId=" + id + " limit 0,100";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        resultCount = int.Parse(cmd.ExecuteScalar() + "");
                        
                        //result = conn.Query(query);
                        return resultCount;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return resultCount;
        }

        /// <summary>
        /// Select count of tertiaryId from stateurl table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int StatusCountStateUrl(string tableName, string StateUrlId)
        {
            int resultCount = 0;
            int id = Convert.ToInt32(StateUrlId);
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT COUNT(*) FROM " + tableName + "WHERE status=0 AND tertiaryId=" + id + " limit 0,100";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        resultCount = int.Parse(cmd.ExecuteScalar() + "");

                        //result = conn.Query(query);
                        return resultCount;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return resultCount;
        }

        /// <summary>
        /// Select count of secondaryId from stateurl table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int StatusCountTertiaryCategory(string tableName, string StateUrlId)
        {
            int resultCount = 0;
            int id = Convert.ToInt32(StateUrlId);
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT COUNT(*) FROM " + tableName + "WHERE status=0 AND secondaryId=" + id + " limit 0,100";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        resultCount = int.Parse(cmd.ExecuteScalar() + "");

                        //result = conn.Query(query);
                        return resultCount;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : InsertData() : SelectData ant table data ---> " + ex.Message, ErrorLogPath);
            }

            return resultCount;
        }

        /// <summary>
        /// Selecting data with Limit
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectData(string tableName, int count)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT * FROM " + tableName + " WHERE status = '0' OR status = '1' LIMIT  " + count + ", 40 ";
                        result = conn.Query(query);
                        return result;
                    } 
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectData() : SelectData with Limit ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// Select data From Primary Not Crawled data
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDataFromPrimaryNoCrawled()
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT * FROM primarycateogry WHERE status = '0' OR status = '1'";
                        result = conn.Query(query);
                        return result;
                    } 
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectDataFromPrimaryNoCrawled() : SelectDataFromPrimaryNoCrawled ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// select data from secondary cateogry not cralwed data
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDataFromSecondaryNoCrawled()
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT * FROM secondarycateogry WHERE status = '0' OR status = '1'";
                        result = conn.Query(query);
                        return result;
                    } 
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectDataFromSecondaryNoCrawled() : SelectDataFromSecondaryNoCrawled ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// select data tertoiary data not cralwed data
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDataFromTertiaryNoCrawled()
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT * FROM tertiarycateogry WHERE status = '0' OR status = '1'";
                        result = conn.Query(query);
                        return result;
                    } 
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectDataFromTertiaryNoCrawled() : SelectDataFromTertiaryNoCrawled ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// select data from primary cateogry according to cateogry name
        /// </summary>
        /// <param name="cateogryname"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDataFromPrimary(string cateogryname)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT * FROM primarycateogry WHERE cateogryName = '" + cateogryname + "'";
                        result = conn.Query(query);
                        return result;
                    } 
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectDataFromPrimary() : SelectDataFromPrimary from cateogry name ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// select all data from mnata data
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectAllData()
        {
            IEnumerable<dynamic> result = null;
            try
            {
                lock (locker_MySQLDatabse)
                {
                    using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                    {
                        conn.Open();
                        string query = "SELECT * FROM MantaData";
                        result = conn.Query(query);
                        return result;
                    } 
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectAllData() : SelectAllData Manta Data ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        public static int PageSize = 20;
        public static int CurrentPageIndex = 1;
        public static long TotalPage = 0;

        /// <summary>
        /// SelectAllDataManta count
        /// </summary>
        /// <returns></returns>
        public static int SelectAllDataManta()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM company";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    object scalar = cmd.ExecuteScalar();
                    int NoOfPages = int.Parse(scalar.ToString());
                    TotalPage = NoOfPages;
                    return NoOfPages;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectAllDataManta() : SelectAllDataManta Count From Data ---> " + ex.Message, ErrorLogPath);
                return 0;
            }
        }

        /// <summary>
        /// Getting Current record for page data for limit data display on page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static DataSet GetCurrentRecords(int page)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                string query = string.Empty;
                using (MySqlConnection con = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        if (CurrentPageIndex == 1)
                        {
                            query = "Select * FROM company LIMIT " + PageSize + " ";
                        }
                        else
                        {
                            query = "Select * from company LIMIT " + CurrentPageIndex + " , " + PageSize + " "; // +
                        }
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, con))
                    {
                        adapter.Fill(ds, "company");
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetCurrentRecords() : GetCurrentRecords from page no ---> " + ex.Message, ErrorLogPath);
                return new DataSet();
            }
        }

        /// <summary>
        /// select data from secondary cateogry according to category name
        /// </summary>
        /// <param name="cateogryname"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectDataFromSecondary(string cateogryname)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM secondarycateogry WHERE secondaryCateogryName = '" + cateogryname + "'";
                    result = conn.Query(query);
                    return result;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectDataFromSecondary() : SelectDataFromSecondary according to cateogry name ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Getting table from data according to column and data
        /// </summary>
        /// <param name="cateogryname"></param>
        /// <param name="tablename"></param>
        /// <param name="columnname"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GettingData(string cateogryname, string tablename, string columnname)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM " + tablename + " WHERE " + columnname + " = '" + cateogryname + "'";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingData() : GettingData according to coumn name and cateogry name ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// getting data from secondary cateogry according to primary id 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GettingDataFromUrlSecondaryData(int ID)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * from secondarycateogry WHERE status = '1' AND primaryId = '" + ID + "' ";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingDataFromUrlSecondaryData() : GettingDataFromUrlSecondaryData : getting data according to primary id ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// getting data from tertiary table according to id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GettingDataFromUrlTertiaryData(int ID)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * from tertiarycateogry WHERE status = '1' AND secondaryId = '" + ID + "' ";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingDataFromUrlTertiaryData() : GettingDataFromUrlTertiaryData : getting data according to primary id from tertiary ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// getting url from secondary cateogry according to primary id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GettingUrlFromPrimaryID(int ID)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * from secondarycateogry WHERE primaryId = '" + ID + "'";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingUrlFromPrimaryID() : GettingUrlFromPrimaryID : getting url according to primary id ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// getting url from tertiary table according to secondary id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GettingUrlFromSecondaryID(int ID)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * from tertiarycateogry WHERE secondaryId = '" + ID + "'";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingUrlFromSecondaryID() : GettingUrlFromSecondaryID : getting url according to secondary id from tertiary table ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// getting non cralwed data from tertiary cateogry from secondary id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GettingUrlFromSecondaryId(int ID)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * from tertiarycateogry WHERE Status = '0' AND secondaryId = '" + ID + "' ";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingUrlFromSecondaryId() : GettingUrlFromSecondaryId : getting non cralwed data from tertiary cateogry from secondary id ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// getting data from primary cateogry from id and status is not cralwed
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GettingDataFromUrlPrimaryData(int ID)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * from primarycateogry WHERE Status = '0' AND id = '" + ID + "' ";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingDataFromUrlPrimaryData() : GettingDataFromUrlPrimaryData : getting data from primary cateogry from id and status is not cralwed ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// Getting url store data from url of tertiary table
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GettingDataFromUrl(string URL)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * from urlstore WHERE status = '1' AND tertiaryId IN (Select ID from tertiarycateogry WHERE url = '" + URL + "')";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingDataFromUrl() : GettingDataFromUrl : Getting url store data from url of tertiary table ---> " + ex.Message, ErrorLogPath);
            }
            return result;
        }

        /// <summary>
        /// getting id from tertiary cateogry url
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static int GettingIDFromURL(string URL)
        {
            int data = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "Select id from tertiarycateogry WHERE url = '" + URL + "' ";
                    IEnumerable<dynamic> result = conn.Query(query);
                    foreach (dynamic item in result)
                    {
                        data = item.id;
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GettingIDFromURL() : GettingIDFromURL  ---> " + ex.Message, ErrorLogPath);
            }
            return data;
        }

        /// <summary>
        /// updating primary cateogry table
        /// </summary>
        /// <param name="status"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdatePrimaryURLForStatus(string status, int Id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE primarycateogry SET status = '" + status + "' WHERE id = '" + Id + "' ";
                    int data1 = conn.Execute(query);
                    return data1;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdatePrimaryURLForStatus() :  ---> " + ex.Message, ErrorLogPath);
                return 0;
            }
        }

        /// <summary>
        /// updating secondary table status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateSecondaryURLForStatus(string status, string Id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE secondarycateogry SET status = '" + status + "' WHERE id = '" + Id + "' ";
                    int data1 = conn.Execute(query);
                    return data1;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateSecondaryURLForStatus() :  ---> " + ex.Message, ErrorLogPath);
                return 0;
            }
        }

        /// <summary>
        /// update tertiary cateogry status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="ID"></param>
        public static void UpdateTertiaryCateogryURL(string status, int ID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE tertiarycateogry SET status = '" + status + "' WHERE id = '" + ID + "' ";
                    int data1 = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateTertiaryCateogryURL() :  ---> " + ex.Message, ErrorLogPath);
            }
        }

        public static IEnumerable<dynamic> GettingData(string cateogrynamePri, string CateogryNameSec, string tablename, string columnPrimary, string column2)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM " + tablename + " WHERE " + columnPrimary + " = '" + cateogrynamePri + "' AND " + column2 + " = " + CateogryNameSec + " ";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        /// <summary>
        /// getting tertiary cateogry table data fro cateogry name
        /// </summary>
        /// <param name="CateogryName"></param>
        /// <returns></returns>
        public static int GetTertiaryId(string CateogryName)
        {
            IEnumerable<dynamic> result = null;
            int id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM tertiarycateogry WHERE name = '" + CateogryName.Trim() + "' ";
                    result = conn.Query(query);
                    foreach (dynamic data in result)
                    {
                        id = data.ID;
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetTertiaryId() : getting tertiary table data from cateogry name ---> " + ex.Message, ErrorLogPath);
            }

            return id;
        }

        /// <summary>
        /// getting tertiary data from cateogry name
        /// </summary>
        /// <param name="CateogryName"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetTertiaryUrl(string CateogryName)
        {
            IEnumerable<dynamic> result = null;
            int id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    //string query = "SELECT * FROM tertiarycateogry WHERE name = '" + CateogryName.Trim() + "' AND status = 0 OR status = 1 ";
                    string query = "SELECT * FROM tertiarycateogry WHERE name = '" + CateogryName.Trim() + "' AND (status = 0 OR status = 1) ";
                    result = conn.Query(query);
                    return result;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetTertiaryUrl() : getting tertiary data from cateogry name ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Gets data from "tertiarycateogry" table according to tertiary CateogryName
        /// </summary>
        /// <param name="CateogryName"></param>
        /// <param name="Stardindex"></param>
        /// <param name="Endindex"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetTertiaryUrlForUrl(string CateogryName, int Stardindex, int Endindex)
        {
            IEnumerable<dynamic> result = null;
            int id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT id FROM tertiarycateogry WHERE name ='" + CateogryName.Trim() + "'";//"SELECT * FROM tertiarycateogry WHERE name = '" + CateogryName.Trim() + "'";
                    result = conn.Query(query);
                    return result;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetTertiaryUrl() : getting tertiary data from cateogry name ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// Gets data from "company" table where tertiary id is tertiaryid
        /// </summary>
        /// <param name="tertiaryid"></param>
        /// <param name="Stardindex"></param>
        /// <param name="Endindex"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetTeriaryIdData(int tertiaryid, int Stardindex, int Endindex)
        {
            IEnumerable<dynamic> result = null;
            int id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM company WHERE category = '" + tertiaryid + "' limit " + Stardindex + "," + Endindex + "";
                    result = conn.Query(query);
                    return result;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetTertiaryUrl() : getting tertiary data from cateogry name ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// not used test this
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="columnename"></param>
        /// <param name="CateogryName"></param>
        /// <returns></returns>
        public static int GetPrimaryDataByCateogryName(string tablename, string columnename, string CateogryName)
        {
            int id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT id FROM " + tablename + " WHERE " + columnename + " = '" + CateogryName.Trim() + "' ";
                    IEnumerable<dynamic> result = conn.Query(query);
                    foreach (dynamic data in result)
                    {
                        id = data.id;
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetTertiaryUrl() : getting tertiary data from cateogry name ---> " + ex.Message, ErrorLogPath);
            }

            return id;
        }

        /// <summary>
        /// getting status from urlstore for tertiary id where status is not cralwed
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetstatusFromUrl(int data)
        {
            IEnumerable<dynamic> result = null;
            string url = string.Empty;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM urlstore WHERE tertiaryId = " + data + " AND status = '0' ";
                    result = conn.Query(query);
                    return result;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetstatusFromUrl() : GetstatusFromUrl where tertiary id from urlstore ---> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// updating primary status where url is scraped
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="status"></param>
        public static void UpdatePrimaryStatus(string URL, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE primarycategory SET status = '" + status + "' WHERE url = '" + URL + "' ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdatePrimaryStatus() : UpdatePrimaryStatus ---> " + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// update primary catoegry status from id 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="status"></param>
        public static void UpdatePrimarycateogry(int ID, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE primarycategory SET status = '" + status + "' WHERE id = " + ID + " ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdatePrimarycateogry() : UpdatePrimarycateogry from id---> " + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// updating secondary status where url is scraped
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="status"></param>
        public static void UpdateSecondaryStatus(string URL, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE secondarycategory SET status = '" + status + "' WHERE url = '" + URL + "' ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateSecondaryStatus() : UpdateSecondaryStatus ---> " + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// update secondary catoegry status from id 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="status"></param>
        public static void UpdateSecondarycateogry(int ID, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE secondarycategory SET status = '" + status + "' WHERE id = " + ID + " ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateSecondarycateogry() : UpdateSecondarycateogry from id---> " + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// updating tertiary staus where url is scraped
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="status"></param>
        public static void UpdateTertiaryStatus(string URL, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE stateurl SET status = '" + status + "' WHERE url = '" + URL + "' ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateTertiaryStatus() : UpdateTertiaryStatus ---> " + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// updating company table
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="status"></param>
        public static void UpdateCompanyTable(string street, string city, string region, string pincode, string website, string description, string email, int category, int YOB, string location, string employeecount, string url)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE company SET street='" + street + "',city='" + city + "',region='" + region + "',pincode='" + pincode + "',website='" + website + "',description='" + description + "',email='" + email + "',category=" + category + ",yearOfBusiness=" + YOB + ",location='" + location + "',employeecount='" + employeecount + "' WHERE url='" + url + "'"; //WHERE url = '" + URL + "' ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateTertiaryStatus() : UpdateTertiaryStatus ---> " + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// update tertiary catoegry status from id 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="status"></param>
        public static void UpdateTertiarycateogry(string ID, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE tertiarycateogry SET status = '" + status + "' WHERE id = " + ID + " ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateTertiarycateogry() : UpdateTertiarycateogry from id---> " + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// updating state_url staus where url is scraped
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="status"></param>
        public static void UpdateStateUrlStatus(string URL, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE urlstore SET status = '" + status + "' WHERE url = '" + URL + "' ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateTertiaryStatus() : UpdateTertiaryStatus ---> " + ex.Message, ErrorLogPath);
            }
        }

        ///<summary>
        ///update urlstore category status
        ///</summary>>
        public static void UpdateUrlStoreStatus(string URL, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE urlstore SET status = " + status + "WHERE url = " + URL;
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateUrlStoreStatus() : UpdateUrlStoreStatus from id--->" + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// update state_url catoegry status from id 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="status"></param>
        public static void UpdateStateUrlcateogry(int ID, string status)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE stateurl SET status = '" + status + "' WHERE id = " + ID + " ";
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateStateUrlcateogry() : UpdateStateUrlcateogry from id---> " + ex.Message, ErrorLogPath);
            }
        }

        /// <summary>
        /// updating any table staus where url is scraped
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="status"></param>
        public static void UpdateStatus(string tableName, string status,int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "UPDATE " + tableName + " SET status = " + status + "WHERE id = " + id;
                    int data = conn.Execute(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : UpdateTertiaryStatus() : UpdateTertiaryStatus ---> " + ex.Message, ErrorLogPath);
            }
        }



        /// <summary>
        /// getting id for data from company
        /// </summary>
        /// <param name="name"></param>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        //public static long SelectdataForcompnayID(string name , string street , string city , string region)
        //{
        //    long id = 0;
        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
        //        {
        //            conn.Open();
        //            string query = "SELECT id FROM company WHERE name = '" + name + "' AND street = '" + street + "'  AND  city ='" + city + "' AND region = '" + region  + "' ";
        //            IEnumerable<dynamic> result = conn.Query(query);
        //            foreach (dynamic item in result)
        //            {
        //                id = item.id;
        //                return id;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectdataForcompnayID() : SelectdataForcompnayID --> " + ex.Message, ErrorLogPath);
        //    }

        //    return id;
        //}

        /// <summary>
        /// Gets id from "company" table on the basis of name, street, city, region
        /// </summary>
        /// <param name="name"></param>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static long SelectdataForcompnayID(string name, string street, string city, string region)
        {
            long id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT id FROM company WHERE name = '" + name + "' AND street = '" + street + "'  AND  city ='" + city + "' AND region = '" + region + "' ";
                    IEnumerable<dynamic> result = conn.Query(query);
                    foreach (dynamic item in result)
                    {
                        id = item.id;
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectdataForcompnayID() : SelectdataForcompnayID --> " + ex.Message, ErrorLogPath);
            }

            return id;
        }

        /// <summary>
        /// getting data from compnay for userinfo
        /// </summary>
        /// <param name="compnaycontact"></param>
        /// <param name="conmapnayTitle"></param>
        /// <param name="id"></param>
        /// <param name="companyEmail"></param>
        /// <returns></returns>
        public static long SelectUserInfoID(string compnaycontact, string conmapnayTitle, long id, string companyEmail)
        {
            long idPhone = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT id FROM userinfo WHERE name = '" + compnaycontact + "' AND designation = '" + conmapnayTitle + "'  AND  companyid ='" + id + "' AND email = '" + companyEmail + "' ";
                    IEnumerable<dynamic> result = conn.Query(query);
                    foreach (dynamic item in result)
                    {
                        idPhone = item.id;
                        return idPhone;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectUserInfoID() : SelectUserInfoID --> " + ex.Message, ErrorLogPath);
            }

            return id;
        }

        /// <summary>
        /// getting data from userphoneinfo
        /// </summary>
        /// <param name="compnaycontact"></param>
        /// <param name="conmapnayTitle"></param>
        /// <param name="id"></param>
        /// <param name="companyEmail"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetUserPhoneInfo(long id)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT phone FROM userphoneinfo WHERE userid = '" + id + "'";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetUserInfo() : GetUserInfo --> " + ex.Message, ErrorLogPath);
            }

            return result;
        }


        /// <summary>
        /// getting data from userinfo for companyid
        /// </summary>
        /// <param name="compnaycontact"></param>
        /// <param name="conmapnayTitle"></param>
        /// <param name="id"></param>
        /// <param name="companyEmail"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetUserInfo(long id)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM userinfo WHERE companyid = '" + id + "'";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetUserInfo() : GetUserInfo --> " + ex.Message, ErrorLogPath);
            }

            return result;
        }


        /// <summary>
        /// getting data from companyphoneinfo for companyid
        /// </summary>
        /// <param name="compnaycontact"></param>
        /// <param name="conmapnayTitle"></param>
        /// <param name="id"></param>
        /// <param name="companyEmail"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetCompnayPhoneInfo(long id)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT phone FROM companyphoneinfo WHERE companyid = '" + id + "'";
                    result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : GetCompnayPhoneInfo() : GetCompnayPhoneInfo --> " + ex.Message, ErrorLogPath);
            }

            return result;
        }

        /// <summary>
        /// select data from tertiary cateogry from url
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string SelectcateogryNameFromUrl(string Url)
        {
            string data = string.Empty;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM tertiarycateogry WHERE url = '" + Url + "'";
                    IEnumerable<dynamic> result = conn.Query(query);
                    foreach (dynamic item in result)
                    {
                        data = item.name;
                        return data;
                    }

                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : SelectcateogryNameFromUrl() : SelectcateogryNameFromUrl --> " + ex.Message, ErrorLogPath);
            }

            return data;
        }

        /// <summary>
        /// select data from url store
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<dynamic> Selectdata()
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {
                    conn.Open();
                    string query = "SELECT * FROM urlstore";
                    result = conn.Query(query);
                    return result;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLineWithCarat("Error : urlstore() : urlstore --> " + ex.Message, ErrorLogPath);
            }

            return result;
        }
        #endregion

        public int InsertBusinesDtlMasterTable(string tbl, int SiteId, string Url, int CatId, string Cat, int SCatId, string ScatName, string Scat2, string Scat3, string Scat4, string Scat5, string ScrapId, string City, string State, string country, string BusinName, string Slogan, string address, string zipcode, string MPhoneNo, string FaxNo, string OtherPhoneNo, string TollFreeNo, string BusDesc, string SiteUrl, string SiteEmail, string Long, string Lat, string BusDtl1, string BusDtl2, string BusDtl3, string BusDtl4, string BusDtl5, string BusDtl6, string BusDtl7, string BusDtl8, string BusDtl9, string BusDtl10, string BusDtl11, string BusDtl12, string BusDtl13, string Route1, string Route2, string Route3, string Route4, string Route5, string logourl, DateTime CrawlDate)
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

                using (MySqlConnection conn = new MySqlConnection(BG_Db_Class.getNewLocalconnectionstring()))
                {

                    if (conn.State != ConnectionState.Open)
                        try
                        {
                            conn.Open();
                        }
                        catch { }

                    int rowAffected = conn.Execute(@"INSERT INTO " + tbl + " (Site_Id,Url,Category_Id,Category_Name,SubCategory_Id,SubCategory_Name,SubCategory2,SubCategory3,SubCategory4,SubCategory5,ScrapId,City,State,country,Business_Name, " +
                                                   "Slogen,Address,ZipCode,MainPhoneNo,FaxNo,OtherPhoneNo,TollFreeNo,BusinessDesciption,SiteUrl,SiteEmail,Longitude,Latitude,BusinessDtl1,BusinessDtl2,BusinessDtl3,BusinessDtl4,BusinessDtl5,BusinessDtl6, " +
                                                   "BusinessDtl7,BusinessDtl8,BusinessDtl9,BusinessDtl10,BusinessDtl11,BusinessDtl12,BusinessDtl13,Route1,Route2,Route3,Route4,Route5,logourl,CrawlDate) VALUES(@Site_Id,@Url,@Cat_Id,@cat,@SCat_Id,@Scat,@Scat2, " +
                                                   "@Scat3,@Scat4,@Scat5,@Scrp_Id,@City,@State,@Country,@BsnName,@Slogun,@Addr,@Zipcode,@MPh_No,@FaxNo,@Othr_PhNo,@Toll_freeNo,@BusDesc,@SiteUrl,@SiteEmail,@Long,@Lat,@BusDtl1,@BusDtl2,@BusDtl3,@BusDtl4,@BusDtl5, " +
                                                   "@BusDtl6,@BusDtl7,@BusDtl8,@BusDtl9,@BusDtl10,@BusDtl11,@BusDtl12,@BusDtl13,@Route1,@Route2,@Route3,@Route4,@Route5,@logourl,@Crawl_Date)",
                                                           new
                                                           {
                                                               Site_Id = SiteId,
                                                               Url = Url,
                                                               Cat_Id = CatId,
                                                               cat = Cat,
                                                               SCat_Id = SCatId,
                                                               Scat = ScatName,
                                                               Scat2 = Scat2,
                                                               Scat3 = Scat3,
                                                               Scat4 = Scat4,
                                                               Scat5 = Scat5,
                                                               Scrp_Id = ScrapId,
                                                               City = City,
                                                               State = State,
                                                               Country = country,
                                                               BsnName = BusinName,
                                                               Slogun = Slogan,
                                                               Addr = address,
                                                               Zipcode = zipcode,
                                                               MPh_No = MPhoneNo,
                                                               FaxNo = FaxNo,
                                                               Othr_PhNo = OtherPhoneNo,
                                                               Toll_freeNo = TollFreeNo,
                                                               BusDesc = BusDesc,
                                                               SiteUrl = SiteUrl,
                                                               SiteEmail = SiteEmail,
                                                               Long = Long,
                                                               Lat = Lat,
                                                               BusDtl1 = BusDtl1,
                                                               BusDtl2 = BusDtl2,
                                                               BusDtl3 = BusDtl3,
                                                               BusDtl4 = BusDtl4,
                                                               BusDtl5 = BusDtl5,
                                                               BusDtl6 = BusDtl6,
                                                               BusDtl7 = BusDtl7,
                                                               BusDtl8 = BusDtl8,
                                                               BusDtl9 = BusDtl9,
                                                               BusDtl10 = BusDtl10,
                                                               BusDtl11 = BusDtl11,
                                                               BusDtl12 = BusDtl12,
                                                               BusDtl13 = BusDtl13,
                                                               Route1 = Route1,
                                                               Route2 = Route2,
                                                               Route3 = Route3,
                                                               Route4 = Route4,
                                                               Route5 = Route5,
                                                               logourl = logourl,
                                                               Crawl_Date = CrawlDate
                                                           });
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

        #region --For BulkInsertBusinesDtlTable
        public void BulkInsertBusinesDtlTable(System.Data.DataTable table, string tbl)
        {
            try
            {
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
                    var lst = new List<BusinessData>();
                    foreach (System.Data.DataRow item in table.Rows)
                    {

                        lst.Add(
                            new BusinessData()
                            {
                                SiteId = Convert.ToInt64(item.ItemArray[0].ToString()),
                                url = (item.ItemArray[1].ToString()),
                                CatId = Convert.ToInt64(item.ItemArray[2].ToString()),
                                catName = (item.ItemArray[3].ToString()),
                                SCatId = Convert.ToInt64(item.ItemArray[4].ToString()),
                                ScatName = (item.ItemArray[5].ToString()),
                                Scat2 = (item.ItemArray[6].ToString()),
                                Scat3 = (item.ItemArray[7].ToString()),
                                Scat4 = (item.ItemArray[8].ToString()),
                                Scat5 = (item.ItemArray[9].ToString()),
                                ScrapId = (item.ItemArray[10].ToString()),
                                City = (item.ItemArray[11].ToString()),
                                State = (item.ItemArray[12].ToString()),
                                BusinName = (item.ItemArray[13].ToString()),
                                Slogan = (item.ItemArray[14].ToString()),
                                address = (item.ItemArray[15].ToString()),
                                zipcode = (item.ItemArray[16].ToString()),
                                MPhoneNo = (item.ItemArray[17].ToString()),
                                FaxNo = (item.ItemArray[18].ToString()),
                                OtherPhoneNo = (item.ItemArray[19].ToString()),
                                TollFreeNo = (item.ItemArray[20].ToString()),
                                BusDesc = (item.ItemArray[21].ToString()),
                                SiteUrl = (item.ItemArray[22].ToString()),
                                SiteEmail = (item.ItemArray[23].ToString()),
                                Long = (item.ItemArray[24].ToString()),
                                Lat = (item.ItemArray[25].ToString()),
                                BusDtl1 = (item.ItemArray[26].ToString()),
                                BusDtl2 = (item.ItemArray[27].ToString()),
                                BusDtl3 = (item.ItemArray[28].ToString()),
                                BusDtl4 = (item.ItemArray[29].ToString()),
                                BusDtl5 = (item.ItemArray[30].ToString()),
                                BusDtl6 = (item.ItemArray[31].ToString()),
                                BusDtl7 = (item.ItemArray[32].ToString()),
                                BusDtl8 = (item.ItemArray[33].ToString()),
                                BusDtl9 = (item.ItemArray[34].ToString()),
                                BusDtl10 = (item.ItemArray[35].ToString()),
                                BusDtl11 = (item.ItemArray[36].ToString()),
                                BusDtl12 = (item.ItemArray[37].ToString()),
                                BusDtl13 = (item.ItemArray[38].ToString()),
                                Route1 = (item.ItemArray[39].ToString()),
                                Route2 = (item.ItemArray[40].ToString()),
                                Route3 = (item.ItemArray[41].ToString()),
                                Route4 = (item.ItemArray[42].ToString()),
                                Route5 = (item.ItemArray[43].ToString()),
                                logourl = (item.ItemArray[44].ToString()),
                                CrawlDate = Convert.ToDateTime(item.ItemArray[45].ToString()),


                            });
                    }

                    MySqlTransaction trans = conn.BeginTransaction();


                    conn.Execute(@"INSERT INTO " + tbl + "(Site_Id,Url,Category_Id,Category_Name,SubCategory_Id,SubCategory_Name,SubCategory2,SubCategory3,SubCategory4,SubCategory5,ScrapId,City,State,Business_Name,Slogen,Address,ZipCode,MainPhoneNo,FaxNo,OtherPhoneNo,TollFreeNo,BusinessDesciption,SiteUrl,SiteEmail,Longitude,Latitude,BusinessDtl1,BusinessDtl2,BusinessDtl3,BusinessDtl4,BusinessDtl5,BusinessDtl6,BusinessDtl7,BusinessDtl8,BusinessDtl9,BusinessDtl10,BusinessDtl11,BusinessDtl12,BusinessDtl13,Route1,Route2,Route3,Route4,Route5,logourl,CrawlDate)"
                               + " VALUES(@SiteId,@url,@CatId,@catName,@SCatId,@ScatName,@Scat2,@Scat3,@Scat4,@Scat5,@ScrapId,@City,@State,@BusinName,@Slogan,@address,@zipcode,@MPhoneNo,@FaxNo,@OtherPhoneNo,@TollFreeNo,@BusDesc,@SiteUrl,@SiteEmail,@Long,@Lat,@BusDtl1,@BusDtl2,@BusDtl3,@BusDtl4,@BusDtl5,@BusDtl6,@BusDtl7,@BusDtl8,@BusDtl9,@BusDtl10,@BusDtl11,@BusDtl12,@BusDtl13,@Route1,@Route2,@Route3,@Route4,@Route5,@logourl,@CrawlDate)", lst);

                    trans.Commit();
                    lst.Clear();

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //conn.Close();
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

        internal bool IsBusDtlExist(int p, long p_2, long p_3, string scarpId, string endName)
        {
            throw new NotImplementedException();
        }



        
    }
}
