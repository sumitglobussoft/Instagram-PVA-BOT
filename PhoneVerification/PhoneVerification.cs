using Accounts;
using BaseLib;
using BaseLibID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using OpenQA.Selenium.PhantomJS;

namespace PhoneVerification
{
    public delegate void AddToLabel(int count);
    public class PhoneVerification
    {
        public static AddToLabel objVerifiedAccountCount;
        public static AddToLabel objVerifyFailedAccountCount;
        #region Local Variable For Phone Verification
        
        public List<string> listOfAccount = new List<string>();
        public List<Thread> listOfWorkingThread = new List<Thread>();
        public List<string> listOfVerifiedAccount = new List<string>();
        public List<string> listOfFailToVerifyAccount = new List<string>();

        public string usernameOfAPI = string.Empty;
        public string passwordOfAPI = string.Empty;
     
        public int minDelay = 10;
        public int maxDelay = 20;
        public int NoOfThreadsVerifyAccount = 20;
        public int countVerifiedAccount = 0;
        public int countNotVerifiedAccount = 0;

        public bool isStopVerifingAccount = false;
        public bool IsMobileVerification = false;

        int countThreadControllerVerifyAccount = 0;

        public static int countThreadControllerVerifyAccountNew = 0;
        readonly object lockrThreadControllerCheckAccount = new object();
        #endregion

