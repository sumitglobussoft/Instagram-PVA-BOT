using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Linq;

namespace BaseLib
{
    public class GlobusHttpHelper
    {
        public CookieCollection gCookies;
        public HttpWebRequest gRequest;
        public HttpWebResponse gResponse;
        public string responseURI = string.Empty;

        public static string UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0";

        public GlobusHttpHelper()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
        public string GetHrefFromStringForPagination(string pageSrcHtml)
        {
            string mainUrl = "http://www.zillow.com/";
            List<string> list = new List<string>();

            try
            {
                Chilkat.HtmlUtil obj = new Chilkat.HtmlUtil();

                Chilkat.StringArray dataImage = obj.GetHyperlinkedUrls(pageSrcHtml);


                string str = dataImage.GetString(dataImage.Length - 1);
                return str + "\">";

                //for (int i = 0; i < dataImage.Length; i++)
                //{
                //    try
                //    {
                //        string hreflink = dataImage.GetString(i);
                //        //list.Add(hreflink);
                //        list.Add(mainUrl + hreflink);
                //    }
                //    catch (Exception ex)
                //    {
                //        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                //    }

                //}
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return "";
        }


        /// <summary>
        /// Gets Html from a specified Url
        /// </summary>
        /// <param name="url">Http Url Link</param>
        /// <returns>HTML of the page in string format</returns>
        /// 
        public string getHtmlfromUrl(Uri url)
        {
            string responseString = string.Empty;
            try
            {
                //setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; rv:27.0) Gecko/20100101 Firefox/27.0";
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

                gRequest.KeepAlive = true;

                gRequest.AllowAutoRedirect = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                }

                if (this.gCookies == null)
                {
                    this.gCookies = new CookieCollection();
                }
               
              

                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    responseURI = gResponse.ResponseUri.AbsoluteUri;

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                if (ex.Message.Contains("The remote server returned an error: (999)"))
                {
                   // GlobusLogHelper.log.Info("Yahoo trace your IP ,Plz change your proxy IP");
                    responseString = "The remote server returned an error: (999)";
                }
            }
            return responseString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Http Url link</param>
        /// <param name="Referer"> Referer Url</param>
        /// <returns></returns>
        public string getHtmlfromUrl(Uri url,string Referer )
        {
            string responseString = string.Empty;
            try
            {
                //setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:26.0) Gecko/20100101 Firefox/26.0";
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

                gRequest.KeepAlive = true;

                gRequest.AllowAutoRedirect = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                gRequest.Referer = Referer;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                }

                if (this.gCookies == null)
                {
                    this.gCookies = new CookieCollection();
                }



                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    responseURI = gResponse.ResponseUri.AbsoluteUri;

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return responseString;
        }

        public string proxyAddress = string.Empty;
        public int port = 80;
        public string proxyUsername = string.Empty;
        public string proxyPassword = string.Empty;

        public string getHtmlfromUrlProxy(Uri url, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            string responseString = string.Empty;
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);
                //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";

                //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
                //gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                ////gRequest.Headers["Cache-Control"] = "max-age=0";
                //gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                ////gRequest.Connection = "keep-alive";
                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:26.0) Gecko/20100101 Firefox/26.0";
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                ///Set Proxy
                this.proxyAddress = proxyAddress;
                this.port = port;
                this.proxyUsername = proxyUsername;
                this.proxyPassword = proxyPassword;

                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;
              
                gRequest.Method = "GET";
                //gRequest.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);
                }
                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion
                   
