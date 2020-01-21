using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public static class GnSession
    {
        public static Int32 CustomerId
        {
            get
            {
                if (HttpContext.Current.Session["Customerid"] == null)
                    return 0;
                else
                    return Convert.ToInt32(HttpContext.Current.Session["Customerid"]);
            }
            set
            {
                HttpContext.Current.Session["Customerid"] = value;
            }

        }
        public static String CustomerName
        {
            get
            {
                if (HttpContext.Current.Session["CustomerName"] == null)
                    return "";
                else
                    return Convert.ToString(HttpContext.Current.Session["CustomerName"]);
            }
            set
            {
                HttpContext.Current.Session["CustomerName"] = value;
            }

        }
    }
