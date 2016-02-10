﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;

namespace BaseLib
{
    public class Utils
    {
        public static Regex IdCheck = new Regex("^[0-9]*$");

        public string GetAssemblyVersion()
        {
            string appName = Assembly.GetAssembly(this.GetType()).Location;
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
            string versionNumber = assemblyName.Version.ToString();
            return versionNumber;
        }

        public static int GenerateRandom(int minValue, int maxValue)
        {
            int randomNo = 0;
            try
            {
                if (minValue <= maxValue)
                {
                    Random random = new Random();
                    randomNo = random.Next(minValue, maxValue);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return randomNo;
        }

        public int GetProcessor()
        {
            int processor = 1;
            try
            {

                processor = Environment.ProcessorCount;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return processor;
        }

        public static bool IsNumeric(string strInputNo)
        {
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(strInputNo) && IdCheck.IsMatch(strInputNo))
            {
                return true;
            }

            return false;
        }

        public static List<List<string>> Split(List<string> source, int splitNumber)
        {

            if (splitNumber <= 0)
            {
                splitNumber = 1;
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

        }

        public static string GenerateTimeStamp()
        {
            string strGenerateTimeStamp = string.Empty;
            try
            {
                // Default implementation of UNIX time of the current UTC time
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                strGenerateTimeStamp = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return strGenerateTimeStamp;
        }
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static ArrayList RandomNumbers(int max)
        {
            // Create an ArrayList object that will hold the numbers
            ArrayList lstNumbers = new ArrayList();

            try
            {
                // The Random class will be used to generate numbers
                Random rndNumber = new Random();

                // Generate a random number between 1 and the Max
                int number = rndNumber.Next(0, max + 1);
                // Add this first random number to the list
                lstNumbers.Add(number);
                // Set a count of numbers to 0 to start
                int count = 0;

                do // Repeatedly...
                {
                    // ... generate a random number between 1 and the Max
                    number = rndNumber.Next(0, max + 1);

                    // If the newly generated number in not yet in the list...
                    if (!lstNumbers.Contains(number))
                    {
                        // ... add it
                        lstNumbers.Add(number);
                    }

                    // Increase the count
                    count++;
                } while (count <= 10 * max); // Do that again

                /////Casting from ArrayList to List<string>
                //List<int> randomNoList = new List<int>();
                //int[] tempArr = (int[])lstNumbers.ToArray();
                //randomNoList = tempArr.ToList();

                // Once the list is built, return it
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }


            return lstNumbers;
        }

        public static string UploadFolderData(string DirectoryPath)
        {
            string FilePath = string.Empty;
            try
            {
                using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                {
                    ofd.SelectedPath = Application.StartupPath;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        FilePath = ofd.SelectedPath;
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }

            return FilePath;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static string GetParameter(string item, string name)
        {
            string value = "";
            try
            {
                int startindex = item.IndexOf(name + "=\"");
                string start = item.Substring(startindex).Replace(name + "=\"", "");
                int endindex = start.IndexOf("\"");
                string end = start.Substring(0, endindex);
                value = end;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }

            if (string.IsNullOrEmpty(value))
            {
                try
                {
                    int startindex = item.IndexOf(name + "='");
                    string start = item.Substring(startindex).Replace(name + "='", "");
                    int endindex = start.IndexOf("'");
                    string end = start.Substring(0, endindex);
                    value = end;
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
                }
                
            }
            return value;
           
        }

        public static string GetData(string Responce, string Value)
        {
            string data = string.Empty;
            try
            {
                string[] DataList = System.Text.RegularExpressions.Regex.Split(Responce, Value);


                foreach (string Dataitem in DataList)
                {
                    if (!Dataitem.Contains("<!DOCTYPE html>"))
                    {
                        string Finaldata = Dataitem.Substring(Dataitem.IndexOf("value="), Dataitem.IndexOf(">") - Dataitem.IndexOf("value=")).Replace("value=", "").Replace("\"", "").Trim();
                        data = Finaldata;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error("Error : 59" + ex.Message);
            }
            return data;

        }
    }
}
