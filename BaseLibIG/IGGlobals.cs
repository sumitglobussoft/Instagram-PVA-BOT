using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibID
{
 public sealed class IGGlobals
    {
          /// <summary>
        /// Contains all the accounts and related Information
        /// </summary>       
        // DBC Setting

        public static string dbcUserName = string.Empty;
        public static string dbcPassword = string.Empty;


        public static Dictionary<string, InstagramUser> loadedAccountsDictionary = new Dictionary<string, InstagramUser>();
        public static Dictionary<string, InstagramUser> loadedAccountsDictionaryForMobile = new Dictionary<string, InstagramUser>();

        #region Common URL

        public static List<string> listAccounts = new List<string>();
        public readonly string IGhomeurl = "http://websta.me/p/";
        public readonly string IGstagramurl = "http://web.stagram.com/";
        public readonly string IGstagramurl_2 = "http://web.stagram.com/p/";
        public readonly string IGStagram_api = "http://websta.me/api/comments/";
        public readonly string IGLikewebsta_api = "http://websta.me/api/like/";
        public readonly string IGTestURL = "http://websta.me/login";
        public readonly string IGApi_Remove_like = "http://websta.me/api/remove_like/";
        public readonly string IGWEP_HomePage = "http://websta.me/n/";
        public readonly string IGWEPME = "http://websta.me/";
        public readonly string IGApi_media = "https://api.instagram.com/v1/media/";

        #endregion

        #region For Account Module

        public readonly string IGInstagramurl = "https://www.instagram.com/";
        public readonly string IGInstagramurlsecond = "https://www.instagram.com/accounts/login/ajax/";
        public readonly string IGInstagramAuthorizeurl = "https://www.instagram.com/oauth/authorize/?client_id=9d836570317f4c18bca0db6d2ac38e29&redirect_uri=http://websta.me/&response_type=code&scope=comments+relationships+likes";
                                                          
        public readonly string IGWebstaFeedUrl = "http://websta.me/feed";
        
        
        #endregion

        #region For DirectMessage Module

        public readonly string IGiconosquareAuthorizeurl = "https://instagram.com/oauth/authorize?client_id=d9494686198d4dfeb954979a3e270e5e&redirect_uri=http%3A%2F%2Ficonosquare.com&response_type=code&scope=likes+comments+relationships";
        public readonly string IGiconosquareviewUrl = "http://iconosquare.com/viewer.php";
        public readonly string IGiconsquarecommentUrl = "http://iconosquare.com/comments.php";
        public readonly string IGiconsquaremessageUrl = "http://iconosquare.com/messages.php";
        public readonly string IGiconsquaremessagepostUrl = "http://iconosquare.com/message_post_autocomplete.php";
        public readonly string IGiconsquarecontrollerUrl = "http://iconosquare.com/controller_ajax.php";
        public readonly string IGiconsquaremesagecontactUrl = "http://iconosquare.com/messages_by_contact.php?c=";



        #endregion

        #region For follow module

        public readonly string IGFollowapiUrl = "http://websta.me/api/relationships/";
        public readonly string IGwebstaSearchUrl = "http://websta.me/search/";

        #endregion

        #region for Photo module

        public readonly string IGphotolikeurl = "http://websta.me/api/load_likes/";
        public readonly string IGWebstahomepageUrl = "http://websta.me";
        public readonly string IGUserfollowerurl = "http://websta.me/followed-by/";
        public readonly string IGwebtagurl = "http://websta.me/tag/";
        public readonly string IGwebstakeywordurl = "http://websta.me/keyword/";

        #endregion


        //String Flag values

        public readonly string registrationSuccessString = "\"registration_succeeded\":true";
        public readonly string registrationErrorString = "\"error\":";

        public readonly string IGhomepath = @"C:\InstagramPVABot";
        public readonly string IGdatapath = @"C:\InstagramPVABot\Data";
        public readonly string IGdbfilename = @"\InstagramPVABot.db";
   

        public bool isfreeversion = false;
        public static bool Check_likephoto_Byusername = false;


        /// <summary>
        /// Singleton object declaration.
        /// </summary>
        
        private static volatile IGGlobals globals = null;
        private static object syncRoot = new object(); 

        
        public static IGGlobals Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (globals == null)
                    {
                        globals = new IGGlobals();
                        
                    }
                }
            return globals;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IGGlobals()
        {
        }
    }
}