                    responseURI = gResponse.ResponseUri.AbsoluteUri;

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch(Exception ex)
            {


                if (ex.Message.Contains("The remote server returned an error: (999)"))
                {
                    // GlobusLogHelper.log.Info("Yahoo trace your IP ,Plz change your proxy IP");
                    responseString = "The remote server returned an error: (999)";
                }
            }
            return responseString;
        }

        public bool HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {
            bool isUploadProfile = false;

            try
            {
                ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                gRequest.Method = "POST";
                gRequest.KeepAlive = true;
                gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0";
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }
                #endregion

                Stream rs = gRequest.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }

                #endregion

                WebResponse wresp = null;
                try
                {
                    wresp = gRequest.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                    // return true;
                    isUploadProfile = true;
                }
                catch (Exception ex)
                {
                    //log.Error("Error uploading file", ex);
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                    return false;
                }
                finally
                {
                    gRequest = null;
                }
                //}
            }
            catch (Exception ex)
            {
              
            }

            return isUploadProfile;

        }

        public bool MultiPartImageUpload(ref GlobusHttpHelper httpHelper, string Username, string Password, string localImagePath, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword)
        {
            /////Login to FB

            ////string valueLSD = "name=" + "\"lsd\"";

            int intProxyPort = 80;

            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
            {
                intProxyPort = int.Parse(proxyPort);
            }
            //string pageSource = string.Empty;
            //try
            //{
            //    pageSource = getHtmlfromUrlProxy(new Uri("https://www.facebook.com/login.php"), proxyAddress, intProxyPort, proxyUsername, proxyPassword);
            //    //int startIndex = pageSource.IndexOf(valueLSD) + 18;
            //}
            //catch { }
            //string value = GlobusHttpHelper.GetParamValue(pageSource, "lsd");
            string ResponseLogin = string.Empty;
            try
            {
            //    ResponseLogin = postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");

                ResponseLogin = httpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com"));
            }
            catch { }
            ///Setting Post Data Params...

            string userId = GlobusHttpHelper.Get_UserID(ResponseLogin);

            if (string.IsNullOrEmpty(userId) || userId == "0" || userId.Length < 3)
            {
                GlobusLogHelper.log.Info("Please Check The Account : " + Username);
                GlobusLogHelper.log.Debug("Please Check The Account : " + Username);

                return false;
            }

            string pgSrc_Profile = string.Empty;
            try
            {
                pgSrc_Profile = httpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/profile.php?id=" + userId + ""));
            }
            catch { }
            string profileSource = string.Empty;
            try
            {
                profileSource = httpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/ajax/timeline/profile_pic_selector.php?profile_id=" + userId + "&__a=1&__user=" + userId + ""));
            }
            catch { }


            //GlobusHttpHelper httpHelper = new GlobusHttpHelper();
            /////Get User ID
            //ProfileIDExtractor idExtracter = new ProfileIDExtractor();
            //idExtracter.ExtractFriendIDs(ref httpHelper, ref userId);


            string fb_dtsg = GlobusHttpHelper.GetParamValue(ResponseLogin, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
            if (string.IsNullOrEmpty(fb_dtsg))
            {
                fb_dtsg = GlobusHttpHelper.ParseJson(ResponseLogin, "fb_dtsg");
            }


            string last_action_id = GlobusHttpHelper.ParseJson(pgSrc_Profile, "last_action_id");

            if (!Utils.IsNumeric(last_action_id))
            {
                last_action_id = "0";
            }

            string postData = "last_action_id=" + last_action_id + "&fb_dtsg=" + fb_dtsg + "&__user=" + userId + "&phstamp=165816810252768712174";
            string res = string.Empty;
            try
            {
                res = httpHelper.postFormData(new Uri("https://www.facebook.com/ajax/mercury/thread_sync.php?__a=1"), postData);
            }
            catch { }
            NameValueCollection nvc = new NameValueCollection();
            //nvc.Add("post_form_id", post_form_id);
            nvc.Add("fb_dtsg", fb_dtsg);
            nvc.Add("id", userId);
            nvc.Add("type", "profile");
            //nvc.Add("return", "/ajax/profile/picture/upload_iframe.php?pic_type=1&id=" + userId);
            nvc.Add("return", "/ajax/timeline/profile_pic_upload.php?pic_type=1&id=" + userId);

            //UploadFilesToRemoteUrl("http://upload.facebook.com/pic_upload.php ", new string[] { @"C:\Users\Globus-n2\Desktop\Windows Photo Viewer Wallpaper.jpg" }, "", nvc);
            //HttpUploadFile("http://upload.facebook.com/pic_upload.php ", localImagePath, "file", "image/jpeg", nvc);
            if (HttpUploadFile("https://upload.facebook.com/pic_upload.php ", localImagePath, "pic", "image/jpeg", nvc, proxyAddress, intProxyPort, proxyUsername, proxyPassword))
            //if (HttpUploadFile("http://upload.facebook.com/pic_upload.php ", localImagePath, "file", "image/jpeg", nvc, proxyAddress, intProxyPort, proxyUsername, proxyPassword))
            {
                return true;
            }
            return false;

        }

        public bool AddaCover(ref GlobusHttpHelper HttpHelper, string Username, string Password, string localImagePath, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, ref string status)
        {
            bool isAddaCover = false;
            string fb_dtsg = string.Empty;
            string photo_id = string.Empty;
            string UsreId = string.Empty;

            try
            {

                string pageSource_Home = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

                UsreId = GlobusHttpHelper.GetParamValue(pageSource_Home, "user");
                if (string.IsNullOrEmpty(UsreId))
                {
                    UsreId = GlobusHttpHelper.ParseJson(pageSource_Home, "user");
                }

                fb_dtsg = GlobusHttpHelper.GetParamValue(pageSource_Home, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
                if (string.IsNullOrEmpty(fb_dtsg))
                {
                    fb_dtsg = GlobusHttpHelper.ParseJson(pageSource_Home, "fb_dtsg");
                }

                NameValueCollection nvc = new NameValueCollection();

                nvc.Add("fb_dtsg", fb_dtsg);
                //nvc.Add("filename=", fb_dtsg);
                nvc.Add("Content-Type:", "image/jpeg");

                string response = HttpUploadFile_AddaCover(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/timeline/cover/upload/", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                try
                {
                    string okay = HttpHelper.getHtmlfromUrl(new Uri("http://3-pct.channel.facebook.com/pull?channel=p_" + UsreId + "&seq=3&partition=69&clientid=70e140db&cb=8p7w&idle=8&state=active&mode=stream&format=json"));
                }
                catch
                {
                }

                if (!string.IsNullOrEmpty(response) && response.Contains("photo.php?fbid="))
                {
                    #region PostData_ForCoverPhotoSelect
                    //fb_dtsg=AQCLSjCH&photo_id=130869487061841&profile_id=100004163701035&photo_offset=0&video_id=&save=Save%20Changes&nctr[_mod]=pagelet_main_column_personal&__user=100004163701035&__a=1&phstamp=165816776831066772182 
                    #endregion
                    try
                    {
                        string photo_idValue = response.Substring(response.IndexOf("photo.php?fbid="), response.IndexOf(";", response.IndexOf("photo.php?fbid=")) - response.IndexOf("photo.php?fbid=")).Replace("photo.php?fbid=", string.Empty).Trim();
                        string[] arrphoto_idValue = Regex.Split(photo_idValue, "[^0-9]");

                        foreach (string item in arrphoto_idValue)
                        {
                            try
                            {
                                if (item.Length > 6)
                                {
                                    photo_id = item;
                                    break;
                                }
                            }
                            catch
                            {
                            }
                        }

                        string postData = "fb_dtsg=" + fb_dtsg + "&photo_id=" + photo_id + "&profile_id=" + UsreId + "&photo_offset=0&video_id=&save=Save%20Changes&nctr[_mod]=pagelet_main_column_personal&__user=" + UsreId + "&__a=1&phstamp=165816776831066772182 ";
                        string postResponse = HttpHelper.postFormData(new Uri("https://www.facebook.com/ajax/timeline/cover_photo_select.php"), postData);

                        if (!postResponse.Contains("error"))
                        {
                            //string ok = "ok";
                            isAddaCover = true;
                        }
                        if (string.IsNullOrEmpty(postResponse) || string.IsNullOrWhiteSpace(postResponse))
                        {
                            status = "Response Is Null !";
                        }
                        if (postResponse.Contains("errorSummary"))
                        {
                            string summary = GlobusHttpHelper.ParseJson(postResponse, "errorSummary");
                            string errorDescription = GlobusHttpHelper.ParseJson(postResponse, "errorDescription");

                            status = "Posting Error: " + summary + " | Error Description: " + errorDescription;
                            //FanPagePosterLogger("Posting Error: " + summary + " | Error Description: " + errorDescription);
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    if (response.Contains("Please choose an image that"))
                    {
                        status = "Please choose an image that's at least 399 pixels wide";
                    }

                }
            }
            catch
            {
            }
            return isAddaCover;
        }

        public string HttpUploadFile_AddaCover(ref GlobusHttpHelper HttpHelper, string userid, string url, string paramName, string contentType, string localImagePath, NameValueCollection nvc, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {

            #region PostData_ForUploadImage
            //-----------------------------68682554727644
            //Content-Disposition: form-data; name="fb_dtsg"

            //AQCLSjCH
            // -----------------------------68682554727644
            //Content-Disposition: form-data; name="pic"; filename="Hydrangeas.jpg"
            //Content-Type: image/jpeg

            //���� 
            #endregion


            bool isAddaCover = false;
            string responseStr = string.Empty;

            try
            {
                ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
                //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("");
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                gRequest.Referer = "Referer: https://www.facebook.com/profile.php?id=" + userid + "&ref=tn_tnmn";
                gRequest.Method = "POST";
                gRequest.KeepAlive = true;
                gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }
                #endregion

                Stream rs = gRequest.GetRequestStream();

                int tempi = 0;

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    string formitem = string.Empty;
                    if (tempi == 0)
                    {
                        byte[] firstboundarybytes = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                        rs.Write(firstboundarybytes, 0, firstboundarybytes.Length);
                        formitem = string.Format(formdataTemplate, key, nvc[key]);
                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                        rs.Write(formitembytes, 0, formitembytes.Length);
                        tempi++;
                        continue;
                    }
                    //rs.Write(boundarybytes, 0, boundarybytes.Length);
                    //formitem = string.Format(formdataTemplate, key, nvc[key]);
                    //byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    //rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, localImagePath, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();

                byte[] trailer3 = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                rs.Write(trailer3, 0, trailer3.Length);

                string trailerTemplate1 = "Content-Disposition: form-data; name=\"profile_id\"\r\n\r\n{0}\r\n";
                string trailer1 = string.Format(trailerTemplate1, userid);
                byte[] arrtrailer1 = System.Text.Encoding.UTF8.GetBytes(trailer1);
                rs.Write(arrtrailer1, 0, arrtrailer1.Length);

                byte[] trailer4 = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                rs.Write(trailer4, 0, trailer3.Length);

                string trailerTemplate2 = "Content-Disposition: form-data; name=\"source\"\r\n\r\n{0}\r\n";
                string trailer2 = string.Format(trailerTemplate2, "10");
                byte[] arrtrailer2 = System.Text.Encoding.UTF8.GetBytes(trailer2);
                rs.Write(arrtrailer2, 0, arrtrailer2.Length);

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }

                #endregion

                WebResponse wresp = null;
                try
                {
                    wresp = gRequest.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    responseStr = reader2.ReadToEnd();
                    //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                    return responseStr;
                }
                catch (Exception ex)
                {
                    //log.Error("Error uploading file", ex);
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                    // return false;
                }
            }
            catch
            {
            }

            finally
            {
                gRequest = null;
            }
            return responseStr;
        }

        public List<string> GetHrefsFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();

            try
            {
                var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
                var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
                foreach (Match url in regex.Matches(ModifiedString))
                {
                    lstUrl.Add(url.Value);
                }

                var regexhttps = new Regex(@"https://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
                var ModifiedStringHttps = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
                foreach (Match url in regexhttps.Matches(ModifiedStringHttps))
                {
                    lstUrl.Add(url.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.StackTrace);
            }

            return lstUrl;
        }

        public static string ParseEncodedJson(string data, string paramName)
        {
            try
            {
                data = data.Replace("&quot;", "\"");
                int startIndx = data.IndexOf("\"" + paramName + "\"") + ("\"" + paramName + "\"").Length + 1;
                int endIndx = data.IndexOf("\"", startIndx);

                string value = data.Substring(startIndx, endIndx - startIndx);
                value = value.Replace(",", "");
                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public string GetHtmlProxy_JSCSS(string URL, string proxyAddress, int port, string proxyUsername, string proxyPassword, string referer)
        {
            string responseString = string.Empty;
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(URL);
                //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";

                gRequest.UserAgent = UserAgent;
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                ///Set Proxy
                this.proxyAddress = proxyAddress;
                this.port = port;
                this.proxyUsername = proxyUsername;
                this.proxyPassword = proxyPassword;

                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    
                }

                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {

                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            return responseString;
        }

        public string postFormData(Uri formActionUrl, string postData)
        {
           // postData="charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=AVqEAf6F&locale=en_US&email=soni.sameer123%40rediffmail.com&pass=god@12345&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=AVqEAf6F";

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
         
            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:26.0) Gecko/20100101 Firefox/26.0";
            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";        
           // gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

        

            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public string postFormDataMobileLogin(Uri formActionUrl, string postData)
        {
            // postData="charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=AVqEAf6F&locale=en_US&email=soni.sameer123%40rediffmail.com&pass=god@12345&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=AVqEAf6F";

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);

            gRequest.UserAgent = "Instagram 6.21.2 Android (19/4.4.2; 480dpi; 1152x1920; Meizu; MX4; mx4; mt6595; en_US)";
            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            // gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            //   gRequest.KeepAlive = true;
            gRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            // gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            // gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";



            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.UTF8.GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public string postFormDataFromProxyMobileLogin(Uri formActionUrl, string postData, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            // postData="charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=AVqEAf6F&locale=en_US&email=soni.sameer123%40rediffmail.com&pass=god@12345&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=AVqEAf6F";

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);

            gRequest.UserAgent = "Instagram 6.21.2 Android (19/4.4.2; 480dpi; 1152x1920; Meizu; MX4; mx4; mt6595; en_US)";
            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            // gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            //   gRequest.KeepAlive = true;
            gRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            // gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            // gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";



            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.UTF8.GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public string postFormData(Uri formActionUrl, string postData, ref string responseStatus)
        {

            try
            {
                gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
                gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; rv:27.0) Gecko/20100101 Firefox/27.0";

                gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
                gRequest.Method = "POST";
                gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
                gRequest.KeepAlive = true;
                gRequest.ContentType = @"application/x-www-form-urlencoded";
              //  gRequest.Referer = "http://www.facebook.com/events/151218238374603/";
              
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagement
                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);
                }

                //logic to postdata to the form
                try
                {
                    setExpect100Continue();
                    string postdata = string.Format(postData);
                    byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                    gRequest.ContentLength = postBuffer.Length;
                    Stream postDataStream = gRequest.GetRequestStream();
                    postDataStream.Write(postBuffer, 0, postBuffer.Length);
                    postDataStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
                }
                //post data logic ends

                //Get Response for this request url
                try
                {
                    gResponse = (HttpWebResponse)gRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
                    responseStatus = ex.Message;
                }



                //check if the status code is http 200 or http ok

                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                    //check if response object has any cookies or not

                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion



                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    string responseString = reader.ReadToEnd();
                    reader.Close();
                    //Console.Write("Response String:" + responseString);
                    return responseString;
                }
                else
                {
                    return "Error in posting data";
                }
            }
            catch (Exception ex)
            {
                responseStatus = responseStatus + " : " + ex.Message;
                return "Error in posting data";
            }

        }

        public string postFormData(Uri formActionUrl, string postData, ref string responseStatus, string Refere)
        {

            try
            {
                gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
                gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; rv:27.0) Gecko/20100101 Firefox/27.0";

                gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
                gRequest.Method = "POST";
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.KeepAlive = true;
                // gRequest.ContentType = @"application/x-www-form-urlencoded";  // Content-Type: application/x-www-form-urlencoded; charset=UTF-8
               gRequest.ContentType = @"application/x-www-form-urlencoded; charset=UTF-8";
                gRequest.Referer = Refere;

                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
             
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagement
                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);
                }

                //logic to postdata to the form
                try
                {
                    setExpect100Continue();
                    string postdata = string.Format(postData);
                    byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                    gRequest.ContentLength = postBuffer.Length;
                    Stream postDataStream = gRequest.GetRequestStream();
                    postDataStream.Write(postBuffer, 0, postBuffer.Length);
                    postDataStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
                }
                //post data logic ends

                //Get Response for this request url
                try
                {
                    gResponse = (HttpWebResponse)gRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
                    responseStatus = ex.Message;
                }



                //check if the status code is http 200 or http ok

                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                    //check if response object has any cookies or not
                  
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion



                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    string responseString = reader.ReadToEnd();
                    reader.Close();
                    //Console.Write("Response String:" + responseString);
                    return responseString;
                }
                else
                {
                    return "Error in posting data";
                }
            }
            catch (Exception ex)
            {
                responseStatus = responseStatus + " : " + ex.Message;
                return "Error in posting data";
            }

        }

        public static string ParseJson(string data, string paramName)
        {
            try
            {
                int startIndx = data.IndexOf(paramName) + paramName.Length + 3;
                int endIndx = data.IndexOf("\"", startIndx);

                string value = data.Substring(startIndx, endIndx - startIndx);

                if (value.Contains("ion"))
                {
                    value = Utils.getBetween(data, "CurrentUserInitialData", "}").Replace("id", string.Empty).Replace("\"", string.Empty).Replace("[]", string.Empty).Replace(",",string.Empty).Replace("{",string.Empty).Replace(":",string.Empty);                                  
                }              

                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetAttachmentParamsUrlInfoUser(string data, string paramName)
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][user]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("attachment[params][urlInfo][user]"))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value =Uri.EscapeDataString(sss[0].Replace("e=", "").Replace("/", "").Replace("\"", "").Replace("\\", "%2F").Replace("http:", "http%3A"));
                    }
                    
                }
               
                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetAttachmentParamsUrlInfoCanonical(string data, string paramName)
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][canonical]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("value=") && s1_item.Contains("nical"))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = Uri.EscapeDataString(sss[0].Replace("value=", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace("nical]", ""));
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetAttachmentParamsUrlInfoFinal(string data, string paramName)
        
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][final]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("[urlInfo][final]"))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value =Uri.EscapeDataString(sss[0].Replace("ms][urlInfo][final]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", ""));
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetAttachmentParamsUrlInfoTitle(string data, string paramName)
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][final]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("ms][title]") && s1_item.Contains("attachment"))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = sss[0].Replace("ms][title]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", "");
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetAttachmentParamsSummary(string data, string paramName)
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][final]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("[summary]") )//&& s1_item.Contains("attachment"))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = Uri.EscapeDataString(sss[0].Replace("ms][summary]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", "").Replace("&#039;", ""));
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetAttachmentParamsMedium(string data, string paramName)
        {

            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][final]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("ms][medium]") && s1_item.Contains("attachment") )//&& s1_item.Contains("value="))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = sss[0].Replace("ms][medium]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", "").Replace("&#039;", "");
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetAttachmentParamsUrl(string data, string paramName)
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][final]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("ms][url]") && s1_item.Contains("attachment")&& s1_item.Contains("value="))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = Uri.EscapeDataString(sss[0].Replace("ms][url]", "").Replace("/", "%2F").Replace("\"", "").Replace("\\", "").Replace(" value=", "").Replace("&#039;", ""));
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetAttachmentType(string data, string paramName)
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][final]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("attachment[type]")) //&& s1_item.Contains("attachment") && s1_item.Contains("value="))
                    {
                        try
                        {
                            string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, "attachment");
                            string[] sss1 = System.Text.RegularExpressions.Regex.Split(sss[1], ">");
                            value = sss1[0].Replace("[type]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", "").Replace("&#039;", "");
                        }
                        catch(Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " +ex.StackTrace);
                        }
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }     
        public static string GetLinkMetricsSource(string data, string paramName)
        {

            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "link_metrics");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("[source]") && s1_item.Contains("value="))//&& s1_item.Contains("value="))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = sss[0].Replace("[source]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", "").Replace("&#039;", "");
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetLinkMetricsDomain(string data, string paramName)
        {

            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "link_metrics");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("[domain]") && s1_item.Contains("value="))//&& s1_item.Contains("value="))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = sss[0].Replace("[domain]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", "").Replace("&#039;", "");
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetLinkMetricsBaseDomain(string data, string paramName)
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "link_metrics");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("[base_domain]") && s1_item.Contains("value="))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = sss[0].Replace("[base_domain]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", "").Replace("&#039;", "");
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetlinkMetricsTitleLen(string data ,string paramName)
        {
            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "link_metrics");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("[title_len]") && s1_item.Contains("value="))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = sss[0].Replace("[title_len]", "").Replace("/", "").Replace("\"", "").Replace("\\", "").Replace(" value=", "").Replace("&#039;", "");
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetAttachmentParamsfavicon(string data, string paramName)
        {

            try
            {
                string value = string.Empty;
                string[] s1 = System.Text.RegularExpressions.Regex.Split(data, "[params][urlInfo][final]");
                foreach (var s1_item in s1)
                {
                    if (s1_item.Contains("ms][favicon]") && s1_item.Contains("value="))
                    {
                        string[] sss = System.Text.RegularExpressions.Regex.Split(s1_item, ">");

                        value = sss[0].Replace("ms][favicon]", "").Replace("/", "%2F").Replace("\"", " ").Replace("\\", "").Replace(" value=", "").Replace("&#039;", "").Replace(" ","");
                        value = Uri.EscapeDataString(value);
                    }

                }

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }


        public static string GetParamValue(string pgSrc, string paramName)
        {
            string valueparamName = string.Empty;
            try
            {
                if (pgSrc.Contains("name='" + paramName + "'"))
                {
                    string param = "name='" + paramName + "'";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=", startparamName) + "value=".Length + 1;
                    int endparamName = pgSrc.IndexOf("'", startparamName);
                    valueparamName = pgSrc.Substring(startparamName, endparamName - startparamName);
                    return valueparamName;
                }
                else if (pgSrc.Contains("name=\"" + paramName + "\""))
                {
                    string param = "name=\"" + paramName + "\"";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=", startparamName) + "value=".Length + 1;
                    int endcommentPostID = pgSrc.IndexOf("\"", startparamName);
                    valueparamName = pgSrc.Substring(startparamName, endcommentPostID - startparamName);
                    return valueparamName;
                }
                else if (paramName.Contains("user"))
                {
                       string value = string.Empty;
                
                        value = getBetween(pgSrc, "CurrentUserInitialData", "}").Replace("id", string.Empty).Replace("\"", string.Empty).Replace("[]", string.Empty).Replace(",", string.Empty).Replace("{", string.Empty).Replace(":", string.Empty);
                        return value;
                   
                }
                return null;
            }
            catch (Exception ex)
            {
                // GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return valueparamName;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static string Get_UserID(string pgSrc)
        {
            string UserID = GlobusHttpHelper.GetParamValue(pgSrc, "user");//pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
            if (string.IsNullOrEmpty(UserID))
            {
                UserID = GlobusHttpHelper.ParseJson(pgSrc, "user");
            }
            return UserID;
        }

        public static string Get_fb_dtsg(string pgSrc)
        {
            string fb_dtsg = GlobusHttpHelper.GetParamValue(pgSrc, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
            if (string.IsNullOrEmpty(fb_dtsg))
            {
                fb_dtsg = GlobusHttpHelper.ParseJson(pgSrc, "fb_dtsg");
            }
            return fb_dtsg;
        }

        public string postFormData(Uri formActionUrl, string postData, string referer)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            gRequest.UserAgent = UserAgent;
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            //gRequest.Headers["Cache-Control"] = "max-age=0";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded; charset=UTF-8";
            gRequest.Method = "POST";

            gRequest.KeepAlive = true;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            gRequest.CookieContainer = new CookieContainer();

            if (!string.IsNullOrEmpty(referer))
            {
                gRequest.Referer = referer;
            }

            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public string postFormDataProxy(Uri formActionUrl, string postData, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            string responseString = string.Empty;

            try
            {
                gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0";

                gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
                gRequest.Method = "POST";
                gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
                gRequest.KeepAlive = true;
                gRequest.ContentType = @"application/x-www-form-urlencoded";

                ///Modified BySumit 18-11-2011
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagement
                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);
                }

                //logic to postdata to the form
                try
                {
                    setExpect100Continue();
                    string postdata = string.Format(postData);
                    byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                    gRequest.ContentLength = postBuffer.Length;
                    Stream postDataStream = gRequest.GetRequestStream();
                    postDataStream.Write(postBuffer, 0, postBuffer.Length);
                    postDataStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
                }
                //post data logic ends

                //Get Response for this request url
                try
                {
                    gResponse = (HttpWebResponse)gRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
                }



                //check if the status code is http 200 or http ok

                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                    //check if response object has any cookies or not
                    //Added by sandeep pathak
                    //gCookiesContainer = gRequest.CookieContainer;  

                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion



                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    //Console.Write("Response String:" + responseString);

                }
                else
                {
                    return "Error in posting data";
                }
            }
            catch ( Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            return responseString;

        }

        public string postFormDataProxy(Uri formActionUrl, string postData, string Referes, string Token, string Origin, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            string responseString = string.Empty;

            try
            {
                gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0";

                gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
                gRequest.Method = "POST";
                gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
                gRequest.KeepAlive = true;
                gRequest.ContentType = @"application/x-www-form-urlencoded";
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                gRequest.Host = "www.instagram.com";
                

                if (!string.IsNullOrEmpty(Referes))
                {
                    gRequest.Referer = Referes;
                }
                if (!string.IsNullOrEmpty(Token))
                {
                    gRequest.Headers.Add("X-CSRFToken", "bc622723a802f92aea9fdd5ff6d31150");
                }
                if (!string.IsNullOrEmpty(Origin))
                {
                    gRequest.Headers.Add("Origin", Origin);
                }

                ///Modified BySumit 18-11-2011
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagement
                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);
                }

                //logic to postdata to the form
                try
                {
                    setExpect100Continue();
                    string postdata = string.Format(postData);
                    byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                    gRequest.ContentLength = postBuffer.Length;
                    Stream postDataStream = gRequest.GetRequestStream();
                    postDataStream.Write(postBuffer, 0, postBuffer.Length);
                    postDataStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
                }
                //post data logic ends

                //Get Response for this request url
                try
                {
                    gResponse = (HttpWebResponse)gRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
                }



                //check if the status code is http 200 or http ok

                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                    //check if response object has any cookies or not
                    //Added by sandeep pathak
                    //gCookiesContainer = gRequest.CookieContainer;  

                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion



                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    //Console.Write("Response String:" + responseString);

                }
                else
                {
                    return "Error in posting data";
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            return responseString;

        }

        public string postFormDataForInstaWithProxy(Uri formActionUrl, string postData, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            
            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
           
            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0";
            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            gRequest.UserAgent = UserAgent;            
            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";  //application/json, text/javascript, */*; q=0.01
            
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded; charset=UTF-8"; // https://www.linkedin.com/reg/captcha-challenge?submissionId=ad005a2c-8371-4ac7-9c94-0515e0b7a478&rt=AQEh33mEnbSHdwAAAVHUbzAaCIiH994lPHLyrSAzc1aQIaUQUjZzRjRKW-EXhNUMAXz6V1HAR4AGOr4Z389uSLcbK32JNz7PasJeYOGjZSlJQS0yAnux9LCr7l2U9CdI6E-RbfylTirZsQ46IKzeMSCKSSuBLxophBcsVEL8gIksX1hCgkXVpuF2SdHyDQmqDxqJgKXNpmUBJtmjxqNeDBE          
            gRequest.Headers.Add("Javascript-enabled", "true");
            gRequest.Referer = "https://www.instagram.com/?hl=en";
            gRequest.Headers.Add("X-CSRFToken", "bc622723a802f92aea9fdd5ff6d31150");
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);
           
            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public void ChangeProxy(string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            try
            {
                WebProxy myproxy = new WebProxy(proxyAddress, port);
                myproxy.BypassProxyOnLocal = false;

                if (!string.IsNullOrEmpty(proxyUsername) && !string.IsNullOrEmpty(proxyPassword))
                {
                    myproxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                }
                gRequest.Proxy = myproxy;
            }
            catch (Exception ex)
            {

            }

        }

        public void setExpect100Continue()
        {
            if (ServicePointManager.Expect100Continue == true)
            {
                ServicePointManager.Expect100Continue = false;
            }
        }

        public void setExpect100ContinueToTrue()
        {
            if (ServicePointManager.Expect100Continue == false)
            {
                ServicePointManager.Expect100Continue = true;
            }
        }

        public List<string> ExtractFriendIDs_URLSpecific(ref GlobusHttpHelper HttpHelper, ref string userID, string specificURL, ref List<string> lstFriend_Suggestions)
        {
            try
            {
                string pgSrc_HomePage = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));
                string ProFileURL = string.Empty;

                string UserId = string.Empty;

                #region Get User or Account ID
                if (pgSrc_HomePage.Contains("http://www.facebook.com/profile.php?id="))
                {
                    ///Modified Sumit [10-12-2011]
                    #region

                    int startIndx = pgSrc_HomePage.IndexOf("http://www.facebook.com/profile.php?id=");
                    int endIndx = pgSrc_HomePage.IndexOf("\"", startIndx + 1);
                    ProFileURL = pgSrc_HomePage.Substring(startIndx, endIndx - startIndx);
                    if (ProFileURL.Contains("&"))
                    {
                        string[] Arr = ProFileURL.Split('&');
                        ProFileURL = Arr[0];
                    }

                    #endregion
                }
                if (ProFileURL.Contains("http://www.facebook.com/profile.php?id="))
                {
                    UserId = ProFileURL.Replace("http://www.facebook.com/profile.php?id=", "");
                    if (UserId.Contains("&"))
                    {
                        UserId = UserId.Remove(UserId.IndexOf("&"));
                    }
                    //userID = UserId;
                }
                #endregion

                List<string> lstFriend_Requests = new List<string>();
                //List<string> lstFriend_Suggestions = new List<string>();

                string pgSrc_FriendsPage = HttpHelper.getHtmlfromUrl(new Uri(specificURL));

                ChilkatHttpHelpr chilkatHelpr = new ChilkatHttpHelpr();

                //List<string> aTags = chilkatHelpr.GetDataTag(pgSrc_FriendsPage, "a");

                //List<string> spanTags = chilkatHelpr.GetDataTag(pgSrc_FriendsPage, "span");

                List<string> requestProfileURLs_FR_Requests = chilkatHelpr.GetHrefsByTagAndAttributeName(pgSrc_FriendsPage, "span", "title fsl fwb fcb");
                lstFriend_Requests.AddRange(requestProfileURLs_FR_Requests);

                List<string> requestProfileURLs_FR_Suggestions = chilkatHelpr.GetElementsbyTagAndAttributeName(pgSrc_FriendsPage, "div", "title fsl fwb fcb", "id");
                lstFriend_Suggestions.AddRange(requestProfileURLs_FR_Suggestions);

                #region Old Code

                //if (pgSrc_FriendsPage.Contains("http://www.facebook.com/profile.php?id="))
                //{
                //    string[] arr = Regex.Split(pgSrc_FriendsPage, "href");
                //    foreach (string strhref in arr)
                //    {
                //        if (!strhref.Contains("<!DOCTYPE"))
                //        {
                //            if (strhref.Contains("profile.php?id"))
                //            {
                //                int startIndx = strhref.IndexOf("profile.php?id") + "profile.php?id".Length + 1;
                //                int endIndx = strhref.IndexOf("\"", startIndx);

                //                string profileID = strhref.Substring(startIndx, endIndx - startIndx);

                //                if (profileID.Contains("&"))
                //                {
                //                    profileID = profileID.Remove(profileID.IndexOf("&"));
                //                }
                //                if (profileID.Contains("\\"))
                //                {
                //                    profileID = profileID.Replace("\\", "");
                //                }
                //                lstFriend.Add(profileID);
                //            }
                //        }
                //    }
                //} 
                #endregion
                List<string> itemId = lstFriend_Requests.Distinct().ToList();
                return itemId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string ExtractIDUsingGraphAPI(string URL, ref GlobusHttpHelper HttpHelper)
        {
            string profileID = string.Empty;

            try
            {
                if (URL.Contains("id="))
                {
                    profileID = URL.Replace(URL.Remove(URL.IndexOf("id=") + "id=".Length), "");
                    return profileID;
                }

                int startIndx = URL.LastIndexOf("/");
                string name = URL.Substring(startIndx).Replace("/", "");

                string response = HttpHelper.getHtmlfromUrl(new Uri("https://graph.facebook.com/" + name));
                profileID = ParseJsonGraphAPI(response, "id");
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            return profileID;
        }

        private string ParseJsonGraphAPI(string data, string paramName)
        {
            try
            {
                int startIndx = data.IndexOf(paramName) + paramName.Length + 4;
                int endIndx = data.IndexOf("\"", startIndx);

                string value = data.Substring(startIndx, endIndx - startIndx);
                return value;
            }
            catch { return string.Empty; }

        }

        public string ExtractIDOfNonTimeLine(string URL, ref GlobusHttpHelper HttpHelper)
        {
            string id = "";
            try
            {
                string strURLPageSource = HttpHelper.getHtmlfromUrl(new Uri(URL));
                if (strURLPageSource.Contains("profile-picture-overlay"))
                {
                    string[] arrprofile_picture_overlay = Regex.Split(strURLPageSource, "profile-picture-overlay");
                    if (arrprofile_picture_overlay.Count() > 1)
                    {
                        if (arrprofile_picture_overlay[1].Contains("?set="))
                        {
                            string strid = arrprofile_picture_overlay[1].Substring(arrprofile_picture_overlay[1].IndexOf("?set="), (arrprofile_picture_overlay[1].IndexOf("&amp", arrprofile_picture_overlay[1].IndexOf("?set=")) - arrprofile_picture_overlay[1].IndexOf("?set="))).Replace("?set=", string.Empty).Replace(".", "/").Trim();
                            string[] arrId = Regex.Split(strid, "/");
                            id = arrId.Last();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return id;
        }

        public static string GetPageID(string PageSrcFanPageUrl, ref string FanpageUrl)
        {
            #region Get FB Page ID Modified

            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

            string fbpage_id = string.Empty;

            try
            {
                if (PageSrcFanPageUrl.Contains("profile_owner"))
                {
                    fbpage_id = ParseEncodedJsonPageID(PageSrcFanPageUrl, "profile_owner");
                }

                if (string.IsNullOrEmpty(fbpage_id))
                {
                    if (PageSrcFanPageUrl.Contains("/feeds/page.php?id="))
                    {
                        int startIndxFeeds = PageSrcFanPageUrl.IndexOf("/feeds/page.php?id=") + "/feeds/page.php?id=".Length;
                        int endIndxFeeds = PageSrcFanPageUrl.IndexOf("\"", startIndxFeeds);
                        fbpage_id = PageSrcFanPageUrl.Substring(startIndxFeeds, endIndxFeeds - startIndxFeeds);

                        if (fbpage_id.Contains("&amp"))
                        {
                            fbpage_id = fbpage_id.Remove(fbpage_id.IndexOf("&amp"));
                        }
                    }
                    else if (PageSrcFanPageUrl.Contains("page.php?id="))
                    {
                        int startIndxFeeds = PageSrcFanPageUrl.IndexOf("/page.php?id=") + "/page.php?id=".Length;
                        int endIndxFeeds = PageSrcFanPageUrl.IndexOf("\"", startIndxFeeds);
                        fbpage_id = PageSrcFanPageUrl.Substring(startIndxFeeds, endIndxFeeds - startIndxFeeds);

                        if (fbpage_id.Contains("&amp"))
                        {
                            fbpage_id = fbpage_id.Remove(fbpage_id.IndexOf("&amp"));
                        }
                    }
                    else if (PageSrcFanPageUrl.Contains("php?page_id="))
                    {
                        int startIndxFeeds = PageSrcFanPageUrl.IndexOf("php?page_id=") + "php?page_id=".Length;
                        int endIndxFeeds = PageSrcFanPageUrl.IndexOf("\"", startIndxFeeds);
                        fbpage_id = PageSrcFanPageUrl.Substring(startIndxFeeds, endIndxFeeds - startIndxFeeds);

                        if (fbpage_id.Contains("&amp"))
                        {
                            fbpage_id = fbpage_id.Remove(fbpage_id.IndexOf("&amp"));
                        }
                    }
                    else
                    {
                        //FanPagePosterLogger("Unable To like Fan Page Wall with " + Username + " and " + FanpageUrl);
                        //return;
                    }


                    if (string.IsNullOrEmpty(fbpage_id))
                    {
                        fbpage_id = HttpHelper.ExtractIDUsingGraphAPI(FanpageUrl, ref HttpHelper);
                    }

                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            return fbpage_id;

            #endregion
        }

        public static string ParseEncodedJsonPageID(string data, string paramName)
        {
            try
            {
                data = data.Replace("&quot;", "\"");

                string value = ParseJson(data, paramName);

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public string HttpUploadPictureForWall(ref GlobusHttpHelper HttpHelper, string userid, string url, string paramName, string contentType, string localImagePath, NameValueCollection nvc, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword, string picfilepath)
        {

            #region PostData_ForUploadImage
            string localImagePath1 = localImagePath.Replace(picfilepath, string.Empty).Replace("\\", string.Empty);

            #endregion


            bool isAddaCover = false;
            string responseStr = string.Empty;

            try
            {
                ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
                //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("");
                string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString();
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                //gRequest.Referer = "Referer: https://www.facebook.com/profile.php?id=" + userid + "&ref=tn_tnmn";
                gRequest.Referer = "Referer: https://www.facebook.com/home.php";

                gRequest.Method = "POST";
                gRequest.KeepAlive = true;
                gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }
                #endregion

                Stream rs = gRequest.GetRequestStream();


                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, localImagePath, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();
                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }

                #endregion

                WebResponse wresp = null;
                try
                {
                    wresp = gRequest.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    responseStr = reader2.ReadToEnd();
                    return responseStr;
                    //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                    //return true;
                }
                catch (Exception ex)
                {
                    //log.Error("Error uploading file", ex);
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                    // return false;


                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);

                }
                finally
                {
                    gRequest = null;
                }
                return responseStr;

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return responseStr;
        }

        public string HttpUploadFileInstaPicWithProxy(string url, string paramName, string contentType, string localImagePath, NameValueCollection nvc, string proxyAddress, int proxyPort, string proxyUsername,string proxyPassword)
        {

            bool isAddaCover = false;
            string responseStr = string.Empty;

            try
            {
                ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
                //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("");
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();//"-----------------------------" + DateTime.Now.Ticks.ToString();
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                //gRequest.Referer = "Referer: https://www.facebook.com/profile.php?id=" + userid + "&ref=tn_tnmn";
                // gRequest.Referer = referer;// "http://www.facebook.com/?sk=welcome";

                gRequest.Method = "POST";
                gRequest.KeepAlive = true;
                gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

                //  gRequest.Headers.Add("X-SVN-Rev", "827944");
                gRequest.UserAgent = "Instagram 6.21.2 Android (19/4.4.2; 480dpi; 1152x1920; Meizu; MX4; mx4; mt6595; en_US)";
                // gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }
                #endregion

                using (Stream rs = gRequest.GetRequestStream())
                {
                    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    foreach (string key in nvc.Keys)
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, key, nvc[key]);
                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                        rs.Write(formitembytes, 0, formitembytes.Length);
                    }
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string fileName = Path.GetFileName(localImagePath);

                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    string header = string.Format(headerTemplate, paramName, fileName, contentType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                    rs.Write(headerbytes, 0, headerbytes.Length);

                    using (FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            rs.Write(buffer, 0, bytesRead);
                        }
                    }
                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    rs.Write(trailer, 0, trailer.Length);
                }

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }

                #endregion

                WebResponse wresp = null;
                try
                {
                    wresp = gRequest.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    using (StreamReader reader2 = new StreamReader(stream2))
                    {
                        responseStr = reader2.ReadToEnd();
                    }
                    return responseStr;
                    //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                    //return true;
                }
                catch (Exception ex)
                {
                    //log.Error("Error uploading file", ex);
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                    // return false;
                }
                finally
                {
                    gRequest = null;
                }
                return responseStr;

            }
            catch { }
            return responseStr;
        }

        public byte[] getImgfromUrl(Uri url)
        {
            byte[] data = null;
            string responseString = string.Empty;
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);

                gRequest.Timeout = 2 * 45000;
                gRequest.UserAgent = UserAgent;
                gRequest.Accept = "image/png,image/*;q=0.8,*/*;q=0.5";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";

                gRequest.KeepAlive = true;

                gRequest.AllowAutoRedirect = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    setExpect100Continue();
                    gRequest.CookieContainer.Add(gCookies);

                    try
                    {
                        //gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0 - 2078004405 - 1321685323158", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1322116799.1322206824.9", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                        //gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //Get Response for this request url

                setExpect100Continue();
                gResponse = (HttpWebResponse)gRequest.GetResponse();

                //check if the status code is http 200 or http ok
                if (gResponse.StatusCode == HttpStatusCode.OK)
                {
                    //get all the cookies from the current request and add them to the response object cookies
                    setExpect100Continue();
                    gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                    //check if response object has any cookies or not
                    if (gResponse.Cookies.Count > 0)
                    {
                        //check if this is the first request/response, if this is the response of first request gCookies
                        //will be null
                        if (this.gCookies == null)
                        {
                            gCookies = gResponse.Cookies;
                        }
                        else
                        {
                            foreach (Cookie oRespCookie in gResponse.Cookies)
                            {
                                bool bMatch = false;
                                foreach (Cookie oReqCookie in this.gCookies)
                                {
                                    if (oReqCookie.Name == oRespCookie.Name)
                                    {
                                        oReqCookie.Value = oRespCookie.Value;
                                        bMatch = true;
                                        break; // 
                                    }
                                }
                                if (!bMatch)
                                    this.gCookies.Add(oRespCookie);
                            }
                        }
                    }
                #endregion

                    //StreamReader reader = new StreamReader(gResponse.GetResponseStream());

                    MemoryStream ms = new MemoryStream();
                    gResponse.GetResponseStream().CopyTo(ms);
                    // If you need it...
                    data = ms.ToArray();

                    return data;

                    //responseString = reader.ReadToEnd();
                    //reader.Close();
                    //return responseString;
                }
                else
                {
                    return data;
                }
            }
            catch
            {
            }
            return data;
        }



        public string response_ImageUpload = string.Empty;

        public string error_ImageUpload = string.Empty;

       

       //public bool AddaPicture(ref GlobusHttpHelper HttpHelper, string Username, string Password, string localImagePath, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, string targeturl, string message, ref string status, string pageSource_Home, string xhpc_targetid, string xhpc_composerid, string message_text, string fb_dtsg, string UsreId, string pageSource)              
       // {

       //    // string pageSource = HttpHelper.getHtmlfromUrl(new Uri(targeturl));
       //     int tempCount = 0;
       //   startAgain:

       //     bool isSentPicMessage = false;
       //     //string fb_dtsg = string.Empty;
       //     string photo_id = string.Empty;
       //     //string UsreId = string.Empty;
       //     //xhpc_composerid = string.Empty;
       //     //xhpc_targetid = string.Empty;
       //     //message_text = string.Empty;

       //     try
       //     {
       //         #region commentedCode


       //         //string pageSource_Home = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

       //         //UsreId = GlobusHttpHelper.GetParamValue(pageSource_Home, "user");
       //         //if (string.IsNullOrEmpty(UsreId))
       //         //{
       //         //    UsreId = GlobusHttpHelper.ParseJson(pageSource_Home, "user");
       //         //}

       //         //fb_dtsg = GlobusHttpHelper.GetParamValue(pageSource_Home, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
       //         //if (string.IsNullOrEmpty(fb_dtsg))
       //         //{
       //         //    fb_dtsg = GlobusHttpHelper.ParseJson(pageSource_Home, "fb_dtsg");
       //         //}


       //         //string pageSource_HomeData = HttpHelper.getHtmlfromUrl(new Uri(targeturl));
       //         //xhpc_composerid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "composerid");
       //         //xhpc_targetid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "xhpc_targetid"); 
       //         #endregion

       //         string composer_session_id = "";

       //         string tempresponse1 = "";
       //         ///temp post
       //         {
       //             string source = "";
       //             string profile_id = "";
       //             string gridID = "";
       //             string qn = string.Empty;

       //             try
       //             {
       //                 string Url = "https://www.facebook.com/ajax/composerx/attachment/media/upload/?composerurihash=1";
       //                 string posturl1 ="fb_dtsg=" + fb_dtsg + "&composerid=" + xhpc_composerid + "&targetid=" + xhpc_targetid + "&loaded_components[0]=maininput&loaded_components[1]=cameraicon&loaded_components[2]=withtaggericon&loaded_components[3]=placetaggericon&loaded_components[4]=mainprivacywidget&loaded_components[5]=cameraicon&loaded_components[6]=mainprivacywidget&loaded_components[7]=withtaggericon&loaded_components[8]=placetaggericon&loaded_components[9]=maininput&nctr[_mod]=pagelet_group_composer&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=i&phstamp=16581688688747595501";    //"fb_dtsg=" + fb_dtsg + "&composerid=" + xhpc_composerid + "&targetid=" + xhpc_targetid + "&istimeline=1&timelinelocation=composer&loaded_components[0]=maininput&loaded_components[1]=mainprivacywidget&loaded_components[2]=mainprivacywidget&loaded_components[3]=maininput&loaded_components[4]=explicitplaceinput&loaded_components[5]=hiddenplaceinput&loaded_components[6]=placenameinput&loaded_components[7]=hiddensessionid&loaded_components[8]=withtagger&loaded_components[9]=backdatepicker&loaded_components[10]=placetagger&loaded_components[11]=withtaggericon&loaded_components[12]=backdateicon&loaded_components[13]=citysharericon&nctr[_mod]=pagelet_timeline_recent&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=18&phstamp=1658168111112559866679";
       //                 // string PostUrl = "city_id=" + CityIDS1 + "&city_page_id=" + city_page_id + "&city_name=" + CityName1 + "&is_default=false&session_id=1362404125&__user=" + UsreId + "&__a=1&__dyn=798aD5z5ynU&__req=z&fb_dtsg=" + fb_dtsg + "&phstamp=1658168111112559866165";
       //                 string res11 =HttpHelper.postFormData(new Uri(Url), posturl1);


       //                 try
       //                 {
       //                     source = res11.Substring(res11.IndexOf("source\":"), (res11.IndexOf(",", res11.IndexOf("source\":")) - res11.IndexOf("source\":"))).Replace("source\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
       //                 }
       //                 catch { }
       //                 try
       //                 {
       //                     profile_id = res11.Substring(res11.IndexOf("profile_id\":"), (res11.IndexOf("}", res11.IndexOf("profile_id\":")) - res11.IndexOf("profile_id\":"))).Replace("profile_id\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
       //                     if(profile_id.Contains(","))
       //                     {
       //                         profile_id = ParseEncodedJson(res11, "profile_id");
       //                     }
       //                     //"gridID":
       //                 }
       //                 catch { }
       //                 try
       //                 {
       //                     gridID = res11.Substring(res11.IndexOf("gridID\":"), (res11.IndexOf(",", res11.IndexOf("gridID\":")) - res11.IndexOf("gridID\":"))).Replace("gridID\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
       //                 }
       //                 catch { }
       //                 try
       //                 {
       //                     composer_session_id = res11.Substring(res11.IndexOf("composer_session_id\":"), (res11.IndexOf("}", res11.IndexOf("composer_session_id\":")) - res11.IndexOf("composer_session_id\":"))).Replace("composer_session_id\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
       //                 }
       //                 catch { }
                     
       //                 try
       //                 {
       //                     if (string.IsNullOrEmpty(composer_session_id))
       //                     {
       //                         composer_session_id = res11.Substring(res11.IndexOf("composerID\":"), (res11.IndexOf("}", res11.IndexOf("composerID\":")) - res11.IndexOf("composerID\":"))).Replace("composerID\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();

       //                     }
       //                 }
       //                 catch { }

       //                 try
       //                 {
       //                     qn = getBetween(res11, "qn\\\" value=\\\"", "\\\" \\/>");
       //                 }
       //                 catch { }
       //             }
       //             catch { }

       //             NameValueCollection nvc1 = new NameValueCollection();
       //             try
       //             {
       //                 //message = Uri.EscapeDataString(message);
       //             }
       //             catch { }

       //             //xhpc_composerid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "composerid");
       //             //xhpc_targetid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "xhpc_targetid");
       //             //-------------------------------
       //             //nvc1.Add("fb_dtsg", fb_dtsg);
       //             //nvc1.Add("source", source);
       //             //nvc1.Add("profile_id", profile_id);
       //             //nvc1.Add("grid_id", gridID);
       //             //nvc1.Add("upload_id", "1024");
       //             //-----------------------------------
       //             nvc1.Add("fb_dtsg", fb_dtsg);
       //             nvc1.Add("source", source);
       //             nvc1.Add("profile_id", profile_id);
       //             nvc1.Add("grid_id", gridID);
       //             nvc1.Add("upload_id", "1024");
       //             nvc1.Add("qn", qn);

       //             //nvc1.Add("fb_dtsg", fb_dtsg);
       //             //nvc1.Add("source", source);
       //             //nvc1.Add("profile_id", profile_id);
       //             //nvc1.Add("grid_id", gridID);
       //             //nvc1.Add("upload_id", "1024");
       //             //nvc1.Add("qn", qn);

       //             string _rev = getBetween(pageSource, "svn_rev", ",");
       //             _rev = _rev.Replace("\":", string.Empty);


       //             string uploadURL = "https://upload.facebook.com/ajax/composerx/attachment/media/saveunpublished?target_id="+xhpc_targetid+"&__user="+UsreId+"&__a=1&__dyn=7n88Oq9ccmqDxl2u5Fa8HzCqm5Aqbx2mbAKGiBAGm&__req=1t&fb_dtsg="+fb_dtsg+"&__rev="+_rev+"";
       //             tempresponse1 = HttpUploadFile_UploadPic_temp(ref HttpHelper, UsreId, uploadURL, "composer_unpublished_photo[]", "image/jpeg", localImagePath, nvc1, "", proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

       //             if (tempresponse1.ToLower().Contains("errorsummary") && tempresponse1.ToLower().Contains("There was a problem with this request. We're working on getting it fixed as soon as we can".ToLower()))
       //             {
       //                 if (tempCount < 2)
       //                 {
       //                     System.Threading.Thread.Sleep(15000);
       //                     tempCount++;
       //                     goto startAgain;
       //                 }
       //                 else
       //                 {
       //                     return false;
       //                 }
       //             }
                    
       //             //string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/timeline/cover/upload/", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

       //             //string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

       //             //tempresponse1 = HttpUploadFile_UploadPic_temp(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/composerx/attachment/media/saveunpublished?target_id=" + xhpc_targetid + "&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=l&fb_dtsg=" + fb_dtsg + "", "composer_unpublished_photo[]", "image/jpeg", localImagePath, nvc1, "", proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

       //         }

       //         NameValueCollection nvc = new NameValueCollection();
       //         try
       //         {
       //             //message = Uri.EscapeDataString(message);
       //         }
       //         catch { }
       //         nvc.Add("fb_dtsg", fb_dtsg);
       //         nvc.Add("xhpc_targetid", xhpc_targetid);
       //         nvc.Add("xhpc_context", "profile");
       //         nvc.Add("xhpc_ismeta", "1");
       //         nvc.Add("xhpc_fbx", "1");
       //         nvc.Add("xhpc_timeline", "");
       //         nvc.Add("xhpc_composerid", xhpc_composerid);
       //         nvc.Add("xhpc_message_text", message);
       //         nvc.Add("xhpc_message", message);
       //         //nvc.Add("name", "file1");
       //         //nvc.Add("Content-Type:", "image/jpeg");
       //         //nvc.Add("filename=", "");



       //         string composer_unpublished_photo = "";
       //         string start_composer_unpublished_photo = Regex.Split(tempresponse1, "},\"")[1];// 
       //         int startIndex_composer_unpublished_photo = start_composer_unpublished_photo.IndexOf(",\"") + ",\"".Length;
       //         int endIndex_composer_unpublished_photo = start_composer_unpublished_photo.IndexOf("\"", startIndex_composer_unpublished_photo + 1);

       //         composer_unpublished_photo = start_composer_unpublished_photo.Substring(startIndex_composer_unpublished_photo, endIndex_composer_unpublished_photo - startIndex_composer_unpublished_photo);

       //         ///New test upload pic post
       //         string waterfallid = GlobusHttpHelper.ParseJson(pageSource_Home, "waterfallID");

       //         string newpostURL = "https://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=r&fb_dtsg=" + fb_dtsg + "";
       //         string newPostData = "";


       //         NameValueCollection newnvc = new NameValueCollection();
       //         try
       //         {
       //             //message = Uri.EscapeDataString(message);
       //         }
       //         catch { }
       //         newnvc.Add("fb_dtsg", fb_dtsg);
       //         newnvc.Add("xhpc_targetid", xhpc_targetid);
       //         newnvc.Add("xhpc_context", "profile");
       //         newnvc.Add("xhpc_ismeta", "1");
       //         newnvc.Add("xhpc_fbx", "1");
       //         newnvc.Add("xhpc_timeline", "");
       //         newnvc.Add("xhpc_composerid", xhpc_composerid);
       //         newnvc.Add("xhpc_message_text", message);
       //         newnvc.Add("xhpc_message", message);

       //         newnvc.Add("composer_unpublished_photo[]", composer_unpublished_photo);
       //         newnvc.Add("album_type", "128");
       //         newnvc.Add("is_file_form", "1");
       //         newnvc.Add("oid", "");
       //         newnvc.Add("qn", waterfallid);
       //         newnvc.Add("application", "composer");
       //         newnvc.Add("is_explicit_place", "");
       //         newnvc.Add("composertags_place", "");
       //         newnvc.Add("composertags_place_name", "");
       //         newnvc.Add("composer_session_id", composer_session_id);
       //         newnvc.Add("composertags_city", "");
       //         newnvc.Add("vzdisable_location_sharing", "false");
       //         newnvc.Add("composer_predicted_city", "");

       //        // string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/timeline/cover/upload/", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

       //        // string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

       //         string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, newpostURL, "file1", "image/jpeg", localImagePath, newnvc, targeturl, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);//HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88O49ccm9o-2Ki&__req=1c&fb_dtsg=" + fb_dtsg + "", "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);



       //         //http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=100004608395129
       //         if (string.IsNullOrEmpty(response))
       //         {
       //             try
       //             {
       //                 //response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);
       //                 response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88O49ccm9o-2Ki&__req=1c&fb_dtsg=" + fb_dtsg + "", "file1", "image/jpeg", localImagePath, nvc, targeturl, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

       //             }
       //             catch { }
       //         }
       //         string posturl = "https://www.facebook.com/ajax/places/city_sharer_reset.php";
       //         string postdata = "__user=" + UsreId + "&__a=1&fb_dtsg=" + fb_dtsg + "&phstamp=1658167761111108210145";
       //         string responsestring = HttpHelper.postFormData(new Uri(posturl), postdata);
       //         try
       //         {
       //             string okay = HttpHelper.getHtmlfromUrl(new Uri("https://3-pct.channel.facebook.com/pull?channel=p_" + UsreId + "&seq=3&partition=69&clientid=70e140db&cb=8p7w&idle=8&state=active&mode=stream&format=json"));
       //         }
       //         catch
       //         {
       //         }

       //         if (!string.IsNullOrEmpty(response) && response.Contains("photo.php?fbid="))
       //         {

       //             #region PostData_ForCoverPhotoSelect
       //             //fb_dtsg=AQCLSjCH&photo_id=130869487061841&profile_id=100004163701035&photo_offset=0&video_id=&save=Save%20Changes&nctr[_mod]=pagelet_main_column_personal&__user=100004163701035&__a=1&phstamp=165816776831066772182 
       //             #endregion

       //             try
       //             {

       //                 if (!response.Contains("errorSummary") || !response.Contains("error"))
       //                 {
       //                     isSentPicMessage = true;
       //                 }
       //                 if (response.Contains("Your post has been submitted and is pending approval by an admin"))
       //                 {
       //                     GlobusLogHelper.log.Debug("Your post has been submitted and is pending approval by an admin." + "GroupUrl >>>" + targeturl );
       //                     GlobusLogHelper.log.Info("Your post has been submitted and is pending approval by an admin." + "GroupUrl >>>" + targeturl);
       //                 }
       //             }
       //             catch { }
       //             #region CodeCommented
       //             //    string photo_idValue = response.Substring(response.IndexOf("photo.php?fbid="), response.IndexOf(";", response.IndexOf("photo.php?fbid=")) - response.IndexOf("photo.php?fbid=")).Replace("photo.php?fbid=", string.Empty).Trim();
       //             //    string[] arrphoto_idValue = Regex.Split(photo_idValue, "[^0-9]");

       //             //    foreach (string item in arrphoto_idValue)
       //             //    {
       //             //        try
       //             //        {
       //             //            if (item.Length > 6)
       //             //            {
       //             //                photo_id = item;
       //             //                break;
       //             //            }
       //             //        }
       //             //        catch
       //             //        {
       //             //        }
       //             //    }

       //             //   // string postData = "fb_dtsg=" + fb_dtsg + "&photo_id=" + photo_id + "&profile_id=" + UsreId + "&photo_offset=0&video_id=&save=Save%20Changes&nctr[_mod]=pagelet_main_column_personal&__user=" + UsreId + "&__a=1&phstamp=165816776831066772182 ";
       //             //   // string postResponse = HttpHelper.postFormData(new Uri("https://www.facebook.com/ajax/timeline/cover_photo_select.php"), postData);

       //             //    //if (!postResponse.Contains("error"))
       //             //    //{
       //             //    //    //string ok = "ok";
       //             //    //    isSentPicMessage = true;
       //             //    //}
       //             //    //if (string.IsNullOrEmpty(postResponse) || string.IsNullOrWhiteSpace(postResponse))
       //             //    //{
       //             //    //    status = "Response Is Null !";
       //             //    //}
       //             //    //if (postResponse.Contains("errorSummary"))
       //             //    //{
       //             //    //    string summary = GlobusHttpHelper.ParseJson(postResponse, "errorSummary");
       //             //    //    string errorDescription = GlobusHttpHelper.ParseJson(postResponse, "errorDescription");

       //             //    //    status = "Posting Error: " + summary + " | Error Description: " + errorDescription;
       //             //    //    //FanPagePosterLogger("Posting Error: " + summary + " | Error Description: " + errorDescription);
       //             //    //}
       //             //}
       //             //catch
       //             //{
       //             //} 
       //             #endregion
       //         }
       //     }
       //     catch
       //     {
       //     }
       //     return isSentPicMessage;
       // }

       public bool AddaPicture(ref GlobusHttpHelper HttpHelper, string Username, string Password, string localImagePath, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, string targeturl, string message, ref string status, string pageSource_Home, string xhpc_targetid, string xhpc_composerid, string message_text, string fb_dtsg, string UsreId, string pageSource, ref int tempCountMain)
       {

           // string pageSource = HttpHelper.getHtmlfromUrl(new Uri(targeturl));
           int tempCount = 0;
       startAgain:

           bool isSentPicMessage = false;
           //string fb_dtsg = string.Empty;
           string photo_id = string.Empty;
           //string UsreId = string.Empty;
           //xhpc_composerid = string.Empty;
           //xhpc_targetid = string.Empty;
           //message_text = string.Empty;

           try
           {
               #region commentedCode


               //string pageSource_Home = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

               //UsreId = GlobusHttpHelper.GetParamValue(pageSource_Home, "user");
               //if (string.IsNullOrEmpty(UsreId))
               //{
               //    UsreId = GlobusHttpHelper.ParseJson(pageSource_Home, "user");
               //}

               //fb_dtsg = GlobusHttpHelper.GetParamValue(pageSource_Home, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
               //if (string.IsNullOrEmpty(fb_dtsg))
               //{
               //    fb_dtsg = GlobusHttpHelper.ParseJson(pageSource_Home, "fb_dtsg");
               //}


               //string pageSource_HomeData = HttpHelper.getHtmlfromUrl(new Uri(targeturl));
               //xhpc_composerid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "composerid");
               //xhpc_targetid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "xhpc_targetid"); 
               #endregion

               string composer_session_id = "";

               string tempresponse1 = "";
               ///temp post
               {
                   string source = "";
                   string profile_id = "";
                   string gridID = "";
                   string qn = string.Empty;

                   try
                   {
                       string Url = "https://www.facebook.com/ajax/composerx/attachment/media/upload/?composerurihash=1";
                       string posturl1 = "fb_dtsg=" + fb_dtsg + "&composerid=" + xhpc_composerid + "&targetid=" + xhpc_targetid + "&loaded_components[0]=maininput&loaded_components[1]=cameraicon&loaded_components[2]=withtaggericon&loaded_components[3]=placetaggericon&loaded_components[4]=mainprivacywidget&loaded_components[5]=cameraicon&loaded_components[6]=mainprivacywidget&loaded_components[7]=withtaggericon&loaded_components[8]=placetaggericon&loaded_components[9]=maininput&nctr[_mod]=pagelet_group_composer&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=i&phstamp=16581688688747595501";    //"fb_dtsg=" + fb_dtsg + "&composerid=" + xhpc_composerid + "&targetid=" + xhpc_targetid + "&istimeline=1&timelinelocation=composer&loaded_components[0]=maininput&loaded_components[1]=mainprivacywidget&loaded_components[2]=mainprivacywidget&loaded_components[3]=maininput&loaded_components[4]=explicitplaceinput&loaded_components[5]=hiddenplaceinput&loaded_components[6]=placenameinput&loaded_components[7]=hiddensessionid&loaded_components[8]=withtagger&loaded_components[9]=backdatepicker&loaded_components[10]=placetagger&loaded_components[11]=withtaggericon&loaded_components[12]=backdateicon&loaded_components[13]=citysharericon&nctr[_mod]=pagelet_timeline_recent&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=18&phstamp=1658168111112559866679";
                       // string PostUrl = "city_id=" + CityIDS1 + "&city_page_id=" + city_page_id + "&city_name=" + CityName1 + "&is_default=false&session_id=1362404125&__user=" + UsreId + "&__a=1&__dyn=798aD5z5ynU&__req=z&fb_dtsg=" + fb_dtsg + "&phstamp=1658168111112559866165";
                       string res11 = HttpHelper.postFormData(new Uri(Url), posturl1);


                       try
                       {
                           source = res11.Substring(res11.IndexOf("source\":"), (res11.IndexOf(",", res11.IndexOf("source\":")) - res11.IndexOf("source\":"))).Replace("source\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                       }
                       catch { }
                       try
                       {
                           profile_id = res11.Substring(res11.IndexOf("profile_id\":"), (res11.IndexOf("}", res11.IndexOf("profile_id\":")) - res11.IndexOf("profile_id\":"))).Replace("profile_id\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                           if (profile_id.Contains(","))
                           {
                               profile_id = ParseEncodedJson(res11, "profile_id");
                           }
                           //"gridID":
                       }
                       catch { }
                       try
                       {
                           gridID = res11.Substring(res11.IndexOf("gridID\":"), (res11.IndexOf(",", res11.IndexOf("gridID\":")) - res11.IndexOf("gridID\":"))).Replace("gridID\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                       }
                       catch { }
                       try
                       {
                           composer_session_id = res11.Substring(res11.IndexOf("composer_session_id\":"), (res11.IndexOf("}", res11.IndexOf("composer_session_id\":")) - res11.IndexOf("composer_session_id\":"))).Replace("composer_session_id\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                       }
                       catch { }

                       try
                       {
                           if (string.IsNullOrEmpty(composer_session_id))
                           {
                               composer_session_id = res11.Substring(res11.IndexOf("composerID\":"), (res11.IndexOf("}", res11.IndexOf("composerID\":")) - res11.IndexOf("composerID\":"))).Replace("composerID\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();

                           }
                       }
                       catch { }

                       try
                       {
                           qn = getBetween(res11, "qn\\\" value=\\\"", "\\\" \\/>");
                       }
                       catch { }
                   }
                   catch { }

                   NameValueCollection nvc1 = new NameValueCollection();
                   try
                   {
                       //message = Uri.EscapeDataString(message);
                   }
                   catch { }

                   //xhpc_composerid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "composerid");
                   //xhpc_targetid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "xhpc_targetid");
                   //-------------------------------
                   //nvc1.Add("fb_dtsg", fb_dtsg);
                   //nvc1.Add("source", source);
                   //nvc1.Add("profile_id", profile_id);
                   //nvc1.Add("grid_id", gridID);
                   //nvc1.Add("upload_id", "1024");
                   //-----------------------------------
                   nvc1.Add("fb_dtsg", fb_dtsg);
                   nvc1.Add("source", source);
                   nvc1.Add("profile_id", profile_id);
                   nvc1.Add("grid_id", gridID);
                   nvc1.Add("upload_id", "1024");
                   nvc1.Add("qn", qn);

                   //nvc1.Add("fb_dtsg", fb_dtsg);
                   //nvc1.Add("source", source);
                   //nvc1.Add("profile_id", profile_id);
                   //nvc1.Add("grid_id", gridID);
                   //nvc1.Add("upload_id", "1024");
                   //nvc1.Add("qn", qn);

                   string _rev = getBetween(pageSource, "svn_rev", ",");
                   _rev = _rev.Replace("\":", string.Empty);


                   string uploadURL = "https://upload.facebook.com/ajax/composerx/attachment/media/saveunpublished?target_id=" + xhpc_targetid + "&__user=" + UsreId + "&__a=1&__dyn=7n88Oq9ccmqDxl2u5Fa8HzCqm5Aqbx2mbAKGiBAGm&__req=1t&fb_dtsg=" + fb_dtsg + "&__rev=" + _rev + "";
                   tempresponse1 = HttpUploadFile_UploadPic_temp(ref HttpHelper, UsreId, uploadURL, "composer_unpublished_photo[]", "image/jpeg", localImagePath, nvc1, "", proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                   if (tempresponse1.ToLower().Contains("errorsummary") && tempresponse1.ToLower().Contains("There was a problem with this request. We're working on getting it fixed as soon as we can".ToLower()))
                   {
                       if (tempCount < 2)
                       {
                           System.Threading.Thread.Sleep(15000);
                           tempCount++;
                           goto startAgain;
                       }
                       else
                       {
                           tempCountMain++;
                           return false;
                       }
                   }

                   //string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/timeline/cover/upload/", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                   //string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                   //tempresponse1 = HttpUploadFile_UploadPic_temp(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/composerx/attachment/media/saveunpublished?target_id=" + xhpc_targetid + "&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=l&fb_dtsg=" + fb_dtsg + "", "composer_unpublished_photo[]", "image/jpeg", localImagePath, nvc1, "", proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

               }

               NameValueCollection nvc = new NameValueCollection();
               try
               {
                   //message = Uri.EscapeDataString(message);
               }
               catch { }
               nvc.Add("fb_dtsg", fb_dtsg);
               nvc.Add("xhpc_targetid", xhpc_targetid);
               nvc.Add("xhpc_context", "profile");
               nvc.Add("xhpc_ismeta", "1");
               nvc.Add("xhpc_fbx", "1");
               nvc.Add("xhpc_timeline", "");
               nvc.Add("xhpc_composerid", xhpc_composerid);
               nvc.Add("xhpc_message_text", message);
               nvc.Add("xhpc_message", message);
               //nvc.Add("name", "file1");
               //nvc.Add("Content-Type:", "image/jpeg");
               //nvc.Add("filename=", "");



               string composer_unpublished_photo = "";
               string start_composer_unpublished_photo = Regex.Split(tempresponse1, "},\"")[1];// 
               int startIndex_composer_unpublished_photo = start_composer_unpublished_photo.IndexOf(",\"") + ",\"".Length;
               int endIndex_composer_unpublished_photo = start_composer_unpublished_photo.IndexOf("\"", startIndex_composer_unpublished_photo + 1);

               composer_unpublished_photo = start_composer_unpublished_photo.Substring(startIndex_composer_unpublished_photo, endIndex_composer_unpublished_photo - startIndex_composer_unpublished_photo);

               ///New test upload pic post
               string waterfallid = GlobusHttpHelper.ParseJson(pageSource_Home, "waterfallID");

               string newpostURL = "https://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=r&fb_dtsg=" + fb_dtsg + "";
               string newPostData = "";


               NameValueCollection newnvc = new NameValueCollection();
               try
               {
                   //message = Uri.EscapeDataString(message);
               }
               catch { }
               newnvc.Add("fb_dtsg", fb_dtsg);
               newnvc.Add("xhpc_targetid", xhpc_targetid);
               newnvc.Add("xhpc_context", "profile");
               newnvc.Add("xhpc_ismeta", "1");
               newnvc.Add("xhpc_fbx", "1");
               newnvc.Add("xhpc_timeline", "");
               newnvc.Add("xhpc_composerid", xhpc_composerid);
               newnvc.Add("xhpc_message_text", message);
               newnvc.Add("xhpc_message", message);

               newnvc.Add("composer_unpublished_photo[]", composer_unpublished_photo);
               newnvc.Add("album_type", "128");
               newnvc.Add("is_file_form", "1");
               newnvc.Add("oid", "");
               newnvc.Add("qn", waterfallid);
               newnvc.Add("application", "composer");
               newnvc.Add("is_explicit_place", "");
               newnvc.Add("composertags_place", "");
               newnvc.Add("composertags_place_name", "");
               newnvc.Add("composer_session_id", composer_session_id);
               newnvc.Add("composertags_city", "");
               newnvc.Add("vzdisable_location_sharing", "false");
               newnvc.Add("composer_predicted_city", "");

               // string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/timeline/cover/upload/", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

               // string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

               string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, newpostURL, "file1", "image/jpeg", localImagePath, newnvc, targeturl, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);//HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88O49ccm9o-2Ki&__req=1c&fb_dtsg=" + fb_dtsg + "", "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);



               //http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=100004608395129
               if (string.IsNullOrEmpty(response))
               {
                   try
                   {
                       //response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);
                       response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88O49ccm9o-2Ki&__req=1c&fb_dtsg=" + fb_dtsg + "", "file1", "image/jpeg", localImagePath, nvc, targeturl, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                   }
                   catch { }
               }
               string posturl = "https://www.facebook.com/ajax/places/city_sharer_reset.php";
               string postdata = "__user=" + UsreId + "&__a=1&fb_dtsg=" + fb_dtsg + "&phstamp=1658167761111108210145";
               string responsestring = HttpHelper.postFormData(new Uri(posturl), postdata);
               try
               {
                   string okay = HttpHelper.getHtmlfromUrl(new Uri("https://3-pct.channel.facebook.com/pull?channel=p_" + UsreId + "&seq=3&partition=69&clientid=70e140db&cb=8p7w&idle=8&state=active&mode=stream&format=json"));
               }
               catch
               {
               }

               if (!string.IsNullOrEmpty(response) && response.Contains("payload\":{\"photo_fbid"))//response.Contains("photo.php?fbid="))
               {

                   #region PostData_ForCoverPhotoSelect
                   //fb_dtsg=AQCLSjCH&photo_id=130869487061841&profile_id=100004163701035&photo_offset=0&video_id=&save=Save%20Changes&nctr[_mod]=pagelet_main_column_personal&__user=100004163701035&__a=1&phstamp=165816776831066772182 
                   #endregion

                   try
                   {

                       if (!response.Contains("errorSummary") || !response.Contains("error"))
                       {
                           isSentPicMessage = true;
                       }
                       if (response.Contains("Your post has been submitted and is pending approval by an admin"))
                       {
                           GlobusLogHelper.log.Debug("Your post has been submitted and is pending approval by an admin." + "GroupUrl >>>" + targeturl);
                           GlobusLogHelper.log.Info("Your post has been submitted and is pending approval by an admin." + "GroupUrl >>>" + targeturl);
                       }
                   }
                   catch { }
                   #region CodeCommented
                   //    string photo_idValue = response.Substring(response.IndexOf("photo.php?fbid="), response.IndexOf(";", response.IndexOf("photo.php?fbid=")) - response.IndexOf("photo.php?fbid=")).Replace("photo.php?fbid=", string.Empty).Trim();
                   //    string[] arrphoto_idValue = Regex.Split(photo_idValue, "[^0-9]");

                   //    foreach (string item in arrphoto_idValue)
                   //    {
                   //        try
                   //        {
                   //            if (item.Length > 6)
                   //            {
                   //                photo_id = item;
                   //                break;
                   //            }
                   //        }
                   //        catch
                   //        {
                   //        }
                   //    }

                   //   // string postData = "fb_dtsg=" + fb_dtsg + "&photo_id=" + photo_id + "&profile_id=" + UsreId + "&photo_offset=0&video_id=&save=Save%20Changes&nctr[_mod]=pagelet_main_column_personal&__user=" + UsreId + "&__a=1&phstamp=165816776831066772182 ";
                   //   // string postResponse = HttpHelper.postFormData(new Uri("https://www.facebook.com/ajax/timeline/cover_photo_select.php"), postData);

                   //    //if (!postResponse.Contains("error"))
                   //    //{
                   //    //    //string ok = "ok";
                   //    //    isSentPicMessage = true;
                   //    //}
                   //    //if (string.IsNullOrEmpty(postResponse) || string.IsNullOrWhiteSpace(postResponse))
                   //    //{
                   //    //    status = "Response Is Null !";
                   //    //}
                   //    //if (postResponse.Contains("errorSummary"))
                   //    //{
                   //    //    string summary = GlobusHttpHelper.ParseJson(postResponse, "errorSummary");
                   //    //    string errorDescription = GlobusHttpHelper.ParseJson(postResponse, "errorDescription");

                   //    //    status = "Posting Error: " + summary + " | Error Description: " + errorDescription;
                   //    //    //FanPagePosterLogger("Posting Error: " + summary + " | Error Description: " + errorDescription);
                   //    //}
                   //}
                   //catch
                   //{
                   //} 
                   #endregion
               }
           }
           catch
           {
           }
           return isSentPicMessage;
       }

       public bool AddaPicture(ref GlobusHttpHelper HttpHelper, string Username, string Password, string localImagePath, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, string targeturl, string message, ref string status, string pageSource_Home, string xhpc_targetid, string xhpc_composerid, string message_text, string fb_dtsg, string UsreId)
       {

           string pageSource = HttpHelper.getHtmlfromUrl(new Uri(targeturl));



           bool isSentPicMessage = false;
           //string fb_dtsg = string.Empty;
           string photo_id = string.Empty;
           //string UsreId = string.Empty;
           //xhpc_composerid = string.Empty;
           //xhpc_targetid = string.Empty;
           //message_text = string.Empty;

           try
           {
               #region commentedCode


               //string pageSource_Home = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

               //UsreId = GlobusHttpHelper.GetParamValue(pageSource_Home, "user");
               //if (string.IsNullOrEmpty(UsreId))
               //{
               //    UsreId = GlobusHttpHelper.ParseJson(pageSource_Home, "user");
               //}

               //fb_dtsg = GlobusHttpHelper.GetParamValue(pageSource_Home, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
               //if (string.IsNullOrEmpty(fb_dtsg))
               //{
               //    fb_dtsg = GlobusHttpHelper.ParseJson(pageSource_Home, "fb_dtsg");
               //}


               //string pageSource_HomeData = HttpHelper.getHtmlfromUrl(new Uri(targeturl));
               //xhpc_composerid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "composerid");
               //xhpc_targetid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "xhpc_targetid"); 
               #endregion

               string composer_session_id = "";

               string tempresponse1 = "";
               ///temp post
               {
                   string source = "";
                   string profile_id = "";
                   string gridID = "";
                   string qn = string.Empty;

                   try
                   {
                       string Url = "https://upload.facebook.com/ajax/composerx/attachment/media/saveunpublished?target_id=139180732957326&__user=100007423262870&__a=1&__dyn=7n88Oq9ccmqDxl2u5Fa8HzCqm5Aqbx2mbAKGiBAGm&__req=1t&fb_dtsg=AQCehhQg&__rev=1062230";
                       string posturl1 = "fb_dtsg=" + fb_dtsg + "&composerid=" + xhpc_composerid + "&targetid=" + xhpc_targetid + "&loaded_components[0]=maininput&loaded_components[1]=cameraicon&loaded_components[2]=withtaggericon&loaded_components[3]=placetaggericon&loaded_components[4]=mainprivacywidget&loaded_components[5]=cameraicon&loaded_components[6]=mainprivacywidget&loaded_components[7]=withtaggericon&loaded_components[8]=placetaggericon&loaded_components[9]=maininput&nctr[_mod]=pagelet_group_composer&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=i&phstamp=16581688688747595501";    //"fb_dtsg=" + fb_dtsg + "&composerid=" + xhpc_composerid + "&targetid=" + xhpc_targetid + "&istimeline=1&timelinelocation=composer&loaded_components[0]=maininput&loaded_components[1]=mainprivacywidget&loaded_components[2]=mainprivacywidget&loaded_components[3]=maininput&loaded_components[4]=explicitplaceinput&loaded_components[5]=hiddenplaceinput&loaded_components[6]=placenameinput&loaded_components[7]=hiddensessionid&loaded_components[8]=withtagger&loaded_components[9]=backdatepicker&loaded_components[10]=placetagger&loaded_components[11]=withtaggericon&loaded_components[12]=backdateicon&loaded_components[13]=citysharericon&nctr[_mod]=pagelet_timeline_recent&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=18&phstamp=1658168111112559866679";
                       // string PostUrl = "city_id=" + CityIDS1 + "&city_page_id=" + city_page_id + "&city_name=" + CityName1 + "&is_default=false&session_id=1362404125&__user=" + UsreId + "&__a=1&__dyn=798aD5z5ynU&__req=z&fb_dtsg=" + fb_dtsg + "&phstamp=1658168111112559866165";
                       string res11 = pageSource_Home;//HttpHelper.postFormData(new Uri(Url), posturl1);


                       try
                       {
                           source = res11.Substring(res11.IndexOf("source\":"), (res11.IndexOf(",", res11.IndexOf("source\":")) - res11.IndexOf("source\":"))).Replace("source\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                       }
                       catch { }
                       try
                       {
                           profile_id = res11.Substring(res11.IndexOf("profile_id\":"), (res11.IndexOf("}", res11.IndexOf("profile_id\":")) - res11.IndexOf("profile_id\":"))).Replace("profile_id\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                           if (profile_id.Contains(","))
                           {
                               profile_id = ParseEncodedJson(res11, "profile_id");
                           }
                           //"gridID":
                       }
                       catch { }
                       try
                       {
                           gridID = res11.Substring(res11.IndexOf("gridID\":"), (res11.IndexOf(",", res11.IndexOf("gridID\":")) - res11.IndexOf("gridID\":"))).Replace("gridID\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                       }
                       catch { }
                       try
                       {
                           composer_session_id = res11.Substring(res11.IndexOf("composer_session_id\":"), (res11.IndexOf("}", res11.IndexOf("composer_session_id\":")) - res11.IndexOf("composer_session_id\":"))).Replace("composer_session_id\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();
                       }
                       catch { }

                       try
                       {
                           if (string.IsNullOrEmpty(composer_session_id))
                           {
                               composer_session_id = res11.Substring(res11.IndexOf("composerID\":"), (res11.IndexOf("}", res11.IndexOf("composerID\":")) - res11.IndexOf("composerID\":"))).Replace("composerID\":", string.Empty).Replace("<dd>", string.Empty).Replace("\\", string.Empty).Replace("\"", string.Empty).Trim();

                           }
                       }
                       catch { }

                       try
                       {
                           qn = getBetween(res11, "qn\\\" value=\\\"", "\\\" \\/>");
                       }
                       catch { }
                   }
                   catch { }

                   NameValueCollection nvc1 = new NameValueCollection();
                   try
                   {
                       //message = Uri.EscapeDataString(message);
                   }
                   catch { }

                   //xhpc_composerid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "composerid");
                   //xhpc_targetid = GlobusHttpHelper.GetParamValue(pageSource_HomeData, "xhpc_targetid");
                   //-------------------------------
                   //nvc1.Add("fb_dtsg", fb_dtsg);
                   //nvc1.Add("source", source);
                   //nvc1.Add("profile_id", profile_id);
                   //nvc1.Add("grid_id", gridID);
                   //nvc1.Add("upload_id", "1024");
                   //-----------------------------------
                   nvc1.Add("fb_dtsg", fb_dtsg);
                   nvc1.Add("source", source);
                   nvc1.Add("profile_id", profile_id);
                   nvc1.Add("grid_id", gridID);
                   nvc1.Add("upload_id", "1024");
                   nvc1.Add("qn", qn);

                   //nvc1.Add("fb_dtsg", fb_dtsg);
                   //nvc1.Add("source", source);
                   //nvc1.Add("profile_id", profile_id);
                   //nvc1.Add("grid_id", gridID);
                   //nvc1.Add("upload_id", "1024");
                   //nvc1.Add("qn", qn);

                   string _rev = getBetween(pageSource, "svn_rev", ",");
                   _rev = _rev.Replace("\":", string.Empty);


                   string uploadURL = "https://upload.facebook.com/ajax/composerx/attachment/media/saveunpublished?target_id=139180732957326&__user=100007423262870&__a=1&__dyn=7n88Oq9ccmqDxl2u5Fa8HzCqm5Aqbx2mbAKGiBAGm&__req=1t&fb_dtsg=AQCehhQg&__rev=1062230";
                   tempresponse1 = HttpUploadFile_UploadPic_temp(ref HttpHelper, UsreId, uploadURL, "composer_unpublished_photo[]", "image/jpeg", localImagePath, nvc1, "", proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                   //string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/timeline/cover/upload/", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                   //string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                   tempresponse1 = HttpUploadFile_UploadPic_temp(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/composerx/attachment/media/saveunpublished?target_id=" + xhpc_targetid + "&__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=l&fb_dtsg=" + fb_dtsg + "", "composer_unpublished_photo[]", "image/jpeg", localImagePath, nvc1, "", proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

               }

               NameValueCollection nvc = new NameValueCollection();
               try
               {
                   //message = Uri.EscapeDataString(message);
               }
               catch { }
               nvc.Add("fb_dtsg", fb_dtsg);
               nvc.Add("xhpc_targetid", xhpc_targetid);
               nvc.Add("xhpc_context", "profile");
               nvc.Add("xhpc_ismeta", "1");
               nvc.Add("xhpc_fbx", "1");
               nvc.Add("xhpc_timeline", "");
               nvc.Add("xhpc_composerid", xhpc_composerid);
               nvc.Add("xhpc_message_text", message);
               nvc.Add("xhpc_message", message);
               //nvc.Add("name", "file1");
               //nvc.Add("Content-Type:", "image/jpeg");
               //nvc.Add("filename=", "");



               string composer_unpublished_photo = "";
               string start_composer_unpublished_photo = Regex.Split(tempresponse1, "},\"")[1];// 
               int startIndex_composer_unpublished_photo = start_composer_unpublished_photo.IndexOf(",\"") + ",\"".Length;
               int endIndex_composer_unpublished_photo = start_composer_unpublished_photo.IndexOf("\"", startIndex_composer_unpublished_photo + 1);

               composer_unpublished_photo = start_composer_unpublished_photo.Substring(startIndex_composer_unpublished_photo, endIndex_composer_unpublished_photo - startIndex_composer_unpublished_photo);

               ///New test upload pic post
               string waterfallid = GlobusHttpHelper.ParseJson(pageSource_Home, "waterfallID");

               string newpostURL = "https://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88QoAMNoBwXAw&__req=r&fb_dtsg=" + fb_dtsg + "";
               string newPostData = "";


               NameValueCollection newnvc = new NameValueCollection();
               try
               {
                   //message = Uri.EscapeDataString(message);
               }
               catch { }
               newnvc.Add("fb_dtsg", fb_dtsg);
               newnvc.Add("xhpc_targetid", xhpc_targetid);
               newnvc.Add("xhpc_context", "profile");
               newnvc.Add("xhpc_ismeta", "1");
               newnvc.Add("xhpc_fbx", "1");
               newnvc.Add("xhpc_timeline", "");
               newnvc.Add("xhpc_composerid", xhpc_composerid);
               newnvc.Add("xhpc_message_text", message);
               newnvc.Add("xhpc_message", message);

               newnvc.Add("composer_unpublished_photo[]", composer_unpublished_photo);
               newnvc.Add("album_type", "128");
               newnvc.Add("is_file_form", "1");
               newnvc.Add("oid", "");
               newnvc.Add("qn", waterfallid);
               newnvc.Add("application", "composer");
               newnvc.Add("is_explicit_place", "");
               newnvc.Add("composertags_place", "");
               newnvc.Add("composertags_place_name", "");
               newnvc.Add("composer_session_id", composer_session_id);
               newnvc.Add("composertags_city", "");
               newnvc.Add("vzdisable_location_sharing", "false");
               newnvc.Add("composer_predicted_city", "");

               // string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/ajax/timeline/cover/upload/", "pic", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

               // string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

               string response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, newpostURL, "file1", "image/jpeg", localImagePath, newnvc, targeturl, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);//HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "http://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88O49ccm9o-2Ki&__req=1c&fb_dtsg=" + fb_dtsg + "", "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);



               //http://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=100004608395129
               if (string.IsNullOrEmpty(response))
               {
                   try
                   {
                       //response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/media/upload/photos/composer/?__a=1&__adt=3&__iframe=true&__user=" + UsreId, "file1", "image/jpeg", localImagePath, nvc, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);
                       response = HttpUploadFile_UploadPic(ref HttpHelper, UsreId, "https://upload.facebook.com/media/upload/photos/composer/?__user=" + UsreId + "&__a=1&__dyn=7n88O49ccm9o-2Ki&__req=1c&fb_dtsg=" + fb_dtsg + "", "file1", "image/jpeg", localImagePath, nvc, targeturl, proxyAddress, Convert.ToInt32(0), proxyUsername, proxyPassword);

                   }
                   catch { }
               }
               string posturl = "https://www.facebook.com/ajax/places/city_sharer_reset.php";
               string postdata = "__user=" + UsreId + "&__a=1&fb_dtsg=" + fb_dtsg + "&phstamp=1658167761111108210145";
               string responsestring = HttpHelper.postFormData(new Uri(posturl), postdata);
               try
               {
                   string okay = HttpHelper.getHtmlfromUrl(new Uri("https://3-pct.channel.facebook.com/pull?channel=p_" + UsreId + "&seq=3&partition=69&clientid=70e140db&cb=8p7w&idle=8&state=active&mode=stream&format=json"));
               }
               catch
               {
               }

               if (!string.IsNullOrEmpty(response) && response.Contains("photo.php?fbid="))
               {

                   #region PostData_ForCoverPhotoSelect
                   //fb_dtsg=AQCLSjCH&photo_id=130869487061841&profile_id=100004163701035&photo_offset=0&video_id=&save=Save%20Changes&nctr[_mod]=pagelet_main_column_personal&__user=100004163701035&__a=1&phstamp=165816776831066772182 
                   #endregion

                   try
                   {

                       if (!response.Contains("errorSummary") || !response.Contains("error"))
                       {
                           isSentPicMessage = true;
                       }
                       if (response.Contains("Your post has been submitted and is pending approval by an admin"))
                       {
                           GlobusLogHelper.log.Debug("Your post has been submitted and is pending approval by an admin." + "GroupUrl >>>" + targeturl);
                           GlobusLogHelper.log.Info("Your post has been submitted and is pending approval by an admin." + "GroupUrl >>>" + targeturl);
                       }
                   }
                   catch { }
                   #region CodeCommented
                   //    string photo_idValue = response.Substring(response.IndexOf("photo.php?fbid="), response.IndexOf(";", response.IndexOf("photo.php?fbid=")) - response.IndexOf("photo.php?fbid=")).Replace("photo.php?fbid=", string.Empty).Trim();
                   //    string[] arrphoto_idValue = Regex.Split(photo_idValue, "[^0-9]");

                   //    foreach (string item in arrphoto_idValue)
                   //    {
                   //        try
                   //        {
                   //            if (item.Length > 6)
                   //            {
                   //                photo_id = item;
                   //                break;
                   //            }
                   //        }
                   //        catch
                   //        {
                   //        }
                   //    }

                   //   // string postData = "fb_dtsg=" + fb_dtsg + "&photo_id=" + photo_id + "&profile_id=" + UsreId + "&photo_offset=0&video_id=&save=Save%20Changes&nctr[_mod]=pagelet_main_column_personal&__user=" + UsreId + "&__a=1&phstamp=165816776831066772182 ";
                   //   // string postResponse = HttpHelper.postFormData(new Uri("https://www.facebook.com/ajax/timeline/cover_photo_select.php"), postData);

                   //    //if (!postResponse.Contains("error"))
                   //    //{
                   //    //    //string ok = "ok";
                   //    //    isSentPicMessage = true;
                   //    //}
                   //    //if (string.IsNullOrEmpty(postResponse) || string.IsNullOrWhiteSpace(postResponse))
                   //    //{
                   //    //    status = "Response Is Null !";
                   //    //}
                   //    //if (postResponse.Contains("errorSummary"))
                   //    //{
                   //    //    string summary = GlobusHttpHelper.ParseJson(postResponse, "errorSummary");
                   //    //    string errorDescription = GlobusHttpHelper.ParseJson(postResponse, "errorDescription");

                   //    //    status = "Posting Error: " + summary + " | Error Description: " + errorDescription;
                   //    //    //FanPagePosterLogger("Posting Error: " + summary + " | Error Description: " + errorDescription);
                   //    //}
                   //}
                   //catch
                   //{
                   //} 
                   #endregion
               }
           }
           catch
           {
           }
           return isSentPicMessage;
       }
        
        public string HttpUploadFile_UploadPic(ref GlobusHttpHelper HttpHelper, string userid, string url, string paramName, string contentType, string localImagePath, NameValueCollection nvc, string referer, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {

            #region PostData_ForUploadImage
            //-----------------------------68682554727644
            //Content-Disposition: form-data; name="fb_dtsg"

            //AQCLSjCH
            // -----------------------------68682554727644
            //Content-Disposition: form-data; name="pic"; filename="Hydrangeas.jpg"
            //Content-Type: image/jpeg

            //���� 
            #endregion


            bool isAddaCover = false;
            string responseStr = string.Empty;

            try
            {
                ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
                //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("");
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();//"-----------------------------" + DateTime.Now.Ticks.ToString();
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                //gRequest.Referer = "Referer: https://www.facebook.com/profile.php?id=" + userid + "&ref=tn_tnmn";
                gRequest.Referer = referer;// "http://www.facebook.com/?sk=welcome";

                gRequest.Method = "POST";
                gRequest.KeepAlive = true;
                gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

                gRequest.Headers.Add("X-SVN-Rev", "827944");
                gRequest.UserAgent = UserAgent;
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }
                #endregion

                using (Stream rs = gRequest.GetRequestStream())
                {
                    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    foreach (string key in nvc.Keys)
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, key, nvc[key]);
                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                        rs.Write(formitembytes, 0, formitembytes.Length);
                    }
                    rs.Write(boundarybytes, 0, boundarybytes.Length);

                    #region CodeCommented
                    //string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    //string header = string.Format(headerTemplate, paramName, localImagePath, contentType);
                    //byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                    //rs.Write(headerbytes, 0, headerbytes.Length);

                    //using (FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read))
                    //{
                    //    byte[] buffer = new byte[4096];
                    //    int bytesRead = 0;
                    //    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    //    {
                    //        rs.Write(buffer, 0, bytesRead);
                    //    }
                    //} 
                    #endregion

                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    rs.Write(trailer, 0, trailer.Length);
                }

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }

                #endregion

                try
                {
                    using (WebResponse wresp = gRequest.GetResponse())
                    {
                        //wresp = gRequest.GetResponse();
                        using (Stream stream2 = wresp.GetResponseStream())
                        {
                            using (StreamReader reader2 = new StreamReader(stream2))
                            {
                                response_ImageUpload = reader2.ReadToEnd();
                            }
                        }
                        //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                        return response_ImageUpload;
                    }
                }
                catch (Exception ex)
                {
                    //log.Error("Error uploading file", ex);
                    error_ImageUpload = ex.Message;
                }
                finally
                {
                    gRequest = null;
                }
                return response_ImageUpload;

                #region COmmentedCode
                //}

                //int tempi = 0;

                //string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                //string formitem = string.Empty;
                //foreach (string key in nvc.Keys)
                //{

                //    if (tempi < 9)
                //    {
                //        byte[] firstboundarybytes = System.Text.Encoding.ASCII.GetBytes(boundary + "\r\n");
                //        rs.Write(firstboundarybytes, 0, firstboundarybytes.Length);
                //        formitem = formitem + string.Format(formdataTemplate, key, nvc[key]);
                //        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                //        rs.Write(formitembytes, 0, formitembytes.Length);
                //        tempi++;
                //        continue;
                //    }
                //    //rs.Write(boundarybytes, 0, boundarybytes.Length);
                //    //formitem = string.Format(formdataTemplate, key, nvc[key]);
                //    //byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                //    //rs.Write(formitembytes, 0, formitembytes.Length);
                //}
                //rs.Write(boundarybytes, 0, boundarybytes.Length);

                //string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                //string header = string.Format(headerTemplate, paramName, localImagePath, contentType);
                //byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                //rs.Write(headerbytes, 0, headerbytes.Length);

                //FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read);
                //byte[] buffer = new byte[4096];
                //int bytesRead = 0;
                //while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                //{
                //    rs.Write(buffer, 0, bytesRead);
                //}
                //fileStream.Close();

                //byte[] trailer3 = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                //rs.Write(trailer3, 0, trailer3.Length);

                //string trailerTemplate1 = "Content-Disposition: form-data; name=\"profile_id\"\r\n\r\n{0}\r\n";
                //string trailer1 = string.Format(trailerTemplate1, userid);
                //byte[] arrtrailer1 = System.Text.Encoding.UTF8.GetBytes(trailer1);
                //rs.Write(arrtrailer1, 0, arrtrailer1.Length);

                //byte[] trailer4 = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                //rs.Write(trailer4, 0, trailer3.Length);

                //string trailerTemplate2 = "Content-Disposition: form-data; name=\"source\"\r\n\r\n{0}\r\n";
                //string trailer2 = string.Format(trailerTemplate2, "10");
                //byte[] arrtrailer2 = System.Text.Encoding.UTF8.GetBytes(trailer2);
                //rs.Write(arrtrailer2, 0, arrtrailer2.Length);

                //byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                //rs.Write(trailer, 0, trailer.Length);
                //rs.Close();

                //#region CookieManagment

                //if (this.gCookies != null && this.gCookies.Count > 0)
                //{
                //    gRequest.CookieContainer.Add(gCookies);
                //}

                //#endregion

                //    WebResponse wresp = null;
                //    try
                //    {
                //        wresp = gRequest.GetResponse();
                //        Stream stream2 = wresp.GetResponseStream();
                //        StreamReader reader2 = new StreamReader(stream2);
                //        responseStr = reader2.ReadToEnd();
                //        //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                //        return responseStr;
                //    }
                //    catch (Exception ex)
                //    {
                //        //log.Error("Error uploading file", ex);
                //        if (wresp != null)
                //        {
                //            wresp.Close();
                //            wresp = null;
                //        }
                //        // return false;
                //    }
                //}
                //catch
                //{
                //}

                //finally
                //{
                //    gRequest = null;
                //}
                //return responseStr; 
                #endregion

            }
            catch { }
            return responseStr;
        }

        public string HttpUploadFile_UploadPic_temp(ref GlobusHttpHelper HttpHelper, string userid, string url, string paramName, string contentType, string localImagePath, NameValueCollection nvc, string referer, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {


            bool isAddaCover = false;
            string responseStr = string.Empty;

            try
            {
                ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
                //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("");
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();//"-----------------------------" + DateTime.Now.Ticks.ToString();
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                //gRequest.Referer = "Referer: https://www.facebook.com/profile.php?id=" + userid + "&ref=tn_tnmn";
                gRequest.Referer = referer;// "http://www.facebook.com/?sk=welcome";

                gRequest.Method = "POST";
                gRequest.KeepAlive = true;
                gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

                gRequest.Headers.Add("X-SVN-Rev", "827944");
                gRequest.UserAgent = UserAgent;
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }
                #endregion

                using (Stream rs = gRequest.GetRequestStream())
                {
                    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    foreach (string key in nvc.Keys)
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, key, nvc[key]);
                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                        rs.Write(formitembytes, 0, formitembytes.Length);
                    }
                    rs.Write(boundarybytes, 0, boundarybytes.Length);

                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    string header = string.Format(headerTemplate, paramName, localImagePath, contentType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                    rs.Write(headerbytes, 0, headerbytes.Length);

                    using (FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            rs.Write(buffer, 0, bytesRead);
                        }
                    }
                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    rs.Write(trailer, 0, trailer.Length);
                }

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }

                #endregion

                WebResponse wresp = null;
                try
                {
                    wresp = gRequest.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    using (StreamReader reader2 = new StreamReader(stream2))
                    {
                        responseStr = reader2.ReadToEnd();
                    }
                    return responseStr;
                    //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                    //return true;
                }
                catch (Exception ex)
                {
                    //log.Error("Error uploading file", ex);
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                    // return false;
                }
                finally
                {
                    gRequest = null;
                }
                return responseStr;

            }
            catch { }
            return responseStr;
        }

        public string UploadprofilePicOnInstagram(ref GlobusHttpHelper HttpHelper, string url, string CSRFToken, string contentType, string localImagePath, NameValueCollection nvc, string referer, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {
          
            bool isAddaCover = false;
            string responseStr = string.Empty;
            string AllDataWriten = string.Empty;
            try
            {
                ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
                //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("");
                string boundary = "------WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] boundarybytes_First = System.Text.Encoding.ASCII.GetBytes(boundary + "\r\n");

                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.ContentType = "multipart/form-data; boundary=" + boundary.Replace("------","----");                               
                gRequest.Referer = referer;// 
                gRequest.Method = "POST";
                gRequest.KeepAlive = true;
                gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                gRequest.Accept = "*/*";
                if (!string.IsNullOrEmpty(CSRFToken))
                {
                    gRequest.Headers.Add("X-CSRFToken", CSRFToken);
                }
                gRequest.Headers.Add("X-Instagram-AJAX", "1");
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                gRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                gRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                gRequest.Headers.Add("Origin", "https://www.instagram.com");

                gRequest.UserAgent = UserAgent;
                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                
                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);
                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }
                #endregion
                string file = string.Empty;
                string ContentType = string.Empty;
                using (Stream rs = gRequest.GetRequestStream())
                {
                    string formdataTemplate1 = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";

                    rs.Write(boundarybytes_First, 0, boundarybytes_First.Length);
                    AllDataWriten += Encoding.UTF8.GetString(boundarybytes_First);
                    foreach (string key in nvc.Keys)
                    {
                        ContentType = Regex.Split(nvc[key], "<:><:><:>")[1];
                        file = Regex.Split(nvc[key], "<:><:><:>")[0];
                        string[] filename = Regex.Split(file, "\\\\");
                        string header = string.Format(formdataTemplate, key, filename[filename.Length - 1], ContentType);
                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(header);
                        rs.Write(formitembytes, 0, formitembytes.Length);
                        AllDataWriten += Encoding.UTF8.GetString(formitembytes);
                        using (FileStream fileStream = new FileStream(localImagePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] buffer = new byte[4096];
                            int bytesRead = 0;
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                rs.Write(buffer, 0, bytesRead);
                            }
                        }
                        AllDataWriten += localImagePath;

                      //  rs.Write(boundarybytes, 0, boundarybytes.Length);
                       // string formitem = string.Format(formdataTemplate, key, nvc[key]);
                       // byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                        
                    }
                //    rs.Write(boundarybytes, 0, boundarybytes.Length);

                 

                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n" + boundary + "--\r\n");
                    rs.Write(trailer, 0, trailer.Length);
                    AllDataWriten += Encoding.UTF8.GetString(trailer);
                }

                #region CookieManagment

                if (this.gCookies != null && this.gCookies.Count > 0)
                {
                    gRequest.CookieContainer.Add(gCookies);
                }

                #endregion

                try
                {
                    WebResponse wresp=null;
                    wresp = gRequest.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    using (StreamReader reader2 = new StreamReader(stream2))
                    {
                        responseStr = reader2.ReadToEnd();
                    }
                    return responseStr;



                    //using (WebResponse wresp = gRequest.GetResponse())
                    //{
                    //    //wresp = gRequest.GetResponse();
                    //    using (Stream stream2 = wresp.GetResponseStream())
                    //    {
                    //        using (StreamReader reader2 = new StreamReader(stream2))
                    //        {
                    //            response_ImageUpload = reader2.ReadToEnd();
                    //        }
                    //    }
                    //    //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                    //    return response_ImageUpload;
                    //}
                }
                catch (Exception ex)
                {
                    //log.Error("Error uploading file", ex);
                    error_ImageUpload = ex.Message;
                }
                finally
                {
                    gRequest = null;
                }
                return response_ImageUpload;             

            }
            catch { }
            return responseStr;
        }

        public List<string> GetTextDataByTagAndAttributeName(string pageSrcHtml, string TagName, string AttributeName)
        {
            List<string> lstData = new List<string>();
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;
                string dataDescription = string.Empty;

                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************
                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    string text = xNode.AccumulateTagContent("text", "script|style");
                    lstData.Add(text);

                    //** Get Data Under Tag All  Html value * *********************************
                    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                }
                #endregion
                return lstData;
            }
            catch (Exception)
            {
                return lstData = null;

            }
        }

        public List<string> GetHrefsByTagAndAttributeName(string pageSrcHtml, string TagName, string className)
        {
            List<string> lstData = new List<string>();
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;
                string dataDescription = string.Empty;

                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", className);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************
                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    List<string> lstHrefs = GetHrefFromString(dataDescription);

                    lstData.AddRange(lstHrefs);//lstData.Add(dataDescription);

                    //** Get Data Under Tag All  Html value * *********************************
                    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", className);
                }
                #endregion
                return lstData;
            }
            catch (Exception)
            {
                return lstData = null;

            }
        }


        public List<string> GetHrefFromString(string pageSrcHtml)
        {
            List<string> list = new List<string>();

            try
            {
                Chilkat.HtmlUtil obj = new Chilkat.HtmlUtil();

                Chilkat.StringArray dataImage = obj.GetHyperlinkedUrls(pageSrcHtml);

                for (int i = 0; i < dataImage.Length; i++)
                {
                    try
                    {
                        string hreflink = dataImage.GetString(i);
                        list.Add(hreflink);
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return list;
        }

        public string GetHrefFromStringForRealEstates(string pageSrcHtml)
        {
            string mainUrl = "http://www.zillow.com/";
            List<string> list = new List<string>();
            string hreflink = string.Empty;

            try
            {
                Chilkat.HtmlUtil obj = new Chilkat.HtmlUtil();

                Chilkat.StringArray dataImage = obj.GetHyperlinkedUrls(pageSrcHtml);

                for (int i = 0; i < dataImage.Length; i++)
                {
                    try
                    {
                        hreflink = dataImage.GetString(i);
                        //list.Add(hreflink);
                        hreflink = mainUrl + hreflink;
                        list.Add(mainUrl+hreflink);
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return hreflink;
        }


        public string GetHrefFromStringForPagination1(string pageSrcHtml)
        {
            string mainUrl = "http://www.zillow.com/";
            List<string> list = new List<string>();

            try
            {
                Chilkat.HtmlUtil obj = new Chilkat.HtmlUtil();

                Chilkat.StringArray dataImage = obj.GetHyperlinkedUrls(pageSrcHtml);


                string str = dataImage.GetString(dataImage.Length - 1);
                return str+"\">";

                //for (int i = 0; i < dataImage.Length; i++)
                //{
                //    try
                //    {
                //        string hreflink = dataImage.GetString(i);
                //        //list.Add(hreflink);
                //        list.Add(mainUrl + hreflink);
                //    }
                //    catch (Exception ex)
                //    {
                //        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                //    }

                //}
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return "";
        }

        public List<string> GetDataTag(string pageSrcHtml, string TagName)
        {
            bool success = false;
            string xHtml = string.Empty;
            List<string> list = new List<string>();

            try
            {


                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //xHtml contain xml data 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                xBeginSearchAfter = null;
                xNode = xml.SearchForTag(xBeginSearchAfter, TagName);
                while ((xNode != null))
                {

                    string TagText = xNode.AccumulateTagContent("text", "script|style");

                    list.Add(TagText);

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForTag(xBeginSearchAfter, TagName);

                }
                //xHtml.
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return list;
        }


        public string GetData_LoginThroughInstagram(Uri url, string proxyAddress, string proxyPort, string ProxyUsername, string proxyPassword)
        {
            gRequest = (HttpWebRequest)WebRequest.Create(url);

            //gRequest.ServicePoint.Expect100Continue = false;
            gRequest.Method = "POST";
            gRequest.Host = "www.instagram.com";
            //  gRequest.KeepAlive = true;
            // gRequest.ContentLength = 41
            //   gRequest.Headers.Add("Origin", "https://www.instagram.com");
            // gRequest.Headers.Add("X-Instagram-AJAX", "1");
            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
            gRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            gRequest.Accept = "*/*";
            //gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            //  gRequest.Headers.Add("X-Requested-With", Token);
            //  gRequest.Headers.Add("DNT", "1");
            gRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            gRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");

            int proxyPortCopy = 80;
            try
            {
                if (string.IsNullOrEmpty(proxyPort))
                {
                    proxyPortCopy = 80;
                }
                else
                {

                    proxyPortCopy = int.Parse(proxyPort);
                }
            }
            catch
            {
                proxyPortCopy = proxyPortCopy;

            }

            try
            {
                this.proxyAddress = proxyAddress;
                this.port = proxyPortCopy;
                this.proxyUsername = ProxyUsername;
                this.proxyPassword = proxyPassword;
            }
            catch { };

            ChangeProxy(this.proxyAddress, this.port, this.proxyUsername, this.proxyPassword);

            #region CookieManagement
            gRequest.CookieContainer = new CookieContainer();

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            //try
            //{
            //    setExpect100Continue();
            //    string postdata = string.Format(postData);
            //    byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
            //    gRequest.ContentLength = postBuffer.Length;
            //    Stream postDataStream = gRequest.GetRequestStream();
            //    postDataStream.Write(postBuffer, 0, postBuffer.Length);
            //    postDataStream.Close();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            //}
            //post data logic ends

            //Get Response for this request url
            string html;
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                //Logger.LogText("Response from " + url + ":" + ex.Message, null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = new CookieCollection();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public string PostData_LoginThroughInstagram(Uri url, string postData, string Referes, string Token)
        {
            gRequest = (HttpWebRequest)WebRequest.Create(url);

            //gRequest.ServicePoint.Expect100Continue = false;
            gRequest.Method = "POST";
            gRequest.Host = "www.instagram.com";
            gRequest.KeepAlive = true;
            // gRequest.ContentLength = 41
            gRequest.Headers.Add("Origin", "https://www.instagram.com");
            gRequest.Headers.Add("X-Instagram-AJAX", "1");
            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
            gRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            gRequest.Accept = "*/*";
            gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            string ssrftoken = "";
            if (this.gCookies.Count > 0)
            {
                foreach (Cookie ck in this.gCookies)
                {
                    if (ck.Name == "csrftoken")
                    {
                        ssrftoken = ck.Value;
                        break;
                    }

                }
            }

            gRequest.Headers.Add("X-CSRFToken", ssrftoken);
            gRequest.Headers.Add("DNT", "1");
            gRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            gRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            gRequest.Referer = "https://www.instagram.com/";



            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);
            gRequest.CookieContainer = new CookieContainer();
            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                //  byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                byte[] postBuffer = System.Text.Encoding.UTF8.GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            string html;
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                //Logger.LogText("Response from " + url + ":" + ex.Message, null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

    }
}
