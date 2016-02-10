using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BotGuruz.MantaLeadPro.DataControler
{
    class BG_DbBusinessDataTable
    {
        public static DataTable tblBusdt = new DataTable();

        #region getBusinessDataTable()
        public void getBusinessDataTable()  
        {
            #region Make data table for store Data
            tblBusdt.TableName = "Business_MasterEntryTable";
            DataColumn column;

            // Create new DataColumn 1, set DataType, ColumnName and add to DataTable.   
            column = new System.Data.DataColumn();
            column.DataType = System.Type.GetType("System.Int64");
            column.ColumnName = "site_id";
            tblBusdt.Columns.Add(column);

            // Create second column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.Int64");
            column.ColumnName = "cat_id";
            tblBusdt.Columns.Add(column);

            // Create second column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "cat_Name";
            tblBusdt.Columns.Add(column);

            // Create 3 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "Sub_CatId";
            tblBusdt.Columns.Add(column);

            // Create second column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Subcat_Name";
            tblBusdt.Columns.Add(column);

            // Create 4 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Sub_Cat2";
            tblBusdt.Columns.Add(column);

            // Create 5 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Sub_Cat3";
            tblBusdt.Columns.Add(column);

            // Create 6 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Sub_Cat4";
            tblBusdt.Columns.Add(column);

            // Create 7 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Sub_Cat5";
            tblBusdt.Columns.Add(column);

            // Create 8 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ScrapId";
            tblBusdt.Columns.Add(column);

            // Create 9 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "City";
            tblBusdt.Columns.Add(column);

            // Create 10 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "State";
            tblBusdt.Columns.Add(column);


            // Create 11 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Name";
            tblBusdt.Columns.Add(column);


            // Create 12 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Slogen";
            tblBusdt.Columns.Add(column);


            // Create 13 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Addr";
            tblBusdt.Columns.Add(column);


            // Create 14 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ZipCode";
            tblBusdt.Columns.Add(column);


            // Create 15 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Main_PhNo";
            tblBusdt.Columns.Add(column);


            // Create 16 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Fax_No";
            tblBusdt.Columns.Add(column);


            // Create 17 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Other_PhNo";
            tblBusdt.Columns.Add(column);


            // Create 18 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Toll_FreeNo";
            tblBusdt.Columns.Add(column);

            // Create 19 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Desc";
            tblBusdt.Columns.Add(column);

            // Create 20 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Site_Url";
            tblBusdt.Columns.Add(column);

            // Create 21 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Site_Email";
            tblBusdt.Columns.Add(column);

            // Create 22 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Long";
            tblBusdt.Columns.Add(column);

            // Create 23 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Lat";
            tblBusdt.Columns.Add(column);

            // Create 24 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl1";
            tblBusdt.Columns.Add(column);

            // Create 25 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl2";
            tblBusdt.Columns.Add(column);

            // Create 26 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl3";
            tblBusdt.Columns.Add(column);

            // Create 27 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl4";
            tblBusdt.Columns.Add(column);

            // Create 28 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl5";
            tblBusdt.Columns.Add(column);

            // Create 29 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl6";
            tblBusdt.Columns.Add(column);

            // Create 30 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl7";
            tblBusdt.Columns.Add(column);

            // Create 31 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl8";
            tblBusdt.Columns.Add(column);

            // Create 32 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl9";
            tblBusdt.Columns.Add(column);

            // Create 33 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl10";
            tblBusdt.Columns.Add(column);

            // Create 34 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl11";
            tblBusdt.Columns.Add(column);

            // Create 35 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl12";
            tblBusdt.Columns.Add(column);

            // Create 36 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Bus_Dtl13";
            tblBusdt.Columns.Add(column);

            // Create 37 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "route1";
            tblBusdt.Columns.Add(column);

            // Create 38 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "route2";
            tblBusdt.Columns.Add(column);

            // Create 39 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "route3";
            tblBusdt.Columns.Add(column);

            // Create 40 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "route4";
            tblBusdt.Columns.Add(column);

            // Create 41 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "route5";
            tblBusdt.Columns.Add(column);

            // Create 42 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "LogoUrl";
            tblBusdt.Columns.Add(column);

            // Create 43 column.
            column = new System.Data.DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Date";
            tblBusdt.Columns.Add(column);

            #endregion
        }
        #endregion
    }
}
