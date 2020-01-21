using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoDataReporting.Models;

namespace GeoDataReporting.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();

        public ActionResult Index()
        {
            return View(db);
        }
        [HttpGet]
        public JsonResult expiringLicenses(int length = 6)
        {
            var expired = db.tblLicenses
                .GroupJoin(db.TblLicenseCompanies, lic => lic.LicenseId, licC => licC.License_ID, (l1, l2) =>
                       new { l1.Status, l1.EndDate, l1.CompanyName, l1.LicenseKey, l1.IsDeleted, l2 })
                //.SelectMany(no => new { Status = no.l1.Status, EndDate = no.l1.EndDate })
                .Where(li => li.Status == "Active" && !li.IsDeleted && li.EndDate != null && li.EndDate < DateTime.Today)
                .OrderByDescending(o => o.EndDate)
                .Take(length)
                .Select(l =>
                new
                {
                    l.CompanyName,
                    l.LicenseKey,
                    l.EndDate,
                    Companies = l.l2.Select(c => c.Company_Name)
                })
                .ToList()
                .Select(lii => new
                {
                    lii.LicenseKey,
                    lii.CompanyName,
                    RemainingDays = (
                    (lii.EndDate == null ? DateTime.Now.Date : lii.EndDate.Value)
                    - DateTime.Now).Days,
                    lii.Companies
                });
            var expiring = db.tblLicenses
                .GroupJoin(db.TblLicenseCompanies, lic => lic.LicenseId, licC => licC.License_ID, (l1, l2) =>
                       new { l1.Status, l1.EndDate, l1.CompanyName, l1.LicenseKey, l1.IsDeleted, l2 })
                //.SelectMany(no => new { Status = no.l1.Status, EndDate = no.l1.EndDate })
                .Where(li => li.Status == "Active" && !li.IsDeleted && li.EndDate != null && li.EndDate >= DateTime.Today)
                .OrderBy(o => o.EndDate)
                .Take(length)
                .Select(l =>
                new
                {
                    l.CompanyName,
                    l.LicenseKey,
                    l.EndDate,
                    Companies = l.l2.Select(c => c.Company_Name)
                })
                .ToList()
                .Select(lii => new
                {
                    lii.LicenseKey,
                    lii.CompanyName,
                    RemainingDays = (
                    (lii.EndDate == null ? DateTime.Now.Date : lii.EndDate.Value)
                    - DateTime.Now).Days,
                    lii.Companies
                });
            return Json(new { expired, expiring }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult versionWiseDevice()
        {
            var groups = db.GetUserDeviceDetail(null, null, false).GroupBy(d => d.AppVersion).Select(g => new { g.Key, Count = g.Count() }).OrderByDescending(o => o.Key).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult lastSyncUsers(int length = 6)
        {
            var groups = db.TblUsers.Where(u => u.IsActive && u.IsDeleted == false)
                .Join(db.tblCompanies, us => us.CompanyId, co => co.CompanyId, (us, co) => new { us.Last_Full_Sync, us.UserName, us.Rep_Id, co.CompanyCode, CompanyName = co.Name })
                .OrderByDescending(o => o.Last_Full_Sync)
                .Take(length)
                .Select(g => new { g.UserName, g.Rep_Id, g.Last_Full_Sync, g.CompanyName, g.CompanyCode })
                .ToList()
                .Select(u => new { u.UserName, u.Rep_Id, DaysAgo = DateTime.Now - u.Last_Full_Sync, u.CompanyName });
            return Json(groups, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult lastSentTnxUsers(int length = 6)
        {
            var groups = db.TblUsers.Where(u => u.IsActive && u.IsDeleted == false)
                .Join(db.tblCompanies, us => us.CompanyId, co => co.CompanyId, (us, co) => new { us.Last_Order_Transmission, us.UserName, us.Rep_Id, co.CompanyCode, CompanyName = co.Name })
                .OrderByDescending(o => o.Last_Order_Transmission)
                .Take(length)
                .Select(g => new { g.UserName, g.Rep_Id, g.Last_Order_Transmission, g.CompanyName, g.CompanyCode })
                .ToList()
                .Select(u => new { u.UserName, u.Rep_Id, DaysAgo = DateTime.Now - u.Last_Order_Transmission, u.CompanyName });
            return Json(groups, JsonRequestBehavior.AllowGet);
        }
        public JsonResult lastCreatedUsers(int Id)
        {
            var users = db.TblUsers.Join(db.tblCompanies, t1 => t1.CompanyId, t2 => t2.CompanyId, (r1, r2) =>
            new { r1.UserId, r1.Rep_Id, r1.UserName,r1.CreatedOn, r2.CompanyCode, CompanyName = r2.Name })
            .OrderByDescending(c => c.CreatedOn)
            .Take(Id)
            .ToList()
            .Select(r=>new { r.UserId,r.Rep_Id,r.UserName, DaysAgo = DateTime.Now - r.CreatedOn, CreatedOn = r.CreatedOn.ToString() , r.CompanyCode,r.CompanyName });

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}
