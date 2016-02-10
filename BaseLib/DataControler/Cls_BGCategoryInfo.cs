using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotGuruz.MantaLeadPro.DataControler
{
  public class Cls_BGCategoryInfo
    {
        public long Site_Id { get; set; }
        public long Cat_id { get; set; }
        public string Cat_Name {get; set;}
    }

    public class CategoryTable
    {
        public long Cat_id { get; set; }
        public string Cat_Name { get; set; }

    }
}