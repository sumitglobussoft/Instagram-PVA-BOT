using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public static class CustomMultithreadManagerDictionary
    {
        public static Dictionary<string, CustomMultithreadsManager> dictionary_CustomMultithreadsManager = new Dictionary<string, CustomMultithreadsManager>() { { "AllParsers", new CustomMultithreadsManager() }, { "DirectUrlParser", new CustomMultithreadsManager() }, { "AccountCreator", new CustomMultithreadsManager() } };
        //public static Dictionary<string, CustomMultithreadsManager> dictionary_CustomMultithreadsManager = new Dictionary<string, CustomMultithreadsManager>();

        //public static int countinstances_CustomMultithreadsManager = 0;

        public static void AddCustomMultithreadsManager(string name_CustomMultithreadsManager)
        {
            try
            {
                dictionary_CustomMultithreadsManager.Add(name_CustomMultithreadsManager, new CustomMultithreadsManager() { poolName = name_CustomMultithreadsManager });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
