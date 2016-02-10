using BaseLib;
using BaseLibID;
using FirstFloor.ModernUI.Windows.Controls;
using Globussoft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
    /// Interaction logic for UploadImage.xaml
    /// </summary>
    public partial class UploadImage : UserControl
    {
        public UploadImage()
        {
            InitializeComponent();
        }

        private void btn_UploadImage_BrowseAccountFile_Click(object sender, RoutedEventArgs e)
        {
            LoadAccounts();
        }

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
                    DateTime sTime = DateTime.Now;
                    txt_UploadImage_LoadAccountFilePath.Text = dlg.FileName;

                    List<string> templist = GlobusFileHelper.ReadFile(dlg.FileName);
                    GlobalDeclaration.objPhoneVerification.listOfAccount = templist.ToList();

                    if (templist.Count > 0)
                    {
                        IGGlobals.loadedAccountsDictionary.Clear();
                        IGGlobals.listAccounts.Clear();
                    }
                    int counter = 0;
                    foreach (string item in templist)
                    {

                        counter = counter + 1;
                        try
                        {
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
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                        }

                    }


                    try
                    {
                        //new CheckListBoxViewModel();
                        DateTime eTime = DateTime.Now;

                        string timeSpan = (eTime - sTime).TotalSeconds.ToString();

                        lbl_UploadImage_Report_NoOfAccountLoaded.Dispatcher.Invoke(new Action(() =>
                        {
                            lbl_UploadImage_Report_NoOfAccountLoaded.Content = IGGlobals.listAccounts.Count.ToString();
                        }));

                        GlobusLogHelper.log.Debug("Accounts Loaded : " + IGGlobals.listAccounts.Count.ToString() + " In " + timeSpan + " Seconds");

                        GlobusLogHelper.log.Info("Accounts Loaded : " + IGGlobals.listAccounts.Count.ToString() + " In " + timeSpan + " Seconds");
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

        private void btn_UploadImage_BrowseImageFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> tempList = new List<string>();
                string timeSpan = string.Empty;
                WPFFolderBrowser.WPFFolderBrowserDialog ofd = new WPFFolderBrowser.WPFFolderBrowserDialog();

                Nullable<bool> result = ofd.ShowDialog();

                if (result == true)
                {
                    DateTime sTime = DateTime.Now;
                    txt_UploadImage_LoadImageFolderPath.Text = ofd.FileName;
                    tempList = GetImagePathFromFolder(ofd.FileName);
                    foreach (string item in tempList)
                    {
                        GlobalDeclaration.objUploadImage.queueOfImage.Enqueue(item);
                    }
                    try
                    {
                        DateTime eTime = DateTime.Now;
                        timeSpan = (eTime - sTime).TotalSeconds.ToString();                       
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                    }
                }
                GlobusLogHelper.log.Info("Image File Loaded : " + tempList.Count() + " In " + timeSpan + " Seconds");
                lbl_UploadImage_Report_NoOfPicLoaded.Dispatcher.Invoke(new Action(() =>
                {
                    lbl_UploadImage_Report_NoOfPicLoaded.Content = tempList.Count().ToString();
                }));
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        #region GetImagePathFromFolder
        private List<string> GetImagePathFromFolder(string folderPath)
        {
            List<string> _listTimageFilePath = new List<string>();

            string[] files = System.IO.Directory.GetFiles(folderPath);

            if (files.Length > 0)
            {
                foreach (string item in files)
                {
                    if (item.Contains(".ini"))
                    {
                        continue;
                    }
                    _listTimageFilePath.Add(item);
                }
            }
            else
            {
                _listTimageFilePath.Clear();
            }

            return _listTimageFilePath;
        }
        #endregion

        private void btn_UploadImage_Start_Click(object sender, RoutedEventArgs e)
        {
            if (IGGlobals.listAccounts.Count > 0)
            {
                if (string.IsNullOrEmpty(txt_UploadImage_DelayMin.Text))
                {
                    GlobusLogHelper.log.Info("Please Enter Minimum Delay");
                    ModernDialog.ShowMessage("Please Enter Minimum Delay", "Upload Image", MessageBoxButton.OK);
                    return;
                }
                else
                {
                    GlobalDeclaration.objUploadImage.minDelay = int.Parse(txt_UploadImage_DelayMin.Text);
                }
                if (string.IsNullOrEmpty(txt_UploadImage_DelayMax.Text))
                {
                    GlobusLogHelper.log.Info("Please Enter Maximum Delay");
                    ModernDialog.ShowMessage("Please Enter Maximum Delay", "Upload Image", MessageBoxButton.OK);
                    return;
                }
                else
                {
                    GlobalDeclaration.objUploadImage.maxDelay = int.Parse(txt_UploadImage_DelayMax.Text);
                }
                if (string.IsNullOrEmpty(txt_UploadImage_NoOfThreads.Text))
                {
                    GlobusLogHelper.log.Info("Please Enter No Of Thread");
                    ModernDialog.ShowMessage("Please Enter No Of Thread", "Upload Image", MessageBoxButton.OK);
                    return;
                }
                else
                {
                    GlobalDeclaration.objUploadImage.NoOfThreadsUploadImage = int.Parse(txt_UploadImage_NoOfThreads.Text);
                }
                if(string.IsNullOrEmpty(txt_UploadImage_LoadAccountFilePath.Text))
                {
                    GlobusLogHelper.log.Info("Please Load Account File");
                    ModernDialog.ShowMessage("Please Load Account File", "Upload Image", MessageBoxButton.OK);
                    return;
                }
               if(string.IsNullOrEmpty(txt_UploadImage_LoadImageFolderPath.Text))
               {
                   GlobusLogHelper.log.Info("Please Select Image Folder");
                   ModernDialog.ShowMessage("Please Select Image Folder", "Upload Image", MessageBoxButton.OK);
                   return;
               }
                if(rdoBtn_UploadImage_LoadProfilePic.IsChecked==false && rdoBtn_UploadImage_PostImage.IsChecked==false)
                {
                    GlobusLogHelper.log.Info("Please Select Option for Upload Profile Pic Or Post pic");
                    ModernDialog.ShowMessage("Please Select Option for Upload Profile Pic Or Post pic", "Upload Image", MessageBoxButton.OK);
                    rdoBtn_UploadImage_LoadProfilePic.Focus();
                    return;
                }
                else
                {
                    if(rdoBtn_UploadImage_LoadProfilePic.IsChecked==true)
                    {
                        GlobalDeclaration.objUploadImage.IsUploadProfilePic = true;
                    }
                    else if (rdoBtn_UploadImage_PostImage.IsChecked == true)
                    {
                        GlobalDeclaration.objUploadImage.IsPostPic = true;
                    }
                }

                try
                {
                    GlobalDeclaration.objUploadImage.startUploadImage();
                }
                catch(Exception ex)
                {
                    GlobusLogHelper.log.Error("Error ==> " + ex.StackTrace);
                }
            }
        }
    }
}
