using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using BaseLib;
using Chilkat;
using System.Text.RegularExpressions;

namespace RegexForScraping
{
    public static class ChilkatBasedRegex
    {
        public static List<string> GettingAllUrls(string PageSource, string MustMatchString)
        {
            List<string> suburllist = new List<string>();
            
            HtmlUtil htmlUtil = new HtmlUtil();
            PageSource = htmlUtil.EntityDecode(PageSource);
            StringArray datagoogle = htmlUtil.GetHyperlinkedUrls(PageSource);

            for (int i = 0; i < datagoogle.Length; i++)
            {
                string hreflink = datagoogle.GetString(i);

                if (hreflink.Contains(MustMatchString)) //&& hreflink.Contains("goback"))
                {
                    suburllist.Add(hreflink);
                }
            }
            return suburllist.Distinct().ToList();
        }

        
        public static List<string> GettingAllUrls1(string PageSource, string MustMatchString)
        {
            List<string> suburllist1 = new List<string>();
            Dictionary<string, string> categoryDictonsry = new Dictionary<string, string>();

            HtmlUtil htmlUtil = new HtmlUtil();
            PageSource = htmlUtil.EntityDecode(PageSource);
         
            try
            {
                string[] Dataconnection = System.Text.RegularExpressions.Regex.Split(PageSource, "<select id=\"cid");
                string DataImage = string.Empty;
                string[] datacon = System.Text.RegularExpressions.Regex.Split(PageSource, "<select id=\"cid");
                string []Arr = Regex.Split(datacon[1],"</option>");


                foreach (string item in Arr)
                {
                    if (!item.Contains("Show All Companies"))
                    {
                        if (item.Contains("option value"))//(item.Contains("search/profile/person?"))
                        {
                            string[] category = Regex.Split(item, ">");
                            string catId = category[0].Replace("<option value=", "").Replace("\n","").Replace("\"","").Replace("/","").Trim();
                            string catname = category[1];
                            //string value = item.Substring(item.IndexOf("<option value="), item.IndexOf("\"") - item.IndexOf("<option value=")).Trim().Replace("<option value=", "").Replace("\"", "");
                            string finalurl = catId + "," + catname;
                            //categoryDict
                           // string finalurl = item.Substring(item.IndexOf("\""), item.IndexOf(">") - item.IndexOf("\"")).Trim().Replace("class=", "").Replace("\"","");
                           //string finalurl1 = "http://subscriber.zoominfo.com/zoominfo/" + finalurl;
                            suburllist1.Add(finalurl);
                            if(item.Contains("<select id=\"status"))
                            {
                                break;
                            }
                               
                        }
                    }
                }
            }
            
            catch { }
            return suburllist1.Distinct().ToList();

           }

        public static List<string> GettingAllprname(string PageSource, string MustMatchString)
        {
            List<string> suburllist1 = new List<string>();
            Dictionary<string, string> categoryDictonsry = new Dictionary<string, string>();

            HtmlUtil htmlUtil = new HtmlUtil();
            PageSource = htmlUtil.EntityDecode(PageSource);

            try
            {
                string[] Dataconnection = System.Text.RegularExpressions.Regex.Split(PageSource, "<select id=\"cid");
                string DataImage = string.Empty;
                string[] datacon = System.Text.RegularExpressions.Regex.Split(PageSource, "<select id=\"cid");
                string[] Arr = Regex.Split(datacon[1], "</option>");


                foreach (string item in Arr)
                {
                    if (!item.Contains("Show All Companies"))
                    {
                        if (item.Contains("option value"))//(item.Contains("search/profile/person?"))
                        {
                            string[] category = Regex.Split(item, ">");
                            string catId = category[0].Replace("<option value=", "").Replace("\n", "").Replace("\"", "").Replace("/", "").Trim();
                            string catname = category[1];
                            //string value = item.Substring(item.IndexOf("<option value="), item.IndexOf("\"") - item.IndexOf("<option value=")).Trim().Replace("<option value=", "").Replace("\"", "");
                            string finalurl = catId + "," + catname;
                            //categoryDict
                            // string finalurl = item.Substring(item.IndexOf("\""), item.IndexOf(">") - item.IndexOf("\"")).Trim().Replace("class=", "").Replace("\"","");
                            //string finalurl1 = "http://subscriber.zoominfo.com/zoominfo/" + finalurl;
                            suburllist1.Add(catname);
                            if (item.Contains("<select id=\"status"))
                            {
                                break;
                            }

                        }
                    }
                }
            }

            catch { }
            return suburllist1.Distinct().ToList();

        }
       
        public static List<string> GettingAllUrls2(string PageSource, string MustMatchString)
        {
            string CampaingnUrl = string.Empty;
            List<string> lstfinaldata_urls = new List<string>();

            List<string> templst = new List<string>();
            string[] dataArray = System.Text.RegularExpressions.Regex.Split(PageSource, "<a name=");
            //int startindex = response1.IndexOf("<tbody>");
            //int endindex = response1.IndexOf("</tbody>");
            string abc = string.Empty;
            try
            {
                int startindex = PageSource.IndexOf("<tbody>");
                int endindex = PageSource.IndexOf("</tbody>");
                abc = PageSource.Substring(startindex + 2, endindex - startindex - 4);
            }
            catch { }
            if (string.IsNullOrEmpty(abc))
            {
                try
                {
                    int startindex = PageSource.IndexOf("<tbody>");
                    int endindex = PageSource.IndexOf("</tbody>");
                    abc = PageSource.Substring(startindex + 4, endindex - startindex - 4);
                }
                catch { }
            }


            string[] dataArray1 = System.Text.RegularExpressions.Regex.Split(abc, "<a href");

           
            foreach (string href in dataArray)
            {

                if (!href.Contains("<!doctype html>"))
                {

                    string depth1URL = string.Empty;
                    if (href.Contains("http:"))// && href.Contains("/dp/") && href.Count() < 2000)
                    {
                        try
                        {
                            if (href.Contains("http:"))
                            {
                                string Coucnty = href.Substring(href.IndexOf("></a>"), href.IndexOf("</h1>") - href.IndexOf("></a>")).Trim().Replace("</h1>", "").Replace("></a>", "");
                                string[] Countryurl = System.Text.RegularExpressions.Regex.Split(href, "<a href");
                                foreach (string item in Countryurl)
                                {
                                    if (!item.Contains("colmask"))
                                    {
                                        CampaingnUrl = item.Substring(0, item.IndexOf("\">")).Replace("\"", ""); // filtered all home page url //
                                        int startIndx = item.IndexOf("http:");
                                        int endIndx = item.IndexOf("\">", startIndx);
                                        CampaingnUrl = item.Substring(startIndx, endIndx - startIndx).Replace("\"", "");
                                        //lstfinaldata_urls.Add(SyandicationURL);
                                        // string country = CampaingnUrl.Substring(CampaingnUrl.IndexOf("http://"), CampaingnUrl.IndexOf(".craigslist") - CampaingnUrl.IndexOf("http://")).Replace("http://", "").Trim();


                                        templst.Add(CampaingnUrl + "*" + Coucnty);

                                        
                                    }
                                   
                                    
                                }
                              


                         
                            }

                        }

                        catch { };

                    }


                }

            }
            return templst.Distinct().ToList();

        }

      }
  }

        
      
  