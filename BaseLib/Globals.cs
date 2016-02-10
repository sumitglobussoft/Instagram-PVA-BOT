using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BaseLibFB
{
    public class Globals
    {

        public static string CheckLicenseManager = string.Empty;
        public bool AdwordContentParser = false;
        public bool ContentExistenseParser = false;
        public bool EcommerceContentParser = false;
        public bool HomePageContentParser = false;
        public bool MarketingContentParser = false;
        public bool SiteExistenceParser = false;
        public bool SSlCertificateParser = false;
        public bool WebSiteHostContentParser = false;
        public bool WebSocialSiteContentParser = false;
        public bool BingContentParser = false;
        public bool GoogleContentParser = false;
        public bool SearchEnginesContentParser = false;
        public bool YahooContentParser = false;
        public bool SocialSiteContentPraser = false;

        public static string desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InstagramPVABot";
        public static string createdAccountTextFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InstagramPVABot\\Account.txt";
        public static string createdAccountCsvFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\InstagramPVABot\\CreatedAccount.csv";

        static public List<Thread> StopProxyCheckerModule = new List<Thread>();

        #region Multithreading settings
        public static int MaxThreadAllParser = 45;
        public static int threadAllParser = 15;
        public static int MaxThreadWebUrlParser = 45;
        public static int threadWebUrlParser = 15;
        #endregion

    }
}
