using GeoDataReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GeoDataReporting.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private mSellerDemoLiveEntities
            db = new mSellerDemoLiveEntities();

        public ActionResult VersionTracking(int? companyId, string appVersion, string serverName, string searchKeyword, int? DaysOld, bool includeDemo = false)
        {
            ViewBag.CompanyId = new SelectList(db.tblCompanies.Where(c => c.isActive && !c.isDeleted)
                            .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
                            .Select(cc => new { cc.CompanyId, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
                            "CompanyId", "Name");
            ViewBag.DaysOld = new List<SelectListItem>
            {
                new SelectListItem(){Text = "Today", Value="0" },
                new SelectListItem(){Text = "30 Days", Value="30" },
                new SelectListItem(){Text = "90 Days", Value="90" },
                new SelectListItem(){Text = "180 Days", Value="180" },
            };

            ViewBag.searchKeyword = searchKeyword;
            ViewBag.appVersion = appVersion;
            ViewBag.serverName = serverName;

            searchKeyword = string.IsNullOrEmpty(searchKeyword) ? null : searchKeyword.Trim();
            var dayTill = DaysOld == null ? DateTime.Now : DateTime.Now.Date.AddDays((-1 * DaysOld.Value));
            var res = db.GetUserDeviceDetail(companyId, null, includeDemo)
                .Where(u => (searchKeyword == null || u.LicenseKey == searchKeyword)
                && (DaysOld == null || u.CreatedOn > dayTill))
                .ToList();
             
            return View(res);
        }
        public ActionResult CompanyLog()
        {
            var list = db.tblCompanies
                .Join(db.tblAdminLogins, c => c.LastModifiedBy, a => a.Loginid, (cr, ar) => new { cr, ar })
                .Select(f => new
                {
                    CompanyId = f.cr.CompanyId,
                    CompanyCode = f.cr.CompanyCode,
                    Name = f.cr.Name,
                    isActive = f.cr.isActive,
                    isDeleted = f.cr.isDeleted,
                    ModifiedOn = f.cr.ModifiedOn,
                    CreateDate = f.cr.CreateDate,
                    LastModifiedBy = f.cr.LastModifiedBy,
                    LoginName = f.ar.LoginName
                })
                .ToList()
                .Select(f => new tblCompany()
                {
                    CompanyId = f.CompanyId,
                    CompanyCode = f.CompanyCode,
                    Name = f.Name,
                    isActive = f.isActive,
                    isDeleted = f.isDeleted,
                    ModifiedOn = f.ModifiedOn,
                    CreateDate = f.CreateDate,
                    LastModifiedBy = f.LastModifiedBy,
                    Address1 = f.LoginName
                })
                .OrderByDescending(d => d.ModifiedOn)
                .ToList();
            return View(list);
        }
        public ActionResult UserLog(int? CompanyId, bool? IncludeDemo)
        {
            ViewBag.CompanyId = new SelectList(db.tblCompanies.Where(c => c.isActive && !c.isDeleted)
                            .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
                            .Select(cc => new { cc.CompanyId, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
                            "CompanyId", "Name");

            var list = db.sp_UserLogs(IncludeDemo??false)
                .Where(cc => cc.CompanyId == CompanyId || CompanyId == null);

            return View(list);
        }
    }

}
