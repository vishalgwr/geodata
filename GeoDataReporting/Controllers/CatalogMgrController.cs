using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GeoDataReporting.Models;

namespace GeoDataReporting.Controllers
{
    [Authorize]
    public class CatalogMgrController : Controller
    {
        private mSellerDemoLiveEntities
            db = new mSellerDemoLiveEntities();
        private CatalogManager mgr = new CatalogManager();

        public ActionResult Index(string CompanyId, string RepId)
        {
            ViewBag.CompanyId = new SelectList(db.tblCompanies.Where(c => c.isActive && !c.isDeleted)
                .Select(ccc => new { ccc.CompanyCode, ccc.CompanyId, ccc.Name, ccc.LocalPath }).ToList()
                .Select(cc => new { cc.CompanyCode, LocalPath = cc.LocalPath + "HandsetExport\\Ipad\\", cc.CompanyId, Name = cc.Name + "(" + cc.CompanyCode.ToString() + ")" }),
                "LocalPath", "Name");

            ViewBag.RepId = new SelectList(mgr.GetRepUsersPath(CompanyId).Select(r =>
            {
                var arr = r.Split('|');
                return new { Text = arr[1], Value = arr[0] };
            }), "Value", "Text");

            if (Request.HttpMethod == "POST")
            {
                var d = DateTime.Now;
                var res = mgr.evaluate(RepId, true);
                ViewBag.Path = $"C:\\mSellerUtilities\\CatalogMgr\\{mgr.Company}\\{mgr.RepId} {mgr.Timestamp}_kept.csv";
                ViewBag.LoadTime = (DateTime.Now - d);
                return View(res);
            }

            return View();
        }
        public JsonResult Evaluate(int Id)
        {
            CatalogManager mgr = new CatalogManager();
            var ext = mgr.evaluate("", true);
            return Json(new { count = ext.Count, List = ext }, JsonRequestBehavior.AllowGet);
        }
        public FileResult File(string path)
        {
            return File(path, "image/jpeg");
        }
        [HttpPost]
        public ActionResult Delete(string CompanyId, string RepId)
        {
            ViewBag.BackUrl = $"{Url.Action("Index")}?companyId={Url.Encode(CompanyId)}&repId={Url.Encode(RepId)}";
            mgr.DeleteFiles(mgr.evaluate(RepId), User.Identity.Name);
            return View();
        }

        public ActionResult Activities(string CompanyId)
        {
            var res = db.tblCompanies.Where(c => c.isActive && !c.isDeleted)
                .OrderByDescending(o => o.CreatedOn)
                .Select(r1 => new { r1.CompanyCode, r1.Name })
                .ToList()
                .Join(mgr.GetTrackingHistory(), h => Convert.ToInt32(h.CompanyCode), h2 => h2.CompanyCode, (cc, hh) =>
                    new UserActivityTracking()
                    {
                        CompanyCode = hh.CompanyCode,
                        RepId = hh.RepId,
                        LoggedInUser = hh.LoggedInUser,
                        ActivityDetail = cc.Name + "#" + hh.ActivityDetail,
                        CreatedOn = hh.CreatedOn
                    });

            return View(res);
        }
    }
}

//

