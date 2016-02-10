using Accounts;
using BaseLib;
using BaseLibID;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace UploadPic
{
    public class UploadImage
    {
        #region Local Variable For Upload  Image
        
        public List<string> listOfAccount = new List<string>();
        public List<Thread> listOfWorkingThread = new List<Thread>();
        public List<string> listOfImage = new List<string>();
        public List<string> listOfPostedImage = new List<string>();
        public List<string> listOfUploadedImage = new List<string>();

        public Queue<string> queueOfImage = new Queue<string>();

        public string usernameOfAPI = string.Empty;
        public string passwordOfAPI = string.Empty;

        public int minDelay = 10;
        public int maxDelay = 20;
        public int NoOfThreadsUploadImage = 20;

        public bool isStopUploadImage = false;
        public bool IsUploadProfilePic = false;
        public bool IsPostPic = false;

        int countThreadControllerUploadImage = 0;
        readonly object lockrThreadControllerUploadImage = new object();
        #endregion

        public void startUploadImage()
        {
            try
            {
                if (isStopUploadImage)
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

                countThreadControllerUploadImage = 0;
                int numberOfAccountPatch = 25;

                if (NoOfThreadsUploadImage > 0)
                {
                    numberOfAccountPatch = NoOfThreadsUploadImage;
                }

                List<List<string>> list_listAccounts = new List<List<string>>();

                if (IGGlobals.listAccounts.Count >= 1)
                {
                    
                    list_listAccounts = Utils.Split(IGGlobals.listAccounts, numberOfAccountPatch);

                    foreach (List<string> listAccounts in list_listAccounts)
                    {
                        //int tempCounterAccounts = 0; 

                        foreach (string account in listAccounts)
                        {
                            try
                            {
                                lock (lockrThreadControllerUploadImage)
                                {
                                    try
                                    {
                                        if (countThreadControllerUploadImage >= listAccounts.Count)
                                        {
                                            Monitor.Wait(lockrThreadControllerUploadImage);
                                        }

                                        string acc = account.Remove(account.IndexOf(':'));

                                        //Run a separate thread for each account
                                        InstagramUser item = null;


                                        IGGlobals.loadedAccountsDictionary.TryGetValue(acc, out item);

                                        if (item != null)
                                        {

                                            Thread profilerThread = new Thread(StartMultiThreadsUploadImage);
                                            profilerThread.Name = "workerThread_Profiler_" + acc;
                                            profilerThread.IsBackground = true;

                                            profilerThread.Start(new object[] { item });

                                            countThreadControllerUploadImage++;
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
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }
        }

        public void StartMultiThreadsUploadImage(object parameters)
        {
            try
            {
                if (!isStopUploadImage)
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
                            AccountManager objAccountManager = new AccountManager();
                           
                            objAccountManager.LoginUsingGlobusHttp(ref objInstagramUser);
                        }
                        if (objInstagramUser.isloggedin)
                        {
                            StartActionUploadImage(ref objInstagramUser);
                        }
                        else
                        {
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

                    lock (lockrThreadControllerUploadImage)
                    {
                        countThreadControllerUploadImage--;
                        Monitor.Pulse(lockrThreadControllerUploadImage);
                    }

                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
            }
        }

        public void StartActionUploadImage(ref InstagramUser objInstagramUser)
        {
            try
            {
                if(isStopUploadImage)
                {
                    return;
                }
                try
                {
                    listOfWorkingThread.Add(Thread.CurrentThread);
                    listOfWorkingThread=listOfWorkingThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
                if(IsUploadProfilePic)
                {
                    if (queueOfImage.Count > 0)
                    {            
                        string csrfToken=string.Empty;
                        string homePageresponse=objInstagramUser.globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://www.instagram.com/"),objInstagramUser.proxyip,int.Parse(objInstagramUser.proxyport),objInstagramUser.proxyusername,objInstagramUser.proxypassword);
                        try
                        {
                            csrfToken = Utils.getBetween(homePageresponse, "csrf_token\":\"", "\"");
                        }
                        catch(Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.Message);
                        }

                        string imageFilePath = queueOfImage.Dequeue();
                        NameValueCollection nvc=new NameValueCollection();
                        string imagename = Path.GetFileName(imageFilePath);

                        nvc.Add("profile_pic", "" + imageFilePath + "<:><:><:>image/jpeg");
                       

                        string postUrl = "https://www.instagram.com/accounts/web_change_profile_picture/";
                        string referer = "https://www.instagram.com/";

                        string response = objInstagramUser.globusHttpHelper.UploadprofilePicOnInstagram(ref objInstagramUser.globusHttpHelper, postUrl, csrfToken, "", imageFilePath, nvc, referer, objInstagramUser.proxyip, int.Parse(objInstagramUser.proxyport), objInstagramUser.proxyusername, objInstagramUser.proxypassword);

                        if(response.Contains("changed_profile\":true") && response.Contains("has_profile_pic\":true"))
                        {
                            GlobusLogHelper.log.Info("Profile Pic Changed For User : " + objInstagramUser.username + " Image Is : " + imageFilePath);

                            Random randomNumber = new Random();
                            int delayAccutal = randomNumber.Next(minDelay, maxDelay);
                            GlobusLogHelper.log.Info("Delay For " + delayAccutal + " Seconds");
                            Thread.Sleep(delayAccutal * 1000);
                        }
                        else if(response.Contains("changed_profile\":false") )
                        {
                            GlobusLogHelper.log.Info("Profile Pic Not Changed For User : " + objInstagramUser.username + " Image Is : " + imageFilePath);
                        }
                        if (response.Contains("has_profile_pic\":false"))
                        {
                            GlobusLogHelper.log.Info("Profile Pic Not Changed For User : " + objInstagramUser.username + " Image Is : " + imageFilePath);
                        }
                    }
                    else
                    {
                        GlobusLogHelper.log.Info("Image List Not Found!!");
                        return;
                    }
                }
                else if (IsPostPic)
                {
                    if (queueOfImage.Count > 0)
                    {
                        string imageFilePath = queueOfImage.Dequeue();
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
