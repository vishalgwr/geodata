using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoDataReporting.Models;
using mSellerCommon;

namespace GeoDataReporting.Controllers
{
    [Authorize]
    public class BatchReportsController : Controller
    {
        //
        // GET: /BatchReports/
        mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();
        BatchReport bm = new BatchReport("iPad");

        public ActionResult Index(int? companyCode, int? repId, DateTime? fromDate, DateTime? toDate, string filter, int? search)
        {
            bool? isActive = null;
            if (filter == "active")
                isActive = true;
            else if (filter == "inactive")
                isActive = false;

            ViewBag.CompanyCode = new SelectList(db.tblCompanies.Where(c => c.isActive)
               .Select(ccc => new { ccc.CompanyCode, ccc.Name }).ToList()
               .Select(cc => new { cc.CompanyCode, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
               "CompanyCode", "Name");
            
            if (companyCode == null)
            {
                ViewBag.RepId = Enumerable.Empty<SelectListItem>();
            }
            else
            {
                var ccode = companyCode.ToString();
                var cid = db.tblCompanies.Single(c => c.CompanyCode == ccode).CompanyId;

                ViewBag.RepId = new SelectList(
                    db.TblUsers.Where(u => u.CompanyId == cid && u.IsActive && u.IsDeleted != true)
                    .Select(r => new { r.Rep_Id, r.UserName }).ToList()
                    .Select(s => new { RepId = int.Parse(s.Rep_Id), UserName = $"{s.UserName} ({s.Rep_Id})" }),
                    "RepId", "UserName", repId
                    );
            }
            
            var items = new List<SelectListItem>()
            {
                new SelectListItem(){Value= "active", Text="Active"},
                new SelectListItem(){Value= "inactive", Text= "Deleted/Held"},
            };
            ViewBag.Filter = new SelectList(items, "Value", "Text");

            var d = bm.GetBatchOrders();

            toDate = toDate == null ? null : (DateTime?)toDate.Value.AddDays(1);
            fromDate = fromDate == null ? DateTime.Now.Date : fromDate.Value;
            if (search == null)
            {
                d = d.Where(f =>
                   (f.IsActive == isActive || isActive == null)
                   && (search == null || f.OrderNo == search || f.BatchNo == search)
                   && (f.CompanyCode == companyCode || companyCode == null)
                   && (f.RepId == repId || repId == null)
                   && (fromDate == null || f.OrderCreatedOn > fromDate)
                   && (toDate == null || f.OrderCreatedOn < toDate));
            }
            else
            {
                d = d.Where(f => f.OrderNo == search || f.BatchNo == search);
            }

            return View(d.ToList());
        }

        public ActionResult OrderDetails(int Id, string filter)
        {
            bool? isActive = null;
            if (filter == "active")
                isActive = true;
            else if (filter == "inactive")
                isActive = false;
            var d = bm.GetBatchOrderDetails(Id);

            ViewBag.Total = d.Count();
            ViewBag.Deleted = d.Count(f => f.IsActive == false);

            d = d.Where(f => f.IsActive == isActive || isActive == null);

            var s = new List<SelectListItem>()
            {
                new SelectListItem(){Value= "active", Text="Active"},
                new SelectListItem(){Value= "inactive", Text= "Deleted"},
            };
            ViewBag.Filter = new SelectList(s, "Value", "Text");

            return View(d);
        }

        [AllowAnonymous]
        public ActionResult DownloadBatch(int Id)
        {
            var content = bm.GetBatchBase64(Id);
            if (content != null)
            {
                var bytes = Convert.FromBase64String(content[1]);
                return File(bytes, "text/plain", content[0]);
            }
            return Json("Batch not found", JsonRequestBehavior.AllowGet);
        }
    }
}
