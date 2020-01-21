using GeoDataReporting.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoDataReporting.Controllers
{
    public class AppKeysController : Controller
    {
        private mSellerDemoLiveEntities
            db = new mSellerDemoLiveEntities();
        //
        // GET: /Find/
        [HttpGet]
        public ActionResult Companies()
        {
            //ViewBag.KeyName = new SelectList(getKeys(), "Value", "KeyName");
            return View();
        }
        [HttpPost]
        public ActionResult Companies(KeyValue param)
        {
            //ViewBag.KeyName = new SelectList(getKeys(), "Value", "KeyName", param.KeyName);
            var k = param.KeyName ?? "";
            param.Value = param.Value ?? "";
            var d = db.Database.SqlQuery<KeyValue>(@"SELECT Name AS CompanyName,CompanyId,CompanyCode,'" + k + "' AS KeyName,CONVERT(NVARCHAR(50),"
                    + k + ") AS Value FROM tblCompany WHERE IsActive=1 AND IsDeleted=0 AND " + k + " LIKE @p0 + '%'", param.Value);

            return View(d);
        }
        //private IEnumerable<KeyValue> getKeys()
        //{
        //    return new List<KeyValue>()
        //    {
        //        new KeyValue()
        //        {
        //            KeyName = "Name",
        //            Value="name",
        //        },
        //        new KeyValue()
        //        {
        //            KeyName = "CompanyCode",
        //            Value="CompanyCode",
        //        },
        //        new KeyValue()
        //        {
        //            KeyName = "Layout_No",
        //            Value="Layout_no",
        //        },
        //        new KeyValue()
        //        {
        //            KeyName = "Is Out of Stock",
        //            Value="IsOutofStock",
        //        },
        //        new KeyValue()
        //        {
        //            KeyName = "Image Char Replacement Enabled",
        //            Value="ImgCharReplacementEnabled",
        //        },
        //        new KeyValue()
        //        {
        //            KeyName = "Image Replacement Char",
        //            Value="ImgReplacementChar",
        //        }
        //    };
        //}
    }
    
}
