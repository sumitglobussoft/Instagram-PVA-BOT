using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Globussoft;
using System.Threading;

namespace BaseLib
{
    public class ClGlobals
    {
        public static List<Thread> listAllThreads = new List<Thread>(); //Used to store threads, for stopping later

        public static bool IsFreeVersion = false;

        public static List<string> listAccounts = new List<string>();
        public static AccountMode accountMode;

        public static string DeCaptcherHost = string.Empty;
        public static string DeCaptcherPort = string.Empty;
        public static string DeCaptcherUsername = string.Empty;
        public static string DeCaptcherPassword = string.Empty;

        public static string DBCUsername = string.Empty;
        public static string DBCPassword = string.Empty;

        public static string _ChangeDefaultFolderPath = string.Empty;

        public static string Path_DesktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CLDominatorlic";

        public static string Path_ModuleFolder = Path_DesktopFolder + "\\Craigslist";

        public static string path_AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\CLDominatorlic";

        public static string Path_ErroLogFolder = path_AppDataFolder + "\\CraigslistErrorLog";

        public static string pathCapcthaLogin = Path_ModuleFolder + "\\CapcthaLoginError.txt";
        
        public static string path_ResentVerfication = Path_ModuleFolder + "\\ResentVerfication.txt";

        public static string path_NonResentVerfication = Path_ModuleFolder + "\\NonResentVerfication.txt";

        public static string path_VerifiedAccounts = Path_ModuleFolder + "\\VerifiedAccounts.txt";

        public static string path_NonVerifiedAccounts = Path_ModuleFolder + "\\NonVerifiedAccounts.txt";

        public static string path_NonAddConnectionEmail = Path_ModuleFolder + "\\NotAddConnectionEmail.txt";

        public static string path_CapcthaNotSolvedAccounts = Path_ModuleFolder + "\\CaptchaNotSolvedAccounts.txt";

        public static string path_AcceptInvitationEmail = Path_ModuleFolder + "\\AddAcceptInvitationEmail.txt";

        public static string path_CreatedAccounts1 = Path_ModuleFolder + "\\CraigslistCreatedAccounts.txt";

        public static string path_NonCreatedAccount1s = Path_ModuleFolder + "\\CraigslistNonCreatedAccounts.txt";

        public static string path_AlreadyCreatedEmails1 = Path_ModuleFolder + "\\CraigslistAlreadyCreatedEmails.txt";

        public static string path_CreateGroupNotConfirmedAccounts = Path_ModuleFolder + "\\CraigslistCreateGroup-NotConfirmedAccounts";

        public static string path_CreateGroupCreatedGroups = Path_ModuleFolder + "\\CraigslistCreateGroup-CreatedGroups";
       
        public static string path_AlreadyCreatedEmails
        {
            get
            {
                return Path_ModuleFolder + "\\CraigslistAlreadyCreatedEmails.txt";
            }
           
        }
        
        public static string path_NonCreatedAccounts
        {
            get
            {
                return Path_ModuleFolder + "\\CraigslistNonCreatedAccounts.txt";
            }

        }

        public static string path_SuccesfullyCreatedLDAccounts
        {
            get
            {
                return Path_ModuleFolder + "\\CraigslistCreatedAccounts.txt";
            }

        }

        public static string path_SuccesfullyCreatedAccounts1 = Path_ModuleFolder + "\\SuccesfullyCreatedAccounts.txt";

        //public static string path_SuccesfullyCreatedLDAccounts
        //{
        //    get
        //    {
        //        return Path_ModuleFolder + "\\SuccesfullyCreatedAccounts.txt";
        //    }

        //}

        public static string Path_ExsistingProxies1 = Path_ModuleFolder + "\\ExsistingProxies.txt";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\LinkedInDominator\\ExsistingProxies.txt";

        public static string Path_ExsistingProxies
        {
            get
            {
                return Path_ModuleFolder + "\\ExsistingProxies.txt";
            }

        }

        public static string Path_WorkingPvtProxies = Path_ModuleFolder + "\\ExsistingPrivateProxies.txt";

        public static string Path_Non_ExsistingProxies1 = Path_ModuleFolder + "\\NonExsistingProxies.txt";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\LinkedInDominator\\NonExsistingProxies.txt";

        public static string Path_Non_ExsistingProxies
        {
            get
            {
                return Path_ModuleFolder + "\\NonExsistingProxies.txt";
            }

        }

        public static string Path_WorkingAccount_AccountChecker
        {
            get
            {
                return Path_ModuleFolder + "\\WorkingAccount_AccountChecker.txt";
            }

        }

        public static string Path_NonWorkingAccount_AccountChecker
        {
            get
            {
                return Path_ModuleFolder + "\\NonWorkingAccount_AccountChecker.txt";
            }

        }

