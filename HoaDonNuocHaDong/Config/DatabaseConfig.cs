﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace HoaDonNuocHaDong.Config
{
    public class DatabaseConfig
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ReportConString"].ConnectionString;
        public string getCurrentDatabaseInitialCatalog()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            string databaseInitialDialog = builder.InitialCatalog;
            if (!String.IsNullOrEmpty(databaseInitialDialog))
            {
                return databaseInitialDialog;
            }
            return "";
        }

        public static String getConnectionString(){
            return connectionString;
        }

        public static String getSubPath()
        {
            return "~/Backups/";
        }
        
    }
}