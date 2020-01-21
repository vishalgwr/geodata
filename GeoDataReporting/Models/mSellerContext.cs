using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GeoDataReporting.Models
{
    public class mSellerContext:DbContext
    {
         static string sDataSourceName = System.Configuration.ConfigurationManager.AppSettings["DataSourceName"].ToString();
        static string sDB_User = System.Configuration.ConfigurationManager.AppSettings["DB_User"].ToString();
        static string sDB_Password = System.Configuration.ConfigurationManager.AppSettings["DB_Password"].ToString();
        static string Database = System.Configuration.ConfigurationManager.AppSettings["Database"].ToString();
        public mSellerContext()
            : base(@"data source=" + sDataSourceName + ";initial catalog=" + Database + ";persist security info=True;user id=" + sDB_User + ";password=" + sDB_Password + ";multipleactiveresultsets=True;")
        {

        }

        public mSellerContext(string database)
            : base(@"data source=" + sDataSourceName + ";initial catalog=" + Database + ";persist security info=True;user id=" + sDB_User + ";password=" + sDB_Password + ";multipleactiveresultsets=True;")
        {

        }
        public DbSet<ActivityTrackingLog> ActivityTrackingLog { get; set; }
        public DbSet<ActivityTrackingUserAgent> ActivityTrackingUserAgent { get; set; }
    
        
    }
}