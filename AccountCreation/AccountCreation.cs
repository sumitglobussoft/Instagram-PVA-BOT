using Accounts;
using BaseLib;
using BaseLibFB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AccountCreation
{
    public delegate void addToLabel(int count);
    public class AccountCreationModule
    {
        public static addToLabel objCreatedAccountAddToLabel;
        public static addToLabel objFailedAccountAddToLabel;

        AccountManager accountManager = new AccountManager();      

        #region Local Variable For Account Creation
            
        public Queue<string> QueueOfProxy = new Queue<string>();
        public Queue<string> QueueOfFirstName= new Queue<string>();
        public Queue<string> QueueOfLastName = new Queue<string>();

        public List<Thread> listOfWorkingThread = new List<Thread>();
        public List<string> listOfFirstName = new List<string>();
        public List<string> listOfLastName = new List<string>();
        public List<string> listOfCreatedAccount = new List<string>();
        public List<string> listOfFailedAccount = new List<string>();

        public bool isStopCreatingAccount = false;
        public bool IsAccountCreation = false;
        public int countThreadControllerAccountCreation = 0;
        public readonly object lockrThreadControllerAccountCreation = new object();

        public int minDelay = 10;
        public int maxDelay = 20;
        public int noOfThread = 20;      
        public int NoOfAccounttoCreate = 0;
        public int noOfAccountCreated = 0;
        public int noOfAccountFailToCreate = 0;

        #endregion
        public void StartinstaAccountCreation()
        {
            try
            {
                if(isStopCreatingAccount)
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
                int countOfIterationHere = 0;

                if (QueueOfProxy.Count>0)
                {
                    GlobusLogHelper.log.Info("Account Creation Process Started");
                    string localPathFirstName="C:\\InstagramPVABot\\Data\\FirstName.txt";
                    string localPathLastName="C:\\InstagramPVABot\\Data\\LastName.txt";

                    if (File.Exists(localPathFirstName))
                    {
                        List<string> tempList = Globussoft.GlobusFileHelper.ReadFiletoStringList(localPathFirstName);                       
                       while(true)
                       {
                           
                           Random rn = new Random();
                           int count = rn.Next(tempList.Count);
                           listOfFirstName.Add(tempList[count]);
                           listOfFirstName=listOfFirstName.Distinct().ToList();
                           if(listOfFirstName.Count>=NoOfAccounttoCreate)
                           {
                               break;
                           }
                       }
                        foreach(string fn in listOfFirstName)
                        {
                            QueueOfFirstName.Enqueue(fn);
                        }
                    }
                    if (File.Exists(localPathLastName))
                    {
                        List<string> tempList = Globussoft.GlobusFileHelper.ReadFiletoStringList(localPathLastName);
                        while (true)
                        {

                            Random rn = new Random();
                            int count = rn.Next(tempList.Count);
                            listOfLastName.Add(tempList[count]);
                            listOfLastName = listOfLastName.Distinct().ToList();
                            if (listOfLastName.Count >= NoOfAccounttoCreate)
                            {
                                break;
                            }
                        }
                        if (listOfFirstName.Count > listOfLastName.Count)
                        {
                             listOfLastName.AddRange(listOfFirstName);
                        }
                        foreach (string ln in listOfLastName)
                        {
                            QueueOfLastName.Enqueue(ln);
                        }
                    }
                                     

                    while (true)
                    {
                        try
                        {

                            string proxy = QueueOfProxy.Dequeue();
                            lock (lockrThreadControllerAccountCreation)
                            {

                                try
                                {
                                    if (countThreadControllerAccountCreation >= 25)
                                    {
                                        Monitor.Wait(lockrThreadControllerAccountCreation);
                                    }

                                    Thread objStartCreateAccount = new Thread(() => StartCreateAccount(proxy));
                                    objStartCreateAccount.SetApartmentState(ApartmentState.STA);
                                    objStartCreateAccount.Start();
                                    countThreadControllerAccountCreation++;
                                }
                                catch (Exception ex)
                                {
                                    GlobusLogHelper.log.Error("Error ==>" + ex.Message);
                                }
                            }                           
                            if (QueueOfProxy.Count == 0)
                            {
                                GlobusLogHelper.log.Info(QueueOfProxy.Count + " Account Created ");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                        }
                          
                    }                 
                   
                }
               
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }

        }

        private void StartCreateAccount(string proxy)
        {
            try
            {
                if (isStopCreatingAccount)
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

                GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();
                #region SetProxy

                string emailAddress = string.Empty;
                string password = string.Empty;
                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUsername = string.Empty;
                string proxyPassword = string.Empty;
                string firstName = string.Empty;
                string lastName = string.Empty;
                string username = string.Empty;

                string[] Instagram_Proxy = proxy.Split(':');
                try
                {
                    if (Instagram_Proxy.Length == 2)
                    {
                        proxyAddress = Instagram_Proxy[0];
                        proxyPort = Instagram_Proxy[1];
                    }
                    else if (Instagram_Proxy.Length == 4)
                    {
                        proxyAddress = Instagram_Proxy[0];
                        proxyPort = Instagram_Proxy[1];
                        proxyUsername = Instagram_Proxy[2];
                        proxyPassword = Instagram_Proxy[3];
                    }

                    try
                    {
                        firstName = QueueOfFirstName.Dequeue();
                    }
                    catch (Exception ex)
                    {
                        firstName = "ABC";
                    }
                    try
                    {
                        lastName = QueueOfLastName.Dequeue();
                    }
                    catch (Exception ex)
                    {
                        lastName = "XYZ";
                    }

                    /// generate email Address
                    try
                    {
                        string []listOfNumber={"1","2","3","4","5","6","7","8","9","0"};
                        string []listOfSpecialChar={"_","."};
                        string []listOfDomain={"hotmail.com","gmail.com","mail.ru"};
                        Random randomno=new Random();
                        int countNo=randomno.Next(0,9);
                        int countChar=randomno.Next(0,1);
                        int countDomain=randomno.Next(0,2);
                        if(string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                        {

                        }
                        else
                        {
                            emailAddress = firstName + listOfSpecialChar[countChar] + lastName+listOfNumber[countNo] + "@" + listOfDomain[countDomain];
                            emailAddress = emailAddress.ToLower();
                        }
                    }
                    catch(Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                    }

                    ///generate password
                    try
                    {
                        string[] listOfNumberAndChar = { "1", "2", "3", "4","Q", "5", "6", "7","@", "8", "9", "0","a","b","c","_","d","e","f","g","h","q","w","e","r","t","y","s","p","o"};
                                                
                        for (int i = 0; i < 9; i++)
                        {
                            Random randomno = new Random();
                            int countNo = randomno.Next(0,29);
                            password =password + listOfNumberAndChar[countNo];
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                }


                #endregion

               // string homepageResponse = objGlobusHttpHelper.getHtmlfromUrl(new Uri("https://www.instagram.com/"));

                string mid = string.Empty;
                string csrf_token = string.Empty;
              
                objGlobusHttpHelper.gCookies = new CookieCollection();
                string CookieData = "mid=VoPEZwAEAAFp_r8Rr_nIwLEwCR59; csrftoken=bc622723a802f92aea9fdd5ff6d31150";

                System.Net.Cookie cookie = new System.Net.Cookie();
                cookie.Name = "mid";
                cookie.Value = "VoPEZwAEAAFp_r8Rr_nIwLEwCR59";
                cookie.Domain = "instagram.com";
                objGlobusHttpHelper.gCookies.Add(cookie);
                cookie.Name = "csrftoken";
                cookie.Value = "bc622723a802f92aea9fdd5ff6d31150";
                cookie.Domain = "instagram.com";
                objGlobusHttpHelper.gCookies.Add(cookie);

                Random rn = new Random();
                int index = 0;
                try
                {
                    // countOfIterationHere++;
                    if (emailAddress.Contains("@"))
                    {
                        try
                        {
                            string un = string.Empty;
                            string[] listOfNumberAndChar = { "1", "2", "3", "4", "Q", "5", "6", "7", "8", "9", "0", "a", "b", "c", "_", "d", "e", "f", "g", "h", "q", "w", "e", "r", "t", "y", "s", "p", "o" };
                            for (int j = 0; j <= 7; j++)
                            {
                                int countUN = rn.Next(0, 28);
                                un = un + listOfNumberAndChar[countUN];
                            }
                            string[] emailSplit = Regex.Split(emailAddress, "@");
                            index = rn.Next(0, 100000);
                            username = emailSplit[0] + index;
                            //username = lastName + "_" + un;
                            // username = username;
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                        }
                    }
                }
                catch { };

                if (string.IsNullOrEmpty(proxyPort))
                {
                    proxyPort = "80";
                }

                string postdata = "email=" + Uri.EscapeDataString(emailAddress) + "&password=" + Uri.EscapeDataString(password) + "&username=" + username + "&first_name=" + firstName.ToLower() + "+" + lastName.ToLower() + "&cb=AQDm7By-IlSfGo7BqYVS_go5hS1hAKru6WSNhyieOaAh2SIfYek_ozePLeb_G4LIxFInxIwY-QnHesRzSzb1yaijZRljz3L6HLenL4zHoAxiteji6mLANAngWkTiw7VVUtoFaaZ_Y5pNbUzKQem8g5yrzyZ7KngIA76L4h_Jod_WFszjR7V19W9_A2LjgvJYPXqEnOz8t37qjVdgcfVvYLpMxpGHy-5YHBE6phkKo3jfWA&qs=7%2C14%2C41%2C51%2C54%2C76%2C81%2C93%2C123%2C141%2C147%2C152%2C158%2C160%2C193%2C196%2C246%2C253%2C268%2C280%2C285%2C308%2C332%2C339%2C342%2C368%2C377%2C380%2C387%2C410%2C478%2C481%2C502%2C508%2C522%2C528%2C535%2C563%2C621%2C666%2C897%2C940%7C11%2C21%2C26%2C33%2C45%2C51%2C70%2C106%2C123%2C128%2C130%2C143%2C171%2C183%2C198%2C224%2C225%2C258%2C276%2C291%2C295%2C327%2C341%2C385%2C388%2C393%2C405%2C413%2C421%2C466%2C468%2C485%2C545%2C554%2C558%2C560%2C564%2C592%2C610%2C612%2C622%2C734%7C%7C19%2C20%2C30%2C40%2C55%2C61%2C78%2C80%2C93%2C129%2C132%2C137%2C149%2C150%2C167%2C199%2C231%2C237%2C241%2C244%2C334%2C353%2C356%2C363%2C366%2C375%2C389%2C394%2C405%2C421%2C423%2C424%2C434%2C439%2C450%2C456%2C459%2C481%2C498%2C502%2C575%2C702%7C0%2C20%2C39%2C49%2C51%2C64%2C66%2C100%2C124%2C140%2C143%2C169%2C190%2C200%2C237%2C257%2C262%2C278%2C306%2C308%2C324%2C377%2C378%2C383%2C416%2C418%2C442%2C447%2C454%2C474%2C489%2C491%2C520%2C538%2C546%2C567%2C595%2C596%2C656%2C659%2C772%2C801%7C1%2C6%2C22%2C28%2C30%2C35%2C49%2C54%2C58%2C69%2C73%2C80%2C97%2C141%2C226%2C231%2C245%2C259%2C293%2C314%2C337%2C350%2C354%2C386%2C391%2C396%2C398%2C415%2C436%2C471%2C500%2C501%2C506%2C513%2C552%2C557%2C595%2C632%2C651%2C654%2C689%2C757%7C1%2C33%2C35%2C48%2C54%2C58%2C65%2C107%2C115%2C129%2C216%2C258%2C264%2C266%2C272%2C282%2C286%2C296%2C313%2C358%2C365%2C367%2C372%2C374%2C398%2C413%2C422%2C432%2C436%2C438%2C452%2C463%2C464%2C499%2C500%2C530%2C574%2C593%2C661%2C663%2C670%2C821&guid=VoPEZwAEAAFp_r8Rr_nIwLEwCR59";
                string postUrl = "https://www.instagram.com/accounts/web_create_ajax/";
                Thread.Sleep(1000);     
                string postResult = objGlobusHttpHelper.postFormDataForInstaWithProxy(new Uri(postUrl), postdata, proxyAddress, int.Parse(proxyPort), proxyUsername, proxyPassword);
                
                if (postResult.Contains("created") && (!postResult.Contains("error")))
                {
                    GlobusLogHelper.log.Info("Account Created with username : " + username + " password : " + password + " emailId : " + emailAddress);
                    if (!File.Exists(Globals.createdAccountCsvFilePath))
                    {
                        string csvHeader = "EmailId,Password,Username,FirstName,Lastname,ProxyAddress,ProxyPort, ProxyUsername, ProxyPassword";
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(csvHeader, Globals.createdAccountCsvFilePath);
                    }
                    string csvData = emailAddress + "," + password + "," + username + "," + firstName + "," + lastName + "," + proxyAddress + "," + proxyPort + "," + proxyUsername + "," + proxyPassword;
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(csvData, Globals.createdAccountCsvFilePath);
                    try
                    {
                        string query = "insert into CreatedAccountTable(EmailId,Password,Username,FirstName,LastName,ProxyAddress,ProxyPort,ProxyUsername,ProxyPassword) values('" + emailAddress + "','" + password + "','" + username + "','" + firstName.ToLower() + "','" + lastName.ToLower() + "','" + proxyAddress + "','" + proxyPort + "','" + proxyUsername + "','" + proxyPassword + "')";
                        DataBaseHandler.InsertQuery(query, "CreatedAccountTable");
                        noOfAccountCreated++;
                        objCreatedAccountAddToLabel(noOfAccountCreated);
                        listOfCreatedAccount.Add(emailAddress + ":" + password + ":" + username + ":" + firstName + ":" + lastName + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword);
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                    }

                    Random random = new Random();
                    int delay = rn.Next(minDelay, maxDelay);
                    GlobusLogHelper.log.Info("Delay by " + delay + "Second");
                    Thread.Sleep(delay * 1000);
                   
                }
                else if (postResult.Contains("Another account is using " + emailAddress))
                {
                    GlobusLogHelper.log.Info("Account Not Created with email : " + emailAddress + "  because Another account is using " + emailAddress);
                    noOfAccountFailToCreate++;
                    objFailedAccountAddToLabel(noOfAccountFailToCreate);
                    listOfFailedAccount.Add(emailAddress + ":" + password + ":" + username + ":" + firstName + ":" + lastName + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword);

                    Random random = new Random();
                    int delay = rn.Next(minDelay, maxDelay);
                    GlobusLogHelper.log.Info("Delay by " + delay + "Second");
                    Thread.Sleep(delay * 1000);
                }
                else if (postResult.Contains("error"))
                {
                    GlobusLogHelper.log.Info("Account Created Fail with email : " + emailAddress + "  because " + postResult);
                    noOfAccountFailToCreate++;
                    objFailedAccountAddToLabel(noOfAccountFailToCreate);
                    listOfFailedAccount.Add(emailAddress + ":" + password + ":" + username + ":" + firstName + ":" + lastName + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword);

                    Random random = new Random();
                    int delay = rn.Next(minDelay, maxDelay);
                    GlobusLogHelper.log.Info("Delay by " + delay + "Second");
                    Thread.Sleep(delay * 1000);
                }
                else
                {
                    GlobusLogHelper.log.Info("could not create account with email : " + emailAddress);
                    noOfAccountFailToCreate++;
                    objFailedAccountAddToLabel(noOfAccountFailToCreate);
                    listOfFailedAccount.Add(emailAddress + ":" + password + ":" + username + ":" + firstName + ":" + lastName + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword);
                    Random random = new Random();
                    int delay = rn.Next(minDelay, maxDelay);
                    GlobusLogHelper.log.Info("Delay by " + delay + "Second");
                    Thread.Sleep(delay * 1000);
                }

            }
            catch(Exception ex) 
            {
                GlobusLogHelper.log.Error("Error ==> " + ex.Message);
            }
            finally
            {
                try
                {
                    lock(lockrThreadControllerAccountCreation)
                    {
                        countThreadControllerAccountCreation--;
                        Monitor.Pulse(lockrThreadControllerAccountCreation);
                    }
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                }
            }
            //return countOfIterationHere;
        }
    }
}