        IWebDriver webDriver = null;
        public void startMobileVerification()
        {
            try
            {
                if(isStopVerifingAccount)
                {
                    return;
                }
                try
                {
                    listOfWorkingThread.Add(Thread.CurrentThread);
                    listOfWorkingThread = listOfWorkingThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch(Exception ex)
                {
                    GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                }

                countThreadControllerVerifyAccount = 0;
                int numberOfAccountPatch = 25;

                if (NoOfThreadsVerifyAccount > 0)
                {
                    numberOfAccountPatch = NoOfThreadsVerifyAccount;
                }

                List<List<string>> list_listAccounts = new List<List<string>>();

                if (IGGlobals.listAccounts.Count >= 1)
                {
                    GlobusLogHelper.log.Info("Account Verification Process Started ");
                    //var driverservice = ChromeDriverService.CreateDefaultService();
                    //driverservice.HideCommandPromptWindow = true;
                   
                    //webDriver = new ChromeDriver(driverservice, new ChromeOptions());
                    //webDriver.Manage().Window.Position=new Point(-2000, 0);
                                     

                    list_listAccounts = Utils.Split(IGGlobals.listAccounts, numberOfAccountPatch);
                    countThreadControllerVerifyAccountNew = 0;
                    foreach (List<string> listAccounts in list_listAccounts)
                    {
                        //int tempCounterAccounts = 0; 

                        foreach (string account in listAccounts)
                        {
                            try
                            {
                                lock (lockrThreadControllerCheckAccount)
                                {
                                    try
                                    {
                                        if (countThreadControllerVerifyAccount >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControllerCheckAccount);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;


                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);                                      

                                        if (item != null)
                                        {

                                            Thread profilerThread = new Thread(StartMultiThreadsAccountVerification);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;

                                            profilerThread.Start(new object[] { item });
                                           // profilerThread.Join();
                                            countThreadControllerVerifyAccount++;
                                        }
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
                        }                       
                    }
                                        
                }
            }
            catch(Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }
            finally
            {
                while (true)
                {
                    if (countThreadControllerVerifyAccount == 0)
                    {
                        GlobusLogHelper.log.Info("--------------------------------------------------");
                        GlobusLogHelper.log.Info("Process Completed");
                        break;
                    }
                }
                               
            }
        }
        public void StartMultiThreadsAccountVerification(object parameters)
        {
            try
            {
                countThreadControllerVerifyAccountNew++;
                if (!isStopVerifingAccount)
                {
                    try
                    {
                        listOfWorkingThread.Add(Thread.CurrentThread);
                        listOfWorkingThread.Distinct();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                    try
                    {
                        Array paramsArray = new object[1];
                        paramsArray = (Array)parameters;

                        InstagramUser objInstagramUser = (InstagramUser)paramsArray.GetValue(0);

                        if (!objInstagramUser.isloggedin)
                        {
                            GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();

                            objInstagramUser.globusHttpHelper = objGlobusHttpHelper;

                            //Login Process
                            AccountManager objAccountManager = new AccountManager();
                            //  TweetAccountManager objAccountManager = new TweetAccountManager();
                            GlobusLogHelper.log.Info("Account : " + objInstagramUser.username + "Is Logging ");
                            objAccountManager.LoginUsingGlobusHttp(ref objInstagramUser);
                        }
                        if (objInstagramUser.isloggedin)
                        {
                            StartActionVerifyAccountFromAPI(ref objInstagramUser);
                            //StartActionVerifyAccount(ref objInstagramUser);                         
                        }
                        else
                        {
                            StartActionVerifyAccountFromAPI(ref objInstagramUser);
                           // StartActionVerifyAccount(ref objInstagramUser); 
                            GlobusLogHelper.log.Info("Couldn't Login With Username : " + objInstagramUser.username);
                            GlobusLogHelper.log.Debug("Couldn't Login With Username : " + objInstagramUser.username);
                        }
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
            finally
            {
                try
                {
                    countThreadControllerVerifyAccountNew--;
                    lock (lockrThreadControllerCheckAccount)
                    {
                        countThreadControllerVerifyAccount--;
                        Monitor.Pulse(lockrThreadControllerCheckAccount);
                    }

                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }
        private void StartActionVerifyAccount(ref InstagramUser IgUser)
        {
            lock (this)
            {
                try
                {
                    if (isStopVerifingAccount)
                    {
                        return;
                    }
                    try
                    {
                        listOfWorkingThread.Add(Thread.CurrentThread);
                        listOfWorkingThread = listOfWorkingThread.Distinct().ToList();
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                    }

                    if (listOfAccount.Count > 0)
                    {
                        //if (string.IsNullOrEmpty(usernameOfAPI) || string.IsNullOrEmpty(passwordOfAPI))
                        //{
                        //   // GlobusLogHelper.log.Info("Please Enter Username And Password Of API");
                        //}
                        //else
                        {
                            if (string.IsNullOrEmpty(IgUser.proxyport))
                            {
                                IgUser.proxyport = "80";
                            }
                            /// login to Api 
                            /// 
                            string homePageResponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.instagram.com/"), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword); //https://www.instagram.com/integrity/checkpoint/?next=%2F

                            string securityCheckPagesource = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.instagram.com/integrity/checkpoint/?next=%2F"), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);


                            if (securityCheckPagesource.Contains("Enter your phone number. We&#39;ll text you a security code to make sure it&#39;s you."))
                            {
                            startAgain:
                                GlobusHttpHelper objGlobusshttphelper = new GlobusHttpHelper();
                                Queue<string> queueOfMobileNo = new Queue<string>();
                                Queue<string> queueOfMobileNoUrl = new Queue<string>();
                                string MobileNoHomePageSource = objGlobusshttphelper.getHtmlfromUrlProxy(new Uri("http://www.receive-sms-online.info/"), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);

                                try
                                {
                                    string[] getAllNumberDetails = System.Text.RegularExpressions.Regex.Split(MobileNoHomePageSource, "<a href=");
                                    if (getAllNumberDetails.Count() > 0)
                                    {
                                        foreach (string item in getAllNumberDetails)
                                        {
                                            if (item.Contains("read-sms.php") && item.Contains("Receive SMS USA"))
                                            {
                                                string mobileNo = string.Empty;
                                                string mobileNourl = string.Empty;

                                                mobileNourl = "http://www.receive-sms-online.info/" + Utils.getBetween(item, "\"", "\"");
                                                mobileNo = Utils.getBetween(item, "phone=", "\"");
                                                queueOfMobileNoUrl.Enqueue(mobileNourl);
                                                queueOfMobileNo.Enqueue(mobileNo);
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error : " + ex.Message);
                                }

                                if (queueOfMobileNoUrl.Count > 0)
                                {
                              
                                    foreach (string item in queueOfMobileNoUrl)
                                    {
                                        try
                                        {
                                            string phoneNo = queueOfMobileNo.Dequeue();

                                            string getcsrfmiddlewaretoken = Utils.getBetween(securityCheckPagesource, "csrfmiddlewaretoken\" value=\"", "\"");

                                            string phoneNoPostData = "csrfmiddlewaretoken=" + getcsrfmiddlewaretoken + "&phone_number=%2B" + phoneNo;
                                            string postUrl = "https://www.instagram.com/integrity/checkpoint/?next=%2F";

                                            string smsActivationPagesource = IgUser.globusHttpHelper.postFormDataProxy(new Uri(postUrl), phoneNoPostData, "https://www.instagram.com/integrity/checkpoint/?next=%2F ", "", "https://www.instagram.com", IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);

                                            if (smsActivationPagesource.Contains("Sorry, please choose a different phone number"))
                                            {
                                                continue;
                                            }

                                            var driverervice = PhantomJSDriverService.CreateDefaultService();
                                            driverervice.HideCommandPromptWindow = true;

                                            webDriver = new PhantomJSDriver(driverervice, new PhantomJSOptions());
                                            Thread.Sleep(5000);
                                            webDriver.Navigate().GoToUrl(item);

                                            string pageSource = webDriver.PageSource;
                                            if (string.IsNullOrEmpty(pageSource))
                                            {
                                                webDriver.Navigate().GoToUrl(item);
                                            }
                                          //  webDriver.Close();
                                           // webDriver.Dispose();
                                            string verificationCode = string.Empty;

                                            try
                                            {
                                                string[] getsmsData = System.Text.RegularExpressions.Regex.Split(pageSource, "<td>");
                                                bool isgotSms = false;

                                                foreach (string msg in getsmsData)
                                                {
                                                    if(msg.Contains("Instagram account"))
                                                    {
                                                        string smsCode = Utils.getBetween(msg, "</script>", "</td>");
                                                        int count = 0; int countsms = 0;
                                                        foreach (string data in smsCode.Split(' '))
                                                        {
                                                           
                                                            string sms = data.Replace(".", "").Replace(",", "");
                                                            System.Text.RegularExpressions.Regex IdCheck = new System.Text.RegularExpressions.Regex("^[0-9]*$");
                                                            if (IdCheck.IsMatch(sms))
                                                            {
                                                                count++;                                                                
                                                                if(count==1)
                                                                {
                                                                    verificationCode = data.Replace(".", "");
                                                                    isgotSms = true;
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    verificationCode = verificationCode + " " + sms;
                                                                    isgotSms = true;
                                                                    break;
                                                                }                                                                
                                                               
                                                            }
                                                            
                                                        }
                                                    }
                                                    if(isgotSms)
                                                    {
                                                        break;
                                                    }
                                                    
                                                }
                                                
                                            }
                                            catch(Exception ex)
                                            {
                                                GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
                                            }

                                            string finalPostData = "csrfmiddlewaretoken=" + getcsrfmiddlewaretoken + "&response_code=" + verificationCode;
                                            string finalPostUrl = "https://www.instagram.com/integrity/checkpoint/?next=%2F";
                                            string finalresponse = IgUser.globusHttpHelper.postFormDataProxy(new Uri(finalPostUrl), finalPostData, "https://www.instagram.com/integrity/checkpoint/?next=%2F ", "", "https://www.instagram.com", IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                            if (finalresponse.Contains("To verify your account, please enter the security code we texted you"))
                                            {
                                                continue;
                                               // goto startAgain;
                                            }
                                            if (finalresponse.Contains(IgUser.username) && !(finalresponse.Contains("To verify your account, please enter the security code we texted you")))
                                            {
                                                GlobusLogHelper.log.Info(IgUser.username + " Account has been Verified");
                                                break;
                                            }

                                            string homepage = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.instagram.com/"), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);

                                            if (!homepage.Contains("logged-in") && !homepage.Contains(IgUser.username))
                                            {
                                                GlobusLogHelper.log.Info(IgUser.username + " Account is disable");
                                                break;
                                            }
                                            #region Commented to decode Aes Encoded data
                                            //string getSMSPageSource = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri(item), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                            //string[] getSMS = System.Text.RegularExpressions.Regex.Split(getSMSPageSource, " <tr>");
                                            //getSMS = getSMS.Take(3).ToArray();
                                            //getSMS = getSMS.Skip(1).ToArray();



                                            //foreach(string smsinfo in getSMS)
                                            //{
                                            //    try
                                            //    {
                                            //        if(smsinfo.Contains("<td>"))
                                            //        {
                                            //            string ct = string.Empty;
                                            //            string iv = string.Empty;
                                            //            string s = string.Empty;

                                            //            ct = Utils.getBetween(smsinfo, "ct\":\"", "\"");
                                            //            iv = Utils.getBetween(smsinfo, "iv\":\"", "\"");
                                            //            s = Utils.getBetween(smsinfo, "s\":\"", "\"");

                                            //           string value= DecryptString(ct, "ct");
                                            //          // using (Aes aes = new AesManaged())//RijndaelManaged
                                            //           using (AesManaged aes = new AesManaged())
                                            //           {
                                            //               string text="Happy Propose Day";
                                            //               byte[] rowbyte = Encoding.Unicode.GetBytes(text);

                                            //               byte[] cipherText = null;
                                            //               byte[] plainText = null;
                                            //               byte[] IV=new byte[16];

                                            //               aes.BlockSize = 128;
                                            //               cipherText = Convert.FromBase64String(ct);
                                            //               IV=Convert.FromBase64String(iv); 

                                            //               aes.Padding = PaddingMode.None;   //PKCS7
                                            //               aes.KeySize = 128;          // in bits
                                            //               aes.Key = cipherText;  // 16 bytes for 128 bit encryption
                                            //               aes.IV = IV;   // AES needs a 16-byte IV
                                            //               // Should set Key and IV here.  Good approach: derive them from 
                                            //               // a password via Cryptography.Rfc2898DeriveBytes 



                                            //               using(MemoryStream ms=new MemoryStream())
                                            //               {
                                            //                   using(CryptoStream cs=new CryptoStream(ms,aes.CreateEncryptor(),CryptoStreamMode.Write))
                                            //                   {

                                            //                   }
                                            //               }

                                            //               using (MemoryStream ms = new MemoryStream())
                                            //               {
                                            //                   using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                                            //                   {
                                            //                      // cs.Write(cipherText, 0, cipherText.Length);
                                            //                       cs.Write(cipherText, 0, cipherText.Length);
                                            //                   }

                                            //                   plainText = ms.ToArray();
                                            //               }

                                            //               string sms = System.Text.Encoding.Unicode.GetString(plainText);

                                            //               string result = System.Text.Encoding.UTF8.GetString(plainText);

                                            //               result = Encoding.ASCII.GetString(plainText);


                                            //               Console.WriteLine(s);
                                            //           }

                                            //            string sss="";

                                            //        }
                                            //        else
                                            //        {
                                            //            continue;
                                            //        }
                                            //    }
                                            //    catch(Exception ex)
                                            //    {
                                            //        GlobusLogHelper.log.Error("Error : " + ex.Message);
                                            //    }
                                            //} 
                                            #endregion
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error : " + ex.Message);
                                        }
                                    }
                                }

                            }
                            else if (securityCheckPagesource.Contains(IgUser.username))
                            {
                                GlobusLogHelper.log.Info("Account : " + IgUser.username  + " Is Already Verified");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.Message);
                } 
            }
        }
        private void StartActionVerifyAccountFromAPI(ref InstagramUser IgUser)
        {
            try
            {
                if (string.IsNullOrEmpty(IgUser.proxyport))
                {
                    IgUser.proxyport = "80";
                }
                if (listOfAccount.Count>0)
                {
                    if (string.IsNullOrEmpty(usernameOfAPI))
                    {
                        GlobusLogHelper.log.Info("Please Enter Your API Key");
                    }
                    else
                    {                       
                        /// login to Api 
                        /// 
                        string homePageResponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.instagram.com/"), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword); //https://www.instagram.com/integrity/checkpoint/?next=%2F

                        string securityCheckPagesource = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.instagram.com/integrity/checkpoint/?next=%2F"), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);

                        if (securityCheckPagesource.Contains("Enter your phone number. We&#39;ll text you a security code to make sure it&#39;s you."))
                        {
                            string LoginApiRewsponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_balance&service=opt16&apikey=" + usernameOfAPI), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);

                            if (LoginApiRewsponse.Contains("response\":\"1\""))
                            {
                                string balance =Utils.getBetween(LoginApiRewsponse, "balance\":\"", "\"");
                                float currentBalance = float.Parse(balance);
                                if (currentBalance > 5)
                                {
                                    string getFreeCount = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_count&service=opt16&apikey=" + usernameOfAPI + "&service_id=instagram"), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                    if(getFreeCount.Contains("response\":\"1\""))
                                    {
                                        string countDataForInstagram=Utils.getBetween(getFreeCount,"counts Instagram\":\"","\"");
                                        if(int.Parse(countDataForInstagram)>0)
                                        {
                                        StartAgain :
                                            string getNumberResponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_number&country=ru&service=opt16&id=1&apikey=" + usernameOfAPI), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                            if(getNumberResponse.Contains("response\":\"1\""))
                                            {
                                                string number = Utils.getBetween(getNumberResponse, "number\":\"", "\"");
                                                string ID = Utils.getBetween(getNumberResponse, "id\":", ","); 

                                                /// send sms on phone number
                                                string getcsrfmiddlewaretoken = Utils.getBetween(securityCheckPagesource, "csrfmiddlewaretoken\" value=\"", "\"");

                                                string phoneNoPostData = "csrfmiddlewaretoken=" + getcsrfmiddlewaretoken + "&phone_number=%2B7"+number;
                                                string postUrl="https://www.instagram.com/integrity/checkpoint/?next=%2F";

                                                string smsActivationPagesource = IgUser.globusHttpHelper.postFormDataProxy(new Uri(postUrl), phoneNoPostData, "https://www.instagram.com/integrity/checkpoint/?next=%2F ", "", "https://www.instagram.com", IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);

                                                //string numberBanResponse = IgUser.globusHttpHelper.getHtmlfromUrl(new Uri("http://smspva.com/priemnik.php?metod=ban&service=opt16&id="+ID+"&apikey=" + System.Configuration.ConfigurationSettings.AppSettings["ApiKey"]));
                                                //if(numberBanResponse.Contains("response\":\"1\""))
                                                {
                                                    string getSmsresponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_sms&service=opt16&id=" + ID + "&apikey=" + usernameOfAPI), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                                    Thread.Sleep(30 * 1000);
                                                    getSmsresponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_sms&country=ru&service=opt16&id=" + ID + "&apikey=" + usernameOfAPI), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                                    if(getSmsresponse.Contains("response\":\"2\""))
                                                    {
                                                        Thread.Sleep(10 * 1000);
                                                        getSmsresponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_sms&country=ru&service=opt16&id=" + ID + "&apikey=" + usernameOfAPI), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                                        if(!getNumberResponse.Contains("response\":\"1\""))
                                                        {
                                                            Thread.Sleep(5 * 1000);
                                                            getSmsresponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_sms&country=ru&service=opt16&id=" + ID + "&apikey=" + usernameOfAPI), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                                            if (!getNumberResponse.Contains("response\":\"1\""))
                                                            {
                                                                string numberBanResponse = IgUser.globusHttpHelper.getHtmlfromUrl(new Uri("http://smspva.com/priemnik.php?metod=ban&service=opt16&id=" + ID + "&apikey=" + usernameOfAPI));
                                                                goto StartAgain;
                                                            }
                                                        }                                                        
                                                    }
                                                    else if (getSmsresponse.Contains("response\":\"3\""))
                                                    {
                                                        Thread.Sleep(10 * 1000);
                                                        getSmsresponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_sms&country=ru&service=opt16&id=" + ID + "&apikey=" + usernameOfAPI), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                                        if (!getNumberResponse.Contains("response\":\"1\""))
                                                        {
                                                            Thread.Sleep(5 * 1000);
                                                            getSmsresponse = IgUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://smspva.com/priemnik.php?metod=get_sms&country=ru&service=opt16&id=" + ID + "&apikey=" + usernameOfAPI), IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                                            if (!getNumberResponse.Contains("response\":\"1\""))
                                                            {
                                                                string numberBanResponse = IgUser.globusHttpHelper.getHtmlfromUrl(new Uri("http://smspva.com/priemnik.php?metod=ban&service=opt16&id=" + ID + "&apikey=" + usernameOfAPI));
                                                                goto StartAgain;
                                                            }
                                                        }     
                                                    }
                                                    if (getNumberResponse.Contains("response\":\"1\""))
                                                    {
                                                        string sms=string.Empty;
                                                        try
                                                        {
                                                            sms = Utils.getBetween(getSmsresponse, "sms\":\"", "\"");
                                                        }
                                                        catch(Exception ex)
                                                        {
                                                            GlobusLogHelper.log.Error("Error ==> " +ex.Message);
                                                        }
                                                        string finalPostData = "csrfmiddlewaretoken=" + getcsrfmiddlewaretoken + "&response_code=" + sms;
                                                        string finalPostUrl = "https://www.instagram.com/integrity/checkpoint/?next=%2F";
                                                        string finalresponse = IgUser.globusHttpHelper.postFormDataProxy(new Uri(finalPostUrl), finalPostData, "https://www.instagram.com/integrity/checkpoint/?next=%2F ", "", "https://www.instagram.com", IgUser.proxyip, int.Parse(IgUser.proxyport), IgUser.proxyusername, IgUser.proxypassword);
                                                        if(finalresponse.Contains("Enter your phone number."))
                                                        {
                                                            goto StartAgain;
                                                        }
                                                        if(finalresponse.Contains("logged-in") && finalresponse.Contains(IgUser.username))
                                                        {
                                                            countVerifiedAccount++;
                                                            GlobusLogHelper.log.Info("Account : " + IgUser.username + "Has Been Verified");
                                                            objVerifiedAccountCount(countVerifiedAccount);
                                                            listOfVerifiedAccount.Add(IgUser.username + ":" + IgUser.password + ":" + IgUser.proxyip + ":" + IgUser.proxyusername + ":" + IgUser.proxypassword);

                                                            Random randomNumber = new Random();
                                                            int delayAccutal = randomNumber.Next(minDelay, maxDelay);
                                                            GlobusLogHelper.log.Info("Delay For " + delayAccutal + " Seconds");
                                                            Thread.Sleep(delayAccutal * 1000);
                                                        }
                                                        else
                                                        {
                                                            countNotVerifiedAccount++;
                                                            GlobusLogHelper.log.Info("Account : " + IgUser.username + "not Verified");
                                                            objVerifyFailedAccountCount(countVerifiedAccount);
                                                            listOfFailToVerifyAccount.Add(IgUser.username + ":" + IgUser.password + ":" + IgUser.proxyip + ":" + IgUser.proxyusername + ":" + IgUser.proxypassword);

                                                            Random randomNumber = new Random();
                                                            int delayAccutal = randomNumber.Next(minDelay, maxDelay);
                                                            GlobusLogHelper.log.Info("Delay For " + delayAccutal + " Seconds");
                                                            Thread.Sleep(delayAccutal * 1000);
                                                        }
                                                    }
                                                }                                                
                                            }
                                            else
                                            {
                                                goto StartAgain;
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    GlobusLogHelper.log.Info("You Don't Have Enough Balance to Verify Account");
                                }
                            }
                        }
                        else if(securityCheckPagesource.Contains(IgUser.username))
                        {
                            GlobusLogHelper.log.Info("Account : " + IgUser.username  + " Is Already Verified");
                        }            
                       

                    } 
                }                              
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }       
        public static string DecryptString(string base64StringToDecrypt, string passphrase)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passphrase)))
            {
                byte[] RawBytes = Convert.FromBase64String(base64StringToDecrypt);
                ICryptoTransform ictD = acsp.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                //csD now contains original byte array, fully decrypted

                //return the content of msD as a regular string
                return (new StreamReader(csD)).ReadToEnd();
            }
        }
        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            // result.IV = RealKey;
            return result;
        }
        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }
    }


    public class MyDriverCalss 
    {
        private const int SW_SHOWMINIMIZED = 2;

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        public void hideChrome()
        {
            Process proc=null;
            
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Equals("chrome"))
                    proc = process;
            }

            IntPtr hWnd = proc.MainWindowHandle;
            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWMINIMIZED);
            }
        }
        public void Start()
        {
            Process myDriverService = new Process();
            myDriverService.StartInfo.CreateNoWindow = true;
        }
    }


}
