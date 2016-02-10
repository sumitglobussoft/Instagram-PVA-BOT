using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotGuruz.MantaLeadPro.DataControler
{
    class BG_Db_Class
    {
        public static string getConnectionString()
        {
            //Old Server
            //return "server=98.130.0.102;User Id=C230527_Botguruz;Persist Security Info=True;database=C230527_BotguruzApps;password=Bot1234";
           
            //New Server
            return "server=198.101.136.227;User Id=692531_botguru;Persist Security Info=True;database=692531_BotguruzApp;password=Bg_123456;Port=3306; Pooling=true; Min Pool Size=5;Max Pool Size=10; Connection Timeout=200;";

        }

        public static string getConnectionString_newDb()
        {
            return "server=198.101.136.227;User Id=692531_botguru;Persist Security Info=True;database=692531_BotguruzApp;password=Bg_123456;Port=3306; Pooling=true; Min Pool Size=5;Max Pool Size=10; Connection Timeout=200;";
        }


        public static string getNewconnectionstring()
        {
            return "server=144.76.69.198;User ID=root;Password=frompo1234;persist security info=False;initial catalog=bussinessinfo;Pooling=false;";
        }

        public static string getNewLocalconnectionstring()
        {
            return "Host=127.0.0.1;User ID=root;Password=ankit;persist security info=False;initial catalog=bussinessinfo;Pooling=false;";
        }

    }

}
