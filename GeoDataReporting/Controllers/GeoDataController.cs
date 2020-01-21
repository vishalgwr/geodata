using GeoDataReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoDataReporting.Controllers
{
    [Authorize]
    public class GeoDataController : Controller
    {
        private dbGeodataEntities geodb = new dbGeodataEntities();
        private mSellerDemoLiveEntities db;

        public ActionResult Index()
        {
            db = new mSellerDemoLiveEntities();
            var dic = db.tblCompanies
                .Where(com =>
                !com.isDeleted)
                .Select(co => new { co.CompanyCode, co.Name })
                .ToDictionary(c => int.Parse(c.CompanyCode), c => c.Name);
            ViewBag.Companies = dic;
            
            var companies = geodb.GeoDataProcessedCompanies
                .OrderByDescending(c2=>c2.IsProcessing)
                .ThenByDescending(c2=>c2.EnableProcessing)
                .ThenBy(c2=>c2.CompanyCode)
                ;

            return View(companies);
        }
        [HttpPost]
        public JsonResult ToggleProcessing(int Id)
        {
            var c = geodb.GeoDataProcessedCompanies.Find(Id);
            c.EnableProcessing = !c.EnableProcessing;
            geodb.SaveChanges();
            return Json(c.EnableProcessing);
        }
        [HttpGet]
        public JsonResult ProcessingCompany()
        {
            var c = geodb.GeoDataProcessedCompanies.SingleOrDefault(c1=>c1.IsProcessing);
            
            return Json(c==null? 0 : c.CompanyCode,JsonRequestBehavior.AllowGet);
        }
    }
}