        public static string Path_ChangeYourPassword_AccountChecker
        {
            get
            {
                return Path_ModuleFolder + "\\ChangeYourPassword_AccountChecker.txt";
            }

        }

        public static string Path_TemporarilyRestrictedAccount_AccountChecker
        {
            get
            {
                return Path_ModuleFolder + "\\TemporarilyRestrictedAccount_AccountChecker.txt";
            }

        }

        public static string Path_ConfirmYourEmailAddress_AccountChecker
        {
            get
            {
                return Path_ModuleFolder + "\\ConfirmYourEmailAddress_AccountChecker.txt";
            }

        }

        public static string Path_EmailAddressOrPasswordDoesNotMatch_AccountChecker
        {
            get
            {
                return Path_ModuleFolder + "\\EmailAddressOrPasswordDoesNotMatch_AccountChecker.txt";
            }

        }

        public static string Path_SecurityVerification_AccountChecker
        {
            get
            {
                return Path_ModuleFolder + "\\SecurityVerification_AccountChecker.txt";
            }

        }

        public static string Path_ErrorFile_AccountChecker
        {
            get
            {
                return Path_ModuleFolder + "\\ErrorFile_AccountChecker.txt";
            }

        }

        public static string Path_ExistingEmail_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\ExistingEmail_EmailChecker.txt";
            }

        }

        public static string Path_NonExistingEmail_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\NonExistingEmail_EmailChecker.txt";
            }

        }

        public static string Path_SecurityVerification_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\SecurityVerification_EmailChecker.txt";
            }

        }

        public static string Path_ErrorFile_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\ErrorFile_EmailChecker.txt";
            }

        }

        public static string Path_OtherProblem_EmailChecker
        {
            get
            {
                return CreatePath_EmailChecker() + "\\OtherProblem_EmailChecker.txt";
            }

        }

        private static string CreatePath_EmailChecker()
        {
            string path = string.Empty;
            try
            {
                if (!Directory.Exists(ClGlobals.Path_ModuleFolder + "\\EmailChecker"))
                {
                    DirectoryInfo di = Directory.CreateDirectory(ClGlobals.Path_ModuleFolder + "\\EmailChecker");
                    di.Create();
                }
                path = ClGlobals.Path_ModuleFolder + "\\EmailChecker";
            }
            catch
            {
            }
            return path;

        }

        public static string path_ProfPicSuccess
        {
            get
            {
                return Path_ModuleFolder + "\\ProfilePicSuccess.txt";
            }

        }

        public static string path_ProfPicFail
        {
            get
            {
                return Path_ModuleFolder + "\\ProfilePicFail.txt";
            }

        }

        public static string path_FailLogin
        {
            get
            {
                return Path_ModuleFolder + "\\LoginFail.txt";
            }

        }
        public static string path_TempRestrictedLogin
        {
            get
            {
                return Path_ModuleFolder + "\\LoginTempRestricted.txt";
            }

        }
        public static string path_SuccessfulLogin
        {
            get
            {
                return Path_ModuleFolder + "\\LoginSuccessful.txt";
            }

        }


        public static string Path_ProxySettingErroLog = path_AppDataFolder + "\\ErrorProxySetting.txt";

        public static bool CapchaTag = false;
        public static string CapchaLoginID = string.Empty;
        public static string CapchaLoginPassword = string.Empty;

    
        public static void ExportDataCSVFile(string CSV_Header, string CSV_Content, string CSV_FilePath)
        {
            try
            {
                if (!File.Exists(CSV_FilePath))
                {
                    GlobusFileHelper.AppendStringToTextFile(CSV_FilePath, CSV_Header);
                }

                GlobusFileHelper.AppendStringToTextFile(CSV_FilePath, CSV_Content);
            }
            catch (Exception)
            {

            }
        }


        #region ErrorLogPaths
        public static string Path_LinkedinErrorLogs1 = Path_ModuleFolder+"\\ErrorLog.txt";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\LinkedInDominator\\ErrorLog.txt";

        public static string Path_LinkedinErrorLogs
        {
            get
            {
                return Path_ModuleFolder + "\\ErrorLog.txt";
            }

        }

        public static string Path_LinkedinAccountCraetionErrorLogs = Path_ErroLogFolder + "\\CraigslistErrorLog.txt";
        public static string Path_LinkedinProxySettingErrorLogs = Path_ErroLogFolder + "\\ProxySettingErrorLog.txt";
        public static string Path_LinkedinDefaultSave = Path_ErroLogFolder + "\\LDDefaultFolderPath.txt";
        #endregion

        public static void CreateFolder(string FolderPath)
        {
            try
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
            }
            catch
            {
            }
        }
    }

    public enum AccountMode
    {
        NoProxy, PublicProxy, PrivateProxy
    }

    
}
