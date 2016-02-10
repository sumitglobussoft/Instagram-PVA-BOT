using AccountCreation;
using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AccountCreation.xaml
    /// </summary>
    public partial class AccountCreation : UserControl
    {
        public AccountCreation()
        {
            InitializeComponent();

            AccountCreationModule.objCreatedAccountAddToLabel = new addToLabel(addCreatedAccounttoLabel);
            AccountCreationModule.objFailedAccountAddToLabel = new addToLabel(addCreatedAccounttoLabel);
        }

        public void addFailedAccounttoLabel(int count)
        {
            try
            {
                lbl_AccountCreation_Report_NoOfAccountFailToCreate.Dispatcher.Invoke(new Action(delegate
                    {
                        lbl_AccountCreation_Report_NoOfAccountFailToCreate.Content = count.ToString();
                    }));
            }
            catch (Exception ex)
            {
               GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        public void addCreatedAccounttoLabel(int count)
        {
            try
            {
                lbl_AccountCreation_Report_NoOfAccountCreated.Dispatcher.Invoke(new Action(delegate
                {
                    lbl_AccountCreation_Report_NoOfAccountCreated.Content = count.ToString();
                }));
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }
        private void txt_Unfollow_DelayMin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_AccountCreation_BrowseAccountFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    GlobalDeclaration.objAccountCreation.QueueOfProxy.Clear();
                    txt_AccountCreation_LoadAccountFilePath.Text = dlg.FileName.ToString();
                    List<string> tempList= Globussoft.GlobusFileHelper.ReadFiletoStringList(dlg.FileName);
                    foreach(string item in tempList)
                    {
                        GlobalDeclaration.objAccountCreation.QueueOfProxy.Enqueue(item);
                    }
                }
                GlobusLogHelper.log.Info(" [ " + GlobalDeclaration.objAccountCreation.QueueOfProxy.Count + " ] Proxy Uploaded");
                lbl_AccountCreation_Report_NoOfAccountLoaded.Dispatcher.Invoke(new Action(() =>
                {
                    lbl_AccountCreation_Report_NoOfAccountLoaded.Content = GlobalDeclaration.objAccountCreation.QueueOfProxy.Count.ToString();
                }));
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void btn_AccountCreation_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(GlobalDeclaration.objAccountCreation.QueueOfProxy.Count>0)
                {
                    if(string.IsNullOrEmpty(txt_AccountCreation_DelayMin.Text))
                    {
                        GlobusLogHelper.log.Info("Please Enter Minimum Delay");
                        ModernDialog.ShowMessage("Please Enter Minimum Delay", "Account Creation", MessageBoxButton.OK);
                        return;
                    }
                    else
                    {
                        GlobalDeclaration.objAccountCreation.minDelay = int.Parse(txt_AccountCreation_DelayMin.Text);
                    }
                    if (string.IsNullOrEmpty(txt_AccountCreation_DelayMax.Text))
                    {
                        GlobusLogHelper.log.Info("Please Enter Maximum Delay");
                        ModernDialog.ShowMessage("Please Enter Maximum Delay", "Account Creation", MessageBoxButton.OK);
                        return;
                    }
                    else
                    {
                        GlobalDeclaration.objAccountCreation.maxDelay = int.Parse(txt_AccountCreation_DelayMax.Text);
                    }
                   if(string.IsNullOrEmpty(txt_AccountCreation_NoOfUserToCreateAccount.Text))
                   {
                       GlobusLogHelper.log.Info("Please Enter No Of Account To Create");
                       ModernDialog.ShowMessage("Please Enter No Of Account To Create", "Account Creation", MessageBoxButton.OK);
                       return;
                   }
                   else
                   {
                       GlobalDeclaration.objAccountCreation.NoOfAccounttoCreate = int.Parse(txt_AccountCreation_NoOfUserToCreateAccount.Text);
                   }

                    try
                    {
                        Thread objStartinstaAccountCreation = new Thread(() => GlobalDeclaration.objAccountCreation.StartinstaAccountCreation());
                        objStartinstaAccountCreation.Start();
                        //GlobalDeclaration.objAccountCreation.StartinstaAccountCreation();
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

        private void btn_AccountCreation_Report_ExportCreatedAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WPFFolderBrowser.WPFFolderBrowserDialog dialog = new WPFFolderBrowser.WPFFolderBrowserDialog();
                dialog.ShowDialog();
                string selectedPath = dialog.FileName;
                if(!string.IsNullOrEmpty(selectedPath))
                {
                    if(GlobalDeclaration.objAccountCreation.listOfCreatedAccount.Count>0)
                    {
                        foreach(string item in GlobalDeclaration.objAccountCreation.listOfCreatedAccount)
                        {
                            try
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, selectedPath + "\\CreatedAccount.txt");
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                            }
                        }
                        GlobusLogHelper.log.Info("Created Account File Exported To Path : " + selectedPath + "\\CreatedAccount.txt");
                    }
                    else
                    {
                        GlobusLogHelper.log.Info("Created Account List Is Empty");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

        private void btn_AccountCreation_Report_ExportFailtoCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WPFFolderBrowser.WPFFolderBrowserDialog dialog = new WPFFolderBrowser.WPFFolderBrowserDialog();
                dialog.ShowDialog();
                string selectedPath = dialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (GlobalDeclaration.objAccountCreation.listOfFailedAccount.Count > 0)
                    {
                        foreach (string item in GlobalDeclaration.objAccountCreation.listOfFailedAccount)
                        {
                            try
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, selectedPath + "\\FailToCreateAccount.txt");
                            }
                            catch (Exception ex)
                            {
                                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                            }
                        }
                        GlobusLogHelper.log.Info("Failed Account File Exported To Path : " + selectedPath + "\\FailToCreateAccount.txt");
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

        private void btn_AccountCreation_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                GlobalDeclaration.objAccountCreation.isStopCreatingAccount = true;
                List<Thread> lstTemp = new List<Thread>();
                lstTemp = GlobalDeclaration.objAccountCreation.listOfWorkingThread.Distinct().ToList();
                if (lstTemp.Count > 0)
                {
                    foreach (Thread item in lstTemp)
                    {
                        try
                        {
                            item.Abort();
                            GlobalDeclaration.objAccountCreation.listOfWorkingThread.Remove(item);
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
