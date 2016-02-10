using BaseLib;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
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
using log4net.Config;
using System.IO;
using BaseLibID;
using FirstFloor.ModernUI.Presentation;
using BaseLibFB;

namespace Instagram_PVA_Bot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public static MainWindow mainFormReference = null;
        public MainWindow()
        {
            XmlConfigurator.Configure();
            InitializeComponent();
            mainFormReference = this;
           // chekLicense();
            CopyDataBase();
            AppearanceManager.Current.AccentColor = Colors.Brown;
            GlobusLogHelper.log.Info("Welcome To Instagram PVA Bot");
        }

        public void chekLicense()
        {
            #region FreeTrial
            try
            {

                ChilkatHttpHelpr objCjilkatHttpHelper = new ChilkatHttpHelpr();
                string strdateTime_DataBase = ("2016-02-06 23:59:59").ToString();// string strdateTime_DataBase = ("2016-02-04 23:59:59").ToString();

                DateTime dt = DateTime.Parse(strdateTime_DataBase);
                strdateTime_DataBase = dt.ToString("yyyy-MM-dd hh:mm:ss");

                string dateTime = objCjilkatHttpHelper.GetHtml("http://licensing.facedominator.com/licensing/FD/Datetime.php");

                DateTime dt_now = DateTime.Parse(dateTime);

                TimeSpan dt_Difference = dt_now.Subtract(dt);

                if (dt_Difference.Days >= 1)
                {
                    //ModernDialog.ShowMessage("Your Trial Version of Software Has Been Expired!!","Warning Message",MessageBoxButton.OK,this.OwnedWindows);  
                    MessageBox.Show("Your Trial Version of Software Has Been Expired!!");
                    mainFormReference.Close();
                    return;
                }
                

            }
            catch { }
            #endregion
        }
        private void CopyDataBase()
        {
            try
            {

                Directory.CreateDirectory(IGGlobals.Instance.IGhomepath);
                Directory.CreateDirectory(IGGlobals.Instance.IGdatapath);               
                string baseDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                string startUpDB = baseDir + "\\InstagramPVABot.db";
                string localAppDataDB = "C:\\InstagramPVABot\\Data\\InstagramPVABot.db";
                string startUpDB64 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\InstagramPVABot.db";

                string startPathOfUn = baseDir + "\\FirstName.txt";
                string localPathOfUn = "C:\\InstagramPVABot\\Data\\FirstName.txt";

                string startPathOfLn = baseDir + "\\LastName.txt";
                string localPathOfLn = "C:\\InstagramPVABot\\Data\\LastName.txt";

                if (!File.Exists(localPathOfUn))
                {
                    if (File.Exists(startPathOfUn))
                    {
                        try
                        {
                            File.Copy(startPathOfUn, localPathOfUn);

                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Could not find a part of the path"))
                            {
                                Directory.CreateDirectory(IGGlobals.Instance.IGhomepath);
                                Directory.CreateDirectory(IGGlobals.Instance.IGdatapath);
                                File.Copy(startPathOfUn, localPathOfUn);
                            }
                        }
                    }
                }
                if (!File.Exists(localPathOfLn))
                {
                    if (File.Exists(startPathOfLn))
                    {
                        try
                        {
                            File.Copy(startPathOfLn, localPathOfLn);

                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Could not find a part of the path"))
                            {
                                Directory.CreateDirectory(IGGlobals.Instance.IGhomepath);
                                Directory.CreateDirectory(IGGlobals.Instance.IGdatapath);
                                File.Copy(startPathOfLn, localPathOfLn);
                            }
                        }
                    }
                } 
                if (!File.Exists(localAppDataDB))
                {
                    if (File.Exists(startUpDB))
                    {
                        try
                        {
                            File.Copy(startUpDB, localAppDataDB);

                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Could not find a part of the path"))
                            {
                                Directory.CreateDirectory(IGGlobals.Instance.IGhomepath);
                                Directory.CreateDirectory(IGGlobals.Instance.IGdatapath);      
                                File.Copy(startUpDB, localAppDataDB);
                            }
                        }
                    }
                    else if (File.Exists(startUpDB64))   //for 64 Bit
                    {
                        try
                        {
                            File.Copy(startUpDB64, localAppDataDB);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Could not find a part of the path"))
                            {
                                Directory.CreateDirectory(IGGlobals.Instance.IGhomepath);
                                Directory.CreateDirectory(IGGlobals.Instance.IGdatapath);      
                                File.Copy(startUpDB64, localAppDataDB);
                            }
                        }
                    }
                }

                if (!Directory.Exists(Globals.desktopFolderPath))
                {
                    Directory.CreateDirectory(Globals.desktopFolderPath);
                }
                // txtDBpath.Text = localAppDataDB;
            }

            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
        }

    }

    #region LogFornetclass
    public class GlobusLogAppender : log4net.Appender.AppenderSkeleton
    {

        private static readonly object lockerLog4Append = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            try
            {
                string loggerName = loggingEvent.Level.Name;

                MainWindow frmInstagramPvaBot = MainWindow.mainFormReference;


                lock (lockerLog4Append)
                {
                    switch (loggingEvent.Level.Name)
                    {
                        case "DEBUG":
                            try
                            {

                                {
                                    if (!frmInstagramPvaBot.lstLogger.Dispatcher.CheckAccess())
                                    {
                                        frmInstagramPvaBot.lstLogger.Dispatcher.Invoke(new Action(delegate
                                        {
                                            try
                                            {
                                                if (frmInstagramPvaBot.lstLogger.Items.Count > 1000)
                                                {
                                                    frmInstagramPvaBot.lstLogger.Items.RemoveAt(frmInstagramPvaBot.lstLogger.Items.Count - 1);//.Add(frmDominator.listBoxLogs.Items.Add(loggingEvent.TimeStamp + "\t" + loggingEvent.LoggerName + "\r\t\t" + loggingEvent.RenderedMessage);
                                                }

                                                frmInstagramPvaBot.lstLogger.Items.Insert(0, loggingEvent.TimeStamp + "\t" + "Instagram PVA Bot" + "\r\t" + loggingEvent.RenderedMessage);
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                                            }

                                        }));

                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (frmInstagramPvaBot.lstLogger.Items.Count > 1000)
                                            {
                                                frmInstagramPvaBot.lstLogger.Items.RemoveAt(frmInstagramPvaBot.lstLogger.Items.Count - 1);
                                            }

                                            frmInstagramPvaBot.lstLogger.Items.Insert(0, loggingEvent.TimeStamp + "\t" + "Instagram PVA Bot" + "\r\t" + loggingEvent.RenderedMessage);
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error("Error : 74" + ex.Message);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error Case Debug : " + ex.StackTrace);
                                Console.WriteLine("Error Case Debug : " + ex.Message);
                                GlobusLogHelper.log.Error(" Error : " + ex.Message);
                            }
                            break;
                        case "INFO":
                            try
                            {


                                if (!frmInstagramPvaBot.lstLogger.Dispatcher.CheckAccess())
                                {
                                    frmInstagramPvaBot.lstLogger.Dispatcher.Invoke(new Action(delegate
                                    {
                                        try
                                        {
                                            if (frmInstagramPvaBot.lstLogger.Items.Count > 1000)
                                            {
                                                frmInstagramPvaBot.lstLogger.Items.RemoveAt(frmInstagramPvaBot.lstLogger.Items.Count - 1);
                                            }

                                            frmInstagramPvaBot.lstLogger.Items.Insert(0, loggingEvent.TimeStamp + "\t" + "Instagram PVA Bot" + "\t\t" + loggingEvent.RenderedMessage);

                                            // frmFaceDominator.lstLogger.Items.Add(loggingEvent.TimeStamp + "\t" + "TWTDominator 5.0 " + "\t\t" + loggingEvent.RenderedMessage);

                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                                        }

                                    }));

                                }
                                else
                                {
                                    try
                                    {
                                        if (frmInstagramPvaBot.lstLogger.Items.Count > 1000)
                                        {
                                            frmInstagramPvaBot.lstLogger.Items.RemoveAt(frmInstagramPvaBot.lstLogger.Items.Count - 1);
                                        }

                                        frmInstagramPvaBot.lstLogger.Items.Insert(0, loggingEvent.TimeStamp + "\t" + "Instagram PVA Bot" + "\t\t" + loggingEvent.RenderedMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusLogHelper.log.Error("Error : 75" + ex.Message);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error Case INFO : " + ex.StackTrace);
                                Console.WriteLine("Error Case INFO : " + ex.Message);
                                GlobusLogHelper.log.Error(" Error : " + ex.Message);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : 76" + ex.Message);
            }

        }


    }
    #endregion
}
