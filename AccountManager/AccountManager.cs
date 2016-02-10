using BaseLibID;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLib;
using System.Text.RegularExpressions;
using System.Web;

namespace Accounts
{
    public class AccountManager
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string proxyAddress { get; set; }
        public string proxyPort { get; set; }
        public string proxyUsername { get; set; }
        public string proxyPassword { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public bool LoggedIn { get; set; }
        public static string Authorization = string.Empty;
        public static string acc_status = string.Empty;
        public GlobusHttpHelper httpHelper = new GlobusHttpHelper();
        public ChilkatHttpHelpr chilkathttpHelper = new ChilkatHttpHelpr();
        bool value = true;
                
        public string LoginUsingGlobusHttp(ref InstagramUser InstagramUser)
        {
            ///Sign In
            #region comment

            //GlobusHttpHelper httpHelper = InstagramUser.globusHttpHelper;


            //GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logging in with Account : " + InstagramUser.username + " ]");
            //string Status = "Failed";
            //try
            //{
            //    string firstUrl = "https://api.instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
            //    #region for Chk Authorization By Anil
            //    //  string Authorization_respo = httpHelper.getHtmlfromUrl(new Uri(firstUrl));

            //    #endregion
            //    //https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes

            //    string secondURL = "https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
            //    // string Authorization_respo1 = httpHelper.getHtmlfromUrl(new Uri(secondURL));
            //    ChilkatHttpHelpr objchilkat = new ChilkatHttpHelpr();
            //    string res_secondURL = string.Empty;
            //    if (!string.IsNullOrEmpty(proxyAddress) && !string.IsNullOrEmpty(proxyPort))
            //    {
            //        try
            //        {
            //            // res_secondURL = objchilkat.GetHtmlProxy(secondURL, proxyAddress, proxyPort, proxyUsername, proxyPassword);
            //            res_secondURL = httpHelper.getHtmlfromUrlProxy(new Uri(secondURL), "", proxyAddress, proxyPort, proxyUsername, proxyPassword);
            //        }
            //        catch { };
            //    }
            //    else
            //    {
            //        res_secondURL = httpHelper.getHtmlfromUrl(new Uri(secondURL), "");
            //        //res_secondURL = HttpHelper.getHtmlfromUrlProxy(new Uri(secondURL), "", proxyAddress, proxyPort, proxyUsername, proxyPassword);
            //    }
            //   string nextUrl = string.Empty;
            //    string res_nextUrl = string.Empty;

            //    if (!string.IsNullOrEmpty(res_secondURL))
            //    {
            //        nextUrl = "https://instagram.com/accounts/login/?force_classic_login=&next=/oauth/authorize/%3Fclient_id%3D9d836570317f4c18bca0db6d2ac38e29%26redirect_uri%3Dhttp%3A//websta.me/%26response_type%3Dcode%26scope%3Dcomments%2Brelationships%2Blikes";

            //        res_nextUrl = httpHelper.getHtmlfromUrl(new Uri(nextUrl), "");//postFormDataProxy
            //    }
            //    else
            //    {
            //        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged in Failed with Account :" + InstagramUser.username + " ]");
            //        Status = "Failed";
            //        this.LoggedIn = false;
            //    }



            //    try
            //    {
            //        int FirstPointToken_nextUrl = res_nextUrl.IndexOf("csrfmiddlewaretoken");//csrfmiddlewaretoken
            //        string FirstTokenSubString_nextUrl = res_nextUrl.Substring(FirstPointToken_nextUrl);
            //        int SecondPointToken_nextUrl = FirstTokenSubString_nextUrl.IndexOf("/>");
            //        this.Token = FirstTokenSubString_nextUrl.Substring(0, SecondPointToken_nextUrl).Replace("csrfmiddlewaretoken", string.Empty).Replace("value=", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty).Trim();
            //    }
            //    catch { };


            //    string login = "https://instagram.com/accounts/login/?force_classic_login=&next=/oauth/authorize/%3Fclient_id%3D9d836570317f4c18bca0db6d2ac38e29%26redirect_uri%3Dhttp%3A//websta.me/%26response_type%3Dcode%26scope%3Dcomments%2Brelationships%2Blikes";


            //    string postdata_Login = string.Empty;
            //    string res_postdata_Login = string.Empty;
            //    try
            //    {
            //        postdata_Login = "csrfmiddlewaretoken=" + this.Token + "&username=" + InstagramUser.username + "&password=" + InstagramUser.password + "";
            //    }
            //    catch { };
            //    try
            //    {

            //        res_postdata_Login = httpHelper.postFormData(new Uri(login), postdata_Login, login, "");
            //        if(res_postdata_Login.Contains("value=\"Authorize\""))
            //    {
            //        string res_token= string.Empty;
            //        //string csrftoken = "https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
            //        try
            //        {
            //             res_token = httpHelper.getHtmlfromUrl(new Uri("https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes"), "https://instagram.com/accounts/login/?force_classic_login=&next=/oauth/authorize/%3Fclient_id%3D9d836570317f4c18bca0db6d2ac38e29%26redirect_uri%3Dhttp%3A//websta.me/%26response_type%3Dcode%26scope%3Dcomments%2Brelationships%2Blikes");
            //        }
            //        catch { };
            //        string csrftoken = Utils.getBetween(res_token, "\"csrfmiddlewaretoken\" value=\"", "\"/>");
            //        string login_Authorise="https://instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
            //        string postAuthorise = "csrfmiddlewaretoken=" + csrftoken + "&allow=Authorize";
            //        try
            //        {
            //            string res_postAuthorise = httpHelper.postFormData(new Uri(login_Authorise), postAuthorise, login_Authorise, "");
            //        }
            //        catch { };
            //    }

            //        if (res_postdata_Login.Contains("Authorization Request &mdash; Instagram"))
            //        {
            //            Authorization = "No";
            //        }
            //        else
            //        {
            //            Authorization = "Yes";
            //        }
            //        if (res_postdata_Login.Contains("Please register your email address from") || res_postdata_Login.Contains("Please register your Phone Number from"))
            //        {
            //            if (res_postdata_Login.Contains("Please register your Phone Number from"))
            //            {
            //                acc_status = "Phone";
            //            }
            //            else
            //            {
            //                acc_status = "Email";
            //            }

            //        }
            //        else
            //        {
            //            acc_status = "Ok";
            //        }
            //    }
            //    catch { };

            //    string autho = "https://instagram.com/oauth/authorize/?scope=comments+likes+relationships&redirect_uri=http%3A%2F%2Fwww.gramfeed.com%2Foauth%2Fcallback%3Fpage%3D&response_type=code&client_id=b59fbe4563944b6c88cced13495c0f49";

            //    if (res_postdata_Login.Contains("Please enter a correct username and password"))
            //    {
            //        Status = "Failed";
            //        this.LoggedIn = false;
            //    }
            //    else if (res_postdata_Login.Contains("requesting access to your Instagram account") || postdata_Login.Contains("is requesting to do the following"))
            //    {
            //        Status = "AccessIssue";
            //    }
            //    else if (res_postdata_Login.Contains("logout") || postdata_Login.Contains("LOG OUT"))
            //{
            //        GlobusLogHelper.log.Info("[ " + DateTime.Now + " ] => [ Logged in with Account Success :" + InstagramUser.username + " ]");
            //        Status = "Success";
            //        this.LoggedIn = true;
            //        string str = httpHelper.getHtmlfromUrl(new Uri("http://websta.me/n/" + InstagramUser.username));
            //        UpdateCampaign(InstagramUser.username, str, "Success");
            //        InstagramUser.isloggedin = true;
            //    }
            //    else if (string.IsNullOrEmpty(res_secondURL))
            //    {

            //        Status = "Failed";
            //        this.LoggedIn = false;
            //        InstagramUser.isloggedin = false;
            //    }

            //    //nameval.Clear();
            //    return Status;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;

            //}

            #endregion
            
            try
            {
                string Status = "Failed";
                string url = IGGlobals.Instance.IGInstagramurl;
                string firstResoponse = InstagramUser.globusHttpHelper.GetData_LoginThroughInstagram(new Uri(url), InstagramUser.proxyip, InstagramUser.proxyport, InstagramUser.proxyusername, InstagramUser.proxypassword);
                string poatData = "username=" + InstagramUser.username + "&password=" + InstagramUser.password;  //csrftoken
                url = IGGlobals.Instance.IGInstagramurlsecond;
                string token = "";
                string response = InstagramUser.globusHttpHelper.PostData_LoginThroughInstagram(new Uri(url), poatData, "", token);                
                string dataBeforelogin = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"), "");

                if (dataBeforelogin.Contains("Was This You?"))
                {
                    string crs_token = Utils.getBetween(response, "=\"csrfmiddlewaretoken\" value=\"", "\"/>");
                    string post_data = "csrfmiddlewaretoken=" + crs_token + "&approve=It+Was+Me";
                    string next_hit = InstagramUser.globusHttpHelper.PostData_LoginThroughInstagram(new Uri("https://www.instagram.com/integrity/checkpoint/?next=%2F"), post_data, "", crs_token);
                    string post_data2 = "csrfmiddlewaretoken=" + crs_token + "&OK=OK";
                    string finalhit = InstagramUser.globusHttpHelper.PostData_LoginThroughInstagram(new Uri("https://www.instagram.com/integrity/checkpoint/?next=%2F"), post_data2, "", crs_token);
                    dataBeforelogin = InstagramUser.globusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"), "");
                }
                try
                {
                    if (dataBeforelogin.Contains("Authorization Request &mdash; Instagram"))
                    {
                        Authorization = "No";
                    }
                    else
                    {
                        Authorization = "Yes";
                    }
                    if (dataBeforelogin.Contains("Please register your email address from") || dataBeforelogin.Contains("Please register your Phone Number from"))
                    {
                        if (dataBeforelogin.Contains("Please register your Phone Number from"))
                        {
                            acc_status = "Phone";
                        }
                        else
                        {
                            acc_status = "Email";
                        }

                    }
                    else
                    {
                        acc_status = "Ok";
                    }
                }
                catch(Exception ex)
                {
                    GlobusLogHelper.log.Error("Error :" + ex.StackTrace);
                }


                if (dataBeforelogin.Contains(InstagramUser.username.ToLower()) && !dataBeforelogin.Contains("lt-ie7 not-logged-in"))//marieturnipseed55614
                {
                    if (value)
                    {
                        GlobusLogHelper.log.Info("[ Logged in with Account Success :" + InstagramUser.username + " ]");
                    }
                    InstagramUser.isloggedin = true;
                    Status = "Success";
                    this.LoggedIn = true;
                    InstagramUser.LogInStatus = "Success";
                    string str = httpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/" + InstagramUser.username+"/"));
                   // UpdateCampaign(InstagramUser.username, str, "Success");
                    value = false;
                    
                }
                else
                {
                    InstagramUser.LogInStatus = "Fail";
                    string str = httpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/" + InstagramUser.username + "/"));
                   // UpdateCampaign(InstagramUser.username, str, "Fail");
                    GlobusLogHelper.log.Info("[ Logged in with Account Fail : " + InstagramUser.username + " ]");
                }
                return Status;
            }
               
            catch
            {
                return null;
            };
            

        }

        #region Commented Code To Login from Mobile
        //public void LoginUsingMobilePhotoUpload(ref InstagramUser InstagramUser)
        //{
        //    ///Sign In


        //    try
        //    {
        //        string guid = Utils.GenerateGuid();
        //        InstagramUser.guid = guid;
        //        string deviceId = "android-" + guid;

        //        string Data = "{\"device_id\":\"" + deviceId + "\",\"guid\":\"" + guid + "\",\"username\":\"" + InstagramUser.username + "\",\"password\":\"" + InstagramUser.password + "\",\"Content-Type\":\"application/x-www-form-urlencoded; charset=UTF-8\"}";

        //        Data = "{\"device_id\":\"" + deviceId + "\",\"guid\":\"" + guid + "\",\"username\":\"" + InstagramUser.username + "\",\"password\":\"" + InstagramUser.password + "\",\"Content-Type\":\"application/x-www-form-urlencoded; charset=UTF-8\"}";

        //        string Sig = Utils.GenerateSignature(Data);

        //        string Data_HttpUtility = HttpUtility.UrlEncode(Data);


        //        string NewData = "signed_body=" + Sig.ToLower() + "." + (Data_HttpUtility) + "&ig_sig_key_version=6";

        //        NewData = "signed_body=" + Sig + "." + Data_HttpUtility + "&ig_sig_key_version=6";

        //        string postUrlLogin = "https://i.instagram.com/api/v1/accounts/login/";


        //        string result = InstagramUser.globusHttpHelper.postFormDataForPicsUpload(new Uri(postUrlLogin), NewData);


        //        if (result.Contains(InstagramUser.username) && result.Contains("profile_pic_url"))
        //        {

        //            GlobusLogHelper.log.Info("[ Logged in with Account Success :" + InstagramUser.username + " ]");


        //            InstagramUser.isloggedinWithPhone = true;

        //        }
        //        else
        //        {
        //            InstagramUser.isloggedinWithPhone = false;
        //            GlobusLogHelper.log.Info("[ Logged in with Account Fail : " + InstagramUser.username + " ]");
        //        }

        //    }

        //    catch
        //    {

        //    };


        //} 
        #endregion

      
    }
}
