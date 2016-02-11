using BaseLib;
using BaseLibFB.DataControler;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Instagram_PVA_Bot.Views
{
    /// <summary>
    /// Interaction logic for AccountVerification.xaml
    /// </summary>
    public partial class AccountVerification : UserControl
    {
        public AccountVerification()
        {
            InitializeComponent();
            PhoneVerification.PhoneVerification.objVerifiedAccountCount = new PhoneVerification.AddToLabel(addVerifiedAccounttoLabel);
            PhoneVerification.PhoneVerification.objVerifyFailedAccountCount = new PhoneVerification.AddToLabel(addFailedToverifyAccounttoLabel);
        }
        public void addFailedToverifyAccounttoLabel(int count)
        {
            try
            {
                lbl_AccountVerification_Report_NoOfAccountFailToVerify.Dispatcher.Invoke(new Action(delegate
                {
                    lbl_AccountVerification_Report_NoOfAccountFailToVerify.Content = count.ToString();
                }));
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        public void addVerifiedAccounttoLabel(int count)
        {
            try
            {
                lbl_AccountVerification_Report_NoOfAccountVerified.Dispatcher.Invoke(new Action(delegate
                {
                    lbl_AccountVerification_Report_NoOfAccountVerified.Content = count.ToString();
                }));
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        private void btn_AccountVerification_BrowseAccountFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread objLoadAccounts = new Thread(() => LoadAccounts());                
                objLoadAccounts.Start();
                //objLoadAccounts.Join();               
                //LoadAccounts();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        static int counter = 0;
        DateTime sTime = new DateTime();
        private void LoadAccounts()
        {
          
            //Globals.IsFreeVersion = true;           
            try
            {
                DataSet ds;

                DataTable dt = new DataTable();

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    sTime = DateTime.Now;
                    txt_AccountVerification_LoadAccountFilePath.Dispatcher.Invoke(new Action(delegate
                    {
                        txt_AccountVerification_LoadAccountFilePath.Text = dlg.FileName;
                    }));
                    
       
                    List<string> templist = GlobusFileHelper.ReadFile(dlg.FileName);
                    GlobalDeclaration.objPhoneVerification.listOfAccount = templist.ToList();

                    if (templist.Count > 0)
                    {
                        IGGlobals.loadedAccountsDictionary.Clear();
                        IGGlobals.listAccounts.Clear();
                    }
                    
                    foreach (string item in templist)
                    {
                        try
                        {
                            Thread objLoadAccountMultithreaded = new Thread(() => LoadAccountMultithreaded( item));
                            objLoadAccountMultithreaded.Start();                           
                            //LoadAccountMultithreaded(counter, item);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error ==> " + ex.Message);
                        }
                    }                 
                    try
                    {
                       
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
                Thread.Sleep(1 * 1000);
                while (true)
                {                    
                    //new CheckListBoxViewModel();
                    if (counter == 0)
                    {
                        DateTime eTime = DateTime.Now;

                        string timeSpan = (eTime - sTime).TotalSeconds.ToString();

                        GlobusLogHelper.log.Debug("Accounts Loaded : " + IGGlobals.listAccounts.Count.ToString() + " In " + timeSpan + " Seconds");

                        GlobusLogHelper.log.Info("Accounts Loaded : " + IGGlobals.listAccounts.Count.ToString() + " In " + timeSpan + " Seconds");
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1 * 1000);
                    }
                    
                }
            }

        }

        private void LoadAccountMultithreaded(string item)
        {            
                //counter = counter + 1;
            try
            {
                counter++;
                string account = item;
                string[] AccArr = account.Split(':');
                if (AccArr.Count() > 1)
                {
                    string accountUser = string.Empty;
                    accountUser = account.Split(':')[0];
                    string accountPass = account.Split(':')[1];
                    string proxyAddress = string.Empty;
                    string proxyPort = string.Empty;
                    string proxyUserName = string.Empty;
                    string proxyPassword = string.Empty;
                    string Status = string.Empty;

                    int DataCount = account.Split(':').Length;
                    if (DataCount == 2)
                    {
                        //Globals.accountMode = AccountMode.NoProxy;

                    }
                    else if (DataCount == 4)
                    {

                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                    }
                    else if (DataCount > 5 && DataCount < 7)
                    {
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                        proxyUserName = account.Split(':')[4];
                        proxyPassword = account.Split(':')[5];

                    }
                    else if (DataCount >= 7)
                    {
                        proxyAddress = account.Split(':')[2];
                        proxyPort = account.Split(':')[3];
                        proxyUserName = account.Split(':')[4];
                        proxyPassword = account.Split(':')[5];
                        Status = "Not Checked";
                    }
                    Status = "Not Checked";
                    // dt.Rows.Add(accountUser, accountPass, proxyAddress, proxyPort, proxyUserName, proxyPassword, Status);
                    // dt.Rows.Add(accountUser, accountPass, proxyAddress, proxyPort, proxyUserName, proxyPassword);

                    string insertQuery = "insert into AccountVerification(Username,Password,ProxyAddress,ProxyPort,ProxyUsername,ProxyPassword,Status) values ('" + accountUser + "','" + accountPass + "','" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "','" + Status + "') ";
                    BaseLib.DataBaseHandler.InsertQuery(insertQuery, "AccountVerification");


                    try
                    {
                        InstagramUser objInstagramUser = new InstagramUser(accountUser, accountPass, proxyAddress, proxyPort);
                        objInstagramUser.username = accountUser;
                        objInstagramUser.password = accountPass;
                        objInstagramUser.proxyip = proxyAddress;
                        objInstagramUser.proxyport = proxyPort;
                        objInstagramUser.proxyusername = proxyUserName;
                        objInstagramUser.proxypassword = proxyPassword;

                        IGGlobals.loadedAccountsDictionary.Add(objInstagramUser.username, objInstagramUser);
                        IGGlobals.listAccounts.Add(objInstagramUser.username + ":" + objInstagramUser.password + ":" + objInstagramUser.proxyip + ":" + objInstagramUser.proxyport + ":" + objInstagramUser.proxyusername + ":" + objInstagramUser.proxypassword);
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }

                    ///Set this to "0" if loading unprofiled accounts
                    ///
                    string profileStatus = "0";
                }
                else
                {
                    GlobusLogHelper.log.Info("Account has some problem : " + item);
                    GlobusLogHelper.log.Debug("Account has some problem : " + item);
                }
                lbl_AccountVerification_Report_NoOfAccountLoaded.Dispatcher.Invoke(new Action(() =>
                {
                    lbl_AccountVerification_Report_NoOfAccountLoaded.Content = IGGlobals.listAccounts.Count.ToString();
                }));
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            finally
            {
                counter--;
            }
                
               
            
        }

        private void btn_AccountVerification_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalDeclaration.objPhoneVerification.listOfAccount.Count > 0)
                {
                    if (string.IsNullOrEmpty(txt_AccountVerification_DelayMin.Text))
                    {
                        GlobusLogHelper.log.Info("Please Enter Minimum Delay");
                        ModernDialog.ShowMessage("Please Enter Minimum Delay", "Account Verification", MessageBoxButton.OK);
                        return;
                    }
                    else
                    {
                        GlobalDeclaration.objPhoneVerification.minDelay = int.Parse(txt_AccountVerification_DelayMin.Text);
                    }
                    if (string.IsNullOrEmpty(txt_AccountVerification_DelayMax.Text))
                    {
                        GlobusLogHelper.log.Info("Please Enter Maximum Delay");
                        ModernDialog.ShowMessage("Please Enter Maximum Delay", "Account Verification", MessageBoxButton.OK);
                        return;
                    }
                    else
                    {
                        GlobalDeclaration.objPhoneVerification.maxDelay = int.Parse(txt_AccountVerification_DelayMax.Text);
                    }
                    //if (string.IsNullOrEmpty(txt_AccountVerification_UsernameOfApi.Text))
                    //{
                    //    GlobusLogHelper.log.Info("Please Enter Username Of API");
                    //    ModernDialog.ShowMessage("Please Enter Username Of API", "Account Verification", MessageBoxButton.OK);
                    //    return;
                    //}
                    //else
                    //{
                    //    GlobalDeclaration.objPhoneVerification.usernameOfAPI = txt_AccountVerification_DelayMax.Text;
                    //}
                    //if (string.IsNullOrEmpty(txt_AccountVerification_PasswordOfApi.Text))
                    //{
                    //    GlobusLogHelper.log.Info("Please Enter Password Of API");
                    //    ModernDialog.ShowMessage("Please Enter Password Of API", "Account Verification", MessageBoxButton.OK);
                    //    return;
                    //}
                    //else
                    //{
                    //    GlobalDeclaration.objPhoneVerification.passwordOfAPI = txt_AccountVerification_PasswordOfApi.Text;
                    //}

                    try
                    {
                        Thread objstartMobileVerification = new Thread(() => GlobalDeclaration.objPhoneVerification.startMobileVerification());
                        objstartMobileVerification.Start();

                        //GlobalDeclaration.objPhoneVerification.startMobileVerification();
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

        private void btn_AccountVerification_Report_ExportVerifiedAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WPFFolderBrowser.WPFFolderBrowserDialog dialog = new WPFFolderBrowser.WPFFolderBrowserDialog();
                dialog.ShowDialog();
                string selectedPath = dialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (GlobalDeclaration.objPhoneVerification.listOfVerifiedAccount.Count > 0)
                    {
                        foreach (string item in GlobalDeclaration.objPhoneVerification.listOfVerifiedAccount)
                        {
                            try
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, selectedPath + "\\VerifiedAccount.txt");
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                            }
                        }
                        GlobusLogHelper.log.Info("Failed Account File Exported To Path : " + selectedPath + "\\VerifiedAccount.txt");
                    }
                    else
                    {
                        GlobusLogHelper.log.Info("Failed Account List Is Empty");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void btn_AccountVerification_Report_ExportFailtoVerifyAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WPFFolderBrowser.WPFFolderBrowserDialog dialog = new WPFFolderBrowser.WPFFolderBrowserDialog();
                dialog.ShowDialog();
                string selectedPath = dialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (GlobalDeclaration.objPhoneVerification.listOfFailToVerifyAccount.Count > 0)
                    {
                        foreach (string item in GlobalDeclaration.objPhoneVerification.listOfFailToVerifyAccount)
                        {
                            try
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, selectedPath + "\\DisableAccount.txt");
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                            }
                        }
                        GlobusLogHelper.log.Info("Failed Account File Exported To Path : " + selectedPath + "\\DisableAccount.txt");
                    }
                    else
                    {
                        GlobusLogHelper.log.Info("Failed Account List Is Empty");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void btn_AccountVerification_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                GlobalDeclaration.objAccountCreation.isStopCreatingAccount = true;
                List<Thread> lstTemp = new List<Thread>();
                lstTemp = GlobalDeclaration.objPhoneVerification.listOfWorkingThread.Distinct().ToList();
                if (lstTemp.Count > 0)
                {
                    foreach (Thread item in lstTemp)
                    {
                        try
                        {
                            item.Abort();
                            GlobalDeclaration.objPhoneVerification.listOfWorkingThread.Remove(item);
                        }
                        catch (Exception ex)
                        {
                            //Thread.ResetAbort();
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }
                    }
                    GlobusLogHelper.log.Info("Process Stopped !");
                    GlobusLogHelper.log.Debug("Process Stopped !");
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

        }
    }
}
