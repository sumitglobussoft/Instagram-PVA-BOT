using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotGuruz.MantaLeadPro.DataControler
{
  public class Cls_BGSubCategoryInfo
    {
   
        public long Site_Id { get; set; }
        public long Cat_id { get; set; }
        public long SubCat_id { get; set; }
        public string Cat_Name {get; set;}
    }

    public class SubCategoryTable
    {
        public long SubCat_id { get; set; }
        public string SubCat_Name { get; set; }

    }
}
