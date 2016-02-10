using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotGuruz.MantaLeadPro.DataControler
{
   public class Cls_BGSiteInfo
    {
        public string Site_Name { get; set; }
    }

    public class SiteTable
    {
        public string Site_Name { get; set; }
        public string Site_Url { get; set; }
        public long SiteId { get; set; }
        public string Cat { get; set; }
        public string SCat { get; set; }
        public string BCat { get; set; }
        public string StCat { get; set; }
        public string CCat { get; set; }
        public string SCat2 { get; set; }
    }
}
