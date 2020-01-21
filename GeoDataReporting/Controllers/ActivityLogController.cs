using GeoDataReporting.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoDataReporting.Controllers
{
    [Authorize]
    public class ActivityLogController : Controller
    {
        private mSellerDemoLiveEntities
            db = new mSellerDemoLiveEntities(); 

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Company()
        {
            ViewBag.CompanyId = new SelectList(db.tblCompanies.Where(c => c.isActive && !c.isDeleted)
                            .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
                            .Select(cc => new { cc.CompanyId, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
                            "CompanyId", "Name");
            return View();
        }
        public ActionResult User()
        {
            ViewBag.CompanyId = new SelectList(db.tblCompanies.Where(c => c.isActive && !c.isDeleted)
                            .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
                            .Select(cc => new { cc.CompanyId, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
                            "CompanyId", "Name");
            return View();
        }
        public ActionResult GeneralConfig()
        {
            ViewBag.CompanyId = new SelectList(db.tblCompanies.Where(c => c.isActive && !c.isDeleted)
                            .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
                            .Select(cc => new { cc.CompanyId, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
                            "CompanyId", "Name");
            return View();
        }
        public ActionResult ThemeConfig()
        {
            ViewBag.CompanyId = new SelectList(db.tblCompanies.Where(c => c.isActive && !c.isDeleted)
                            .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
                            .Select(cc => new { cc.CompanyId, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
                            "CompanyId", "Name");
            return View();
        }
        public JsonResult Logs(string path, string startsWith)
        {
           var list =  Directory.GetFiles(path, $"{startsWith}*.json").Reverse().Take(20);
           
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
