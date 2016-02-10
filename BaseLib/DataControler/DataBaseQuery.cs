using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using MySql.Data.MySqlClient;


using Dapper;
using System.Text;
using System.Diagnostics;
using MySql.Data;
using System.Data;
using System.Threading;

namespace BaseLibFB.DataControler
{

    public class QueryManager
    {
        #region Account_tb

        public void AddAccountData(string username, string Password, string Ipaddress, string ProxyPort, string ProxyUsername, string ProxyPassword)
        {
            string InsertQuery = "insert into Tb_Accounts (Username,Password,ProxyIPAddress,ProxyPort,ProxyUsername,ProxyPassword) values('" + username + "','" + Password + "','" + Ipaddress + "','" + ProxyPort + "','" + ProxyUsername + "','" + ProxyPassword + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "Tb_Accounts");
        }

        public void InsertDataUrlStore(string URL, int CategoryId, string DateOfParsing, int Status, int StateUrlId,string CampaignName)
        {
            string InsertQuery = "Insert into UrlStore (url, CategoryId, DateOfParsing, Status, StateId, CampaignName) values('" + URL + "'," + CategoryId + ",'" + DateOfParsing + "'," + Status + "," + StateUrlId + ",'" + CampaignName + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "UrlStore");
        }

        public void InsertDataPropertyUrls(string URL,string CampaignName)
        {
              //URL = "http://zillow.com/homedetails/";
           
            string InsertQuery = "Insert into PropertyUrls (PropertyUrls, CampaignName,IsUsed) values('" + URL+ "','" + CampaignName + "','"+"0"+"')";
            DataBaseHandler.PerformQuery(InsertQuery, "PropertyUrls");
        }

        public void InsertCampaign(string campaignName)
        {
            string InsertQuery = "Insert into Campaign (CampaignName) values('" + campaignName + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "Campaign");
        }

        public void deleteFromAllDataByCampaignName(string campaignName)
        {
            string DeleteQuery = "Delete from Campaign where CampaignName='" + campaignName + "'";
            DataBaseHandler.PerformQuery(DeleteQuery, "Campaign");

            DeleteQuery = "Delete from PropertyUrls where CampaignName='" + campaignName + "'";
            DataBaseHandler.PerformQuery(DeleteQuery, "PropertyUrls");

            DeleteQuery = "Delete from UrlStore where CampaignName='" + campaignName + "'";
            DataBaseHandler.PerformQuery(DeleteQuery, "UrlStore");

            DeleteQuery = "Delete from propertyinfo where CampaignName='" + campaignName + "'";
            DataBaseHandler.PerformQuery(DeleteQuery, "propertyinfo");
            
        }

        public void InsertProxyData(string Address, string Port, string UserName, string Password, string Type)
        {
            string InsertQuery = "Insert into ProxyData (Address,Port,UserName,Password,Type) values('" + Address + "','" + Port + "','" + UserName + "','" + Password + "','" + Type + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "ProxyData");
        }

        public void InsertPropertInfo(string Url, string StreetName, string City, string Zip, string Beds, string Baths, string Sqft, string Lot, string Type, string YearOfBuilt, string LastSold, string Parking, string Cooling, string Heating, string Fireplace, string DaysOnZillow, string MLSNumber, string AdditionalFeatures, string CountyName, string CoveredParkingSpaces, string LegalDescription, string NearTransportation, string Over55ActiveCommunity, string ParcelNumber, string RoofType, string RoomCount, string RoomTypes, string RVParking, string SecuritySystem, string NoOfStories, string StructureType, string UnitCount, int itemId, string CampaignName, string Zestimate, string ZestimateForRent)   // Zestimate, ZestimateForRent
        {
            string InsertQuery = "Insert into propertyinfo (Url, Address,City,Zip, Beds,Baths,Sqft,Lot,Type,YearOfBuilt,LastSold,Parking,Cooling,Heating,Fireplace,DaysOnZillow,MLSNumber,AdditionalFeatures,CountyName,CoveredParkingSpaces,LegalDescription,NearTransportation,Over55ActiveCommunity,ParcelNumber,RoofType,RoomCount,RoomTypes,RVParking,SecuritySystem,NoOfStories,StructureType,UnitCount,PropertyInfoId,CampaignName,Zestimate,ZestimateForRent) values('" + Url + "','" + StreetName + "','" + City + "','" + Zip + "','" + Beds + "','" + Baths + "','" + Sqft + "','" + Lot + "','" + Type + "','" + YearOfBuilt + "','" + LastSold + "','" + Parking + "','" + Cooling + "','" + Heating + "','" + Fireplace + "','" + DaysOnZillow + "','" + MLSNumber + "','" + AdditionalFeatures + "','" + CountyName + "','" + CoveredParkingSpaces + "','" + LegalDescription + "','" + NearTransportation + "','" + Over55ActiveCommunity + "','" + ParcelNumber + "','" + RoofType + "','" + RoomCount + "','" + RoomTypes + "','" + RVParking + "','" + SecuritySystem + "','" + NoOfStories + "','" + StructureType + "','" + UnitCount + "'," + itemId + ",'" + CampaignName + "','" + Zestimate + "','" + ZestimateForRent + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "propertyinfo");
        }

        public void AddUrlAccountData(string User, string Password, string Url)
        {
            string InsertQuery = "insert into Tb_AccountUrl(User,Password,Url) values('" + User + "','" + Password + "','" + Url + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "Tb_AccountUrl");
        }
        public DataSet SelectAccountData()
        {
            string SelectQuery = "select * from Tb_Accounts";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Tb_Accounts");
            return ds;
        }

        public DataSet SelectSecondaryCategoryId(string name)
        {
            string SelectQuery = "select id, PrimaryCategory from SecondaryCategory where CategoryName='"+name+"'";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "SecondaryCategory");
            return ds;
        }

        public DataSet SelectStateCodeId(string name)
        {
            string SelectQuery = "select id from State where StateCode='" + name + "'";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "State");
            return ds;
        }

        public DataSet SelectUrl(string CampaignName)
        {
            string SelectQuery = "select url from UrlStore where CampaignName='" + CampaignName + "'";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "UrlStore");
            return ds;
        }

        public DataSet SelectUrlfromPropertyUrls(string CampaignName)
        {
            string SelectQuery = "select id, PropertyUrls from PropertyUrls where CampaignName='" + CampaignName + "' and IsUsed = 0";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "PropertyUrls");
            return ds;
        }

        public DataSet SelectUrlAccountData()
        {
            string SelectQuery = "select * from Tb_AccountUrl";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Tb_AccountUrl");
            return ds;
        }

        public DataSet SelectUnSetProxyAccountData()
        {
            string SelectQuery = "select * from RedditAccount where ProxyAddress='" + "" + "' AND ProxyPort='" + "" + "'";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "RedditAccount");
            return ds;
        }


        public void DeleteAccount()
        {
            string SelectQuery = "Delete from Tb_Accounts";
            DataBaseHandler.PerformQuery(SelectQuery, "Tb_Accounts");
        }

        public void DeleteUrlAccount()
        {
            string SelectQuery = "Delete from Tb_AccountUrl";
            DataBaseHandler.PerformQuery(SelectQuery, "Tb_AccountUrl");
        }

        public void DeleteSelectedAccount(string Username, string Password)
        {
            string SelectQuery = "Delete from Tb_Accounts where [Username]='" + Username + "' AND [Password]='" + Password + "'";
            DataBaseHandler.PerformQuery(SelectQuery, "Tb_Accounts");
        }

        public void DeleteSelectedAccountUrl(string Username, string Password, string url)
        {
            string SelectQuery = "Delete from Tb_AccountUrl where [User]='" + Username + "' AND [Password]='" + Password + "' AND [Url]='" + url + "'";
            DataBaseHandler.PerformQuery(SelectQuery, "Tb_AccountUrl");
        }

        public void DeleteProxyAccount()
        {
            string SelectQuery = "UPDATE RedditAccount SET ProxyAddress ='" + "" + "',ProxyPort='" + "" + "',ProxyUserName='" + "" + "',ProxyUserPassword='" + "" + "'";
            DataBaseHandler.PerformQuery(SelectQuery, "RedditAccount");
        }


        public void UpdateAccountData(string username, string Password, string Ipaddress, string ProxyPort, string ProxyUsername, string ProxyPassword)
        {
            string UpdateQuery = "UPDATE RedditAccount SET ProxyAddress ='" + Ipaddress + "',ProxyPort='" + ProxyPort + "',ProxyUserName='" + ProxyUsername + "',ProxyUserPassword='" + ProxyPassword + "' where Username='" + username + "' And Password='" + Password + "'";
            DataBaseHandler.PerformQuery(UpdateQuery, "Tb_Accounts");
        }

        public void UpdateTitleAccountUrlData(string username, string Password, string url, string title)
        {
            string UpdateQuery = "UPDATE Tb_AccountUrl SET Title ='" + title + "' where User='" + username + "' And Password='" + Password + "'And Url='" + url + "'";
            DataBaseHandler.PerformQuery(UpdateQuery, "Tb_AccountUrl");
        }

        public void UpdatePostUrlAccountUrlData(string username, string Password, string url, string PostUrl)
        {
            string UpdateQuery = "UPDATE Tb_AccountUrl SET PostUrl ='" + PostUrl + "' where User='" + username + "' And Password='" + Password + "'And Url='" + url + "'";
            DataBaseHandler.PerformQuery(UpdateQuery, "Tb_AccountUrl");
        }


        public void UpdateAccountDataStatus(string username, string Password, string status)
        {
            string UpdateQuery = "UPDATE Tb_Accounts SET Status ='" + status + "'where Username='" + username + "' And Password='" + Password + "'";
            DataBaseHandler.PerformQuery(UpdateQuery, "Tb_Accounts");
        }

        public void UpdatePropertyUrls(string url)
        {
            string UpdateQuery = "UPDATE PropertyUrls SET IsUsed = 1";
            DataBaseHandler.PerformQuery(UpdateQuery, "PropertyUrls");
        }




        #endregion


        public DataSet getAllPropertyInfo(string campaignName)
        {
            DataSet ds = new DataSet();
            string selectQuery = "Select Url,Address,City,Zip,Beds,Baths,Sqft,Lot,Type,YearOfBuilt,LastSold,Parking,Cooling,Heating,Fireplace,DaysOnZillow,MLSNumber,AdditionalFeatures,CountyName,CoveredParkingSpaces,LegalDescription,NearTransportation,Over55ActiveCommunity,ParcelNumber,RoofType,RoomCount,RoomTypes,RVParking,SecuritySystem,NoOfStories,StructureType,UnitCount,PropertyInfoId,Zestimate,ZestimateForRent from propertyinfo where CampaignName='" + campaignName + "'";
            ds = DataBaseHandler.SelectQuery(selectQuery, "propertyinfo");
            return ds;
        }

        public DataSet getUrlStoreInfo(string campaignName)
        {
            DataSet ds = new DataSet();
            string selectQuery = "Select url from UrlStore where CampaignName='" + campaignName + "'";
            ds = DataBaseHandler.SelectQuery(selectQuery, "UrlStore");
            return ds;
        }

        public DataSet getPropertyUrlsInfo(string campaignName)
        {
            DataSet ds = new DataSet();
            string selectQuery = "Select * from PropertyUrls where CampaignName='" + campaignName + "'";
            ds = DataBaseHandler.SelectQuery(selectQuery, "PropertyUrls");
            return ds;
        }

        #region Tb_Campaign

        public void AddCampaigndata(string CampaignName, string Date)
        {
            string InsertQuery = "insert into Tb_Campaign(CampaignName,Date) values('" + CampaignName + "','" + Date + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "Tb_Campaign");
        }

        public DataSet SelectCampaignData()
        {
            string SelectQuery = "select * from Tb_Campaign";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Tb_Campaign");
            return ds;
        }

        public DataSet SelectCampaignIDbyCampaign(string campname)
        {
            string SelectQuery = "select Campaign_id from Tb_Campaign where Campaign_name='" + campname + "'";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Tb_Campaign");
            return ds;
        }

        public void DeleteCampaign(string CampId)
        {
            string DelectQuery = "Delete from propertyinfo where CampaignName='" + CampId + "'";
            DataBaseHandler.PerformQuery(DelectQuery, "propertyinfo");
        }

        public void DeleteCampaign()
        {
            string SelectQuery = "Delete from Tb_Campaign";
            DataBaseHandler.PerformQuery(SelectQuery, "Tb_Campaign");
        }


        public DataSet SelectCampaign()
        {
            DataSet ds = new DataSet();
            string SelectQuery = "Select CampaignName from Campaign";
            ds=DataBaseHandler.SelectQuery(SelectQuery,"Campaign");
            return ds;
        }

        public DataSet SelectCampaignUnique(string campaignName)
        {
            DataSet ds = new DataSet();
            string SelectQuery = "Select CampaignName from Campaign where CampaignName='" + campaignName + "'";
            ds = DataBaseHandler.SelectQuery(SelectQuery, "Campaign");
            return ds;
        }

        public DataSet SelectProxyData()
        {
            DataSet ds = new DataSet();
            string SelectQuery = "Select Address,Port,UserName,Password from ProxyData";
            ds = DataBaseHandler.SelectQuery(SelectQuery, "ProxyData");
            return ds;
        }

        public DataSet SelectSecondaryCategory(int CategoryId)
        {
            DataSet ds = new DataSet();
            string SelectQuery = "Select CategoryName from SecondaryCategory where id=" + CategoryId + "";
            ds = DataBaseHandler.SelectQuery(SelectQuery, "SecondaryCategory");
            return ds;
        }

        public DataSet SelectState(int StateId)
        {
            DataSet ds = new DataSet();
            string SelectQuery = "Select StateName from State where id=" + StateId + "";
            ds = DataBaseHandler.SelectQuery(SelectQuery, "State");
            return ds;
        }

        public DataSet SelectUrlStore(string CampaignName)
        {
            DataSet ds = new DataSet();
            string SelectQuery = "Select url,CategoryId,StateId from UrlStore Where CampaignName ='" + CampaignName + "'";
            ds = DataBaseHandler.SelectQuery(SelectQuery, "UrlStore");
            return ds;
        }


        #endregion


        #region Tb_Url

        public void AddLikeUrl(string likeUrl, string LikeTarget, string CompleteTarget, string Status)
        {
            string InsertQuery = "insert into Tb_Url (LikeUrl,LikeTarget,CompleteTarget,Status,Date) values('" + likeUrl + "','" + LikeTarget + "','" + CompleteTarget + "','" + Status + "','" + System.DateTime.Now.ToString() + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "Tb_Url");
        }

        public DataSet SelectLikeUrlByStatus(string status)
        {
            string SelectQuery = "select LikeUrl,LikeTarget,CompleteTarget,Status from Tb_Url where Status='" + status + "'";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Tb_Url");
            return ds;
        }

        public DataSet SelectUrlData()
        {
            string SelectQuery = "select StateName from State";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "State");
            return ds;
        }

        public void DeleteUrls()
        {
            string SelectQuery = "Delete from Tb_Url";
            DataBaseHandler.PerformQuery(SelectQuery, "Tb_Url");
        }


        public void DeleteUrls(string url)
        {
            string SelectQuery = "Delete from Tb_Url where LikeUrl='" + url + "'";
            DataBaseHandler.PerformQuery(SelectQuery, "Tb_Url");

        }



        public void UpdateUrlLikeCount(string likeurl, string target)
        {
            string UpdateQuery = "UPDATE Tb_Url SET CompleteTarget ='" + target + "'where LikeUrl='" + likeurl + "'";
            DataBaseHandler.PerformQuery(UpdateQuery, "Tb_Url");
        }
        #endregion


        #region User_details


        public DataSet SelectUserDeatails()
        {
            string SelectQuery = "select * from User_tb";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "User_tb");
            return ds;
        }


        public void AddUser(string User, string UserPassword)
        {
            string InsertQuery = "insert into User_tb (User_name,User_Password,Date) values('" + User + "','" + UserPassword + "','" + System.DateTime.Now.ToShortDateString().ToString() + "')";
            DataBaseHandler.PerformQuery(InsertQuery, "User_tb");
        }




        public void DeleteUserAccount(string User)
        {
            string DeleteQuery = "delete from User_tb where User_name = '" + User + "'";
            DataBaseHandler.PerformQuery(DeleteQuery, "User_tb");
        }
        #endregion


        #region proxy
        public DataSet SelectPublicProxyDeatails()
        {
            string SelectQuery = "SELECT * FROM Tb_ProxyDetails WHERE IsPublic = 0";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Tb_ProxyDetails");
            return ds;
        }

        public DataSet SelectPrivateProxyDeatails()
        {
            string SelectQuery = "SELECT * FROM Tb_ProxyDetails WHERE IsPublic = 1";
            DataSet ds = DataBaseHandler.SelectQuery(SelectQuery, "Tb_ProxyDetails");
            return ds;
        }

        public DataSet getProxyCount()
        {
            DataSet ds = new DataSet();
            try
            {
                string SelectQuery = "Select Address,Port from ProxyData";
                ds=DataBaseHandler.SelectQuery(SelectQuery, "ProxyData");
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public void clearPublicProxy()
        {
            try
            {
                string DeleteQuery = "Delete from ProxyData";
                DataBaseHandler.DeleteQuery(DeleteQuery, "ProxyData");
            }
            catch { }
        }

        public void clearPrivateProxy()
        {
            try
            {
                string DeleteQuery = "Delete from ProxyData where Type not in('public')";
                DataBaseHandler.DeleteQuery(DeleteQuery, "ProxyData");
            }
            catch { }
        }
        #endregion

        
    }

    public class ServerDatabaseQuery
    {
        public void InsertStates(int id, string stateName, string StateCode)
        {
            IEnumerable<dynamic> result = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(getConnectionString()))
                {
                    conn.Open();
                    conn.Execute(@"Insert Into state (id, StateName, StateCode ) VALUES ("+id+", '" + stateName + "' , '" + StateCode + "')");
                    //string query = "Insert Into state Values(" + id + "," + stateName + "," + StateCode + ")";
                    //result = conn.Query(query);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static string getConnectionString()
        {
            return "Host=127.0.0.1;User ID=root;Password=ankit;persist security info=False;initial catalog=zillowleadspro;Pooling=false;";
        }
    }
}
