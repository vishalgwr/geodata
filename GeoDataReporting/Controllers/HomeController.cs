using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GeoDataReporting.Models;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;

namespace GeoDataReporting.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();
        public ActionResult OrderLog(string BatchNo = "", string OrderId = "")
        {
            mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();
            List<OrderLog> OrderLogList = new List<OrderLog>();
            if (string.IsNullOrEmpty(BatchNo))
            {
                OrderLogList = (from m in db.OrderLogs select m).ToList();
            }
            else
            {
                OrderLogList = (from m in db.OrderLogs where m.BatchNo == BatchNo && m.OrderID == OrderId select m).ToList();
            }
            // by vishal on 13july 18
            OrderLogList = (from m in db.OrderLogs where (m.BatchNo == BatchNo) && m.OrderID == OrderId select m).ToList();

            ViewBag.BatchNo = BatchNo;
            ViewBag.Companey = "";
            ViewBag.Repid = "";
            if (OrderLogList.Count > 0)
            {
                ViewBag.Companey = OrderLogList[0].Company;
                ViewBag.Repid = OrderLogList[0].EmployeeID;
            }
            return View(OrderLogList);
        }
        private SqlConnection con;

        public ActionResult OrderIDList()
        {
            List<OrderList> lstOrder = new List<OrderList>();
            mSellerDemoLiveEntities context = new mSellerDemoLiveEntities();
            con = new SqlConnection(context.Database.Connection.ConnectionString);
            string query = "select * from(SELECT ISNULL(OrderLog.Company, tblSaveOrder.Company)as Company,ISNULL(OrderLog.EmployeeID, tblSaveOrder.EmployeeID) as EmployeeID, ISNULL(tblsaveorder.orderid, OrderLog.OrderID)as orderid,ISNULL(OrderLog.OrderDate,tblsaveorder.Order_Date) as Order_Date,OrderLog.BatchNo,tblsaveorder.CreatedDate as SaveTransaction, OrderLog.CreatedDate as TransactionSend,ISNULL(OrderLog.CreatedDate,tblsaveorder.CreatedDate) as createdate,ISNULL(OrderLog.customerid,tblsaveorder.customer_Code) as custcode FROM tblsaveorder FULL OUTER JOIN OrderLog ON tblsaveorder.orderid = OrderLog.OrderID) as p where p.createdate>='" + DateTime.Now.ToString("MM/dd/yyyy") + "' ";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            foreach (DataRow item in dt.Rows)
            {
                OrderList temp = new OrderList();
                temp.BatchNo = item[4].ToString();
                temp.Company = item[0].ToString();
                temp.EmployeeID = item[1].ToString() + "- (" + item[8].ToString() + ")";
                temp.OrderDate = item[3].ToString();
                temp.OrderID = item[2].ToString();
                temp.SaveTransaction = item[5].ToString();
                temp.SendTransaction = item[6].ToString();
                lstOrder.Add(temp);
            }
            int no_of_rows = 0;
            if (dt.Rows != null)
            {
                no_of_rows = dt.Rows.Count;
            }
            ViewBag.noOfOrder = no_of_rows;


            ViewBag.company = new SelectList(db.tblCompanies.Where(c => c.isActive)
                .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
                .Select(cc => new { cc.CompanyCode, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
                "CompanyCode", "Name");

            return View(lstOrder);
        }

        public ActionResult GetOrderList2(string Batchno = "")
        {
            mSellerDemoLiveEntities context = new mSellerDemoLiveEntities();
            con = new SqlConnection(context.Database.Connection.ConnectionString);
            string query = "select * from(SELECT ISNULL(OrderLog.Company, tblSaveOrder.Company)as Company,ISNULL(OrderLog.EmployeeID, tblSaveOrder.EmployeeID) as EmployeeID, ISNULL(tblsaveorder.orderid, OrderLog.OrderID)as orderid,ISNULL(OrderLog.OrderDate,tblsaveorder.Order_Date) as Order_Date,OrderLog.BatchNo,tblsaveorder.CreatedDate as SaveTransaction, OrderLog.CreatedDate as TransactionSend,ISNULL(tblsaveorder.CreatedDate,OrderLog.CreatedDate) as createdate,ISNULL(tblsaveorder.customer_Code,OrderLog.customerid) as custcode FROM tblsaveorder FULL OUTER JOIN OrderLog ON tblsaveorder.orderid = OrderLog.OrderID) as p  where p.Batchno='" + Batchno + "' or p.orderid='" + Batchno + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            da.Dispose();
            List<OrderList> lstOrder = new List<OrderList>();
            foreach (DataRow item in dt.Rows)
            {
                OrderList temp = new OrderList();
                temp.BatchNo = item[4].ToString();
                temp.Company = item[0].ToString();
                temp.EmployeeID = item[1].ToString() + "- (" + item[8].ToString() + ")";
                temp.OrderDate = item[3].ToString();
                temp.OrderID = item[2].ToString();
                temp.SaveTransaction = item[5].ToString();
                temp.SendTransaction = item[6].ToString();
                lstOrder.Add(temp);
            }
            int no_of_rows = 0;
            if (dt.Rows != null)
            {
                no_of_rows = dt.Rows.Count;
            }
            ViewBag.noOfOrder = no_of_rows;
            mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();
            ViewBag.company = new SelectList(db.tblCompanies.Where(c => c.isActive)
               .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
               .Select(cc => new { cc.CompanyCode, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
               "CompanyCode", "Name");
            return View("OrderIDList", lstOrder);

        }

        public ActionResult GetOrderList(DateTime? fromdate, DateTime? Todate, string Type = "", int option = 0, string company = "", string repid = "")
        {

            mSellerDemoLiveEntities context = new mSellerDemoLiveEntities();
            con = new SqlConnection(context.Database.Connection.ConnectionString);
            string query = " select * from( SELECT ISNULL(OrderLog.Company, tblSaveOrder.Company)as Company,ISNULL(OrderLog.EmployeeID, tblSaveOrder.EmployeeID) as EmployeeID , ISNULL(tblsaveorder.orderid, OrderLog.OrderID)as orderid,ISNULL(OrderLog.OrderDate,tblsaveorder.Order_Date) as Order_Date, OrderLog.BatchNo,tblsaveorder.CreatedDate as SaveTransaction, OrderLog.CreatedDate as TransactionSend,ISNULL(OrderLog.CreatedDate,tblsaveorder.CreatedDate) as createdate,ISNULL(OrderLog.customerid,tblsaveorder.customer_Code) as custcode FROM tblsaveorder FULL OUTER JOIN OrderLog ON tblsaveorder.orderid = OrderLog.OrderID) as p  ";
            string filter = "";
            if (!string.IsNullOrEmpty(Type))
            {
                if (Type == "SelectedDate")
                {
                    if (fromdate != null)
                    {
                        string fromdate_ = fromdate.Value.ToString("MM/dd/yyyy");
                        filter = "p.createdate>='" + fromdate_ + "' ";
                    }
                    if (Todate != null)
                    {
                        string Todate_ = Todate.Value.ToString("MM/dd/yyyy hh:mm:ss");
                        filter = string.IsNullOrWhiteSpace(filter) ? " p.createdate<='" + Todate_ + "' " : filter + " and p.createdate<='" + Todate_ + "' ";
                    }
                }
                if (Type == "Defineddate")
                {
                    if (option == 1)
                    {
                        filter = " p.createdate>='" + DateTime.Now.ToString("MM/dd/yyyy") + "' ";
                    }
                    else if (option == 2)
                    {
                        filter = " p.createdate>='" + DateTime.Now.AddDays(-3).ToString("MM/dd/yyyy") + "' ";
                    }
                    else if (option == 3)
                    {
                        filter = " p.createdate>='" + DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy") + "' ";
                    }
                }
            }
            else
            {
                return RedirectToAction("OrderIDList");
            }
            string filter_ = "";

            if (!string.IsNullOrWhiteSpace(filter))
            {
                if (company != "")
                {
                    filter_ = " and p.Company=" + company;
                    if (repid != "" && repid != "0")
                    {
                        filter_ = filter_ + " and p.EmployeeID=" + repid;
                    }
                }

                filter = " where " + filter + filter_ + " order by p.TransactionSend desc";
                query = query + filter;
            }
            else
            {

                if (company != "")
                {
                    filter_ = " where p.Company=" + company;
                    if (repid != "" && repid != "0")
                    {
                        filter_ = filter_ + " and p.EmployeeID=" + repid;
                    }
                }

                filter = filter_ + " order by p.TransactionSend desc";
                query = query + filter;
            }
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            DataTable dt = new DataTable();
            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dt);
            con.Close();
            da.Dispose();
            List<OrderList> lstOrder = new List<OrderList>();
            foreach (DataRow item in dt.Rows)
            {
                OrderList temp = new OrderList();
                temp.BatchNo = item[4].ToString();
                temp.Company = item[0].ToString();
                temp.EmployeeID = item[1].ToString() + "- (" + item[8].ToString() + ")"; ;
                temp.OrderDate = item[3].ToString();
                temp.OrderID = item[2].ToString();
                temp.SaveTransaction = item[5].ToString();
                temp.SendTransaction = item[6].ToString();
                lstOrder.Add(temp);
            }
            int no_of_rows = 0;
            if (dt.Rows != null)
            {
                no_of_rows = dt.Rows.Count;
            }
            ViewBag.noOfOrder = no_of_rows;
            mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();
            ViewBag.company = new SelectList(db.tblCompanies.Where(c => c.isActive)
                 .Select(ccc => new { ccc.CompanyId, ccc.Name, ccc.CompanyCode }).ToList()
                 .Select(cc => new { cc.CompanyCode, Name = cc.Name + " (" + cc.CompanyCode + ")" }),
                 "CompanyCode", "Name");
            return View("OrderIDList", lstOrder);
        }

        public ActionResult GetOrderDetail(string BatchNo = "", string OrderId = "", string companyname = "", string repid = "")
        {
            mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();
            List<SubOrderLog> OrderLogList = new List<SubOrderLog>();
            if (string.IsNullOrEmpty(BatchNo))
            {
                OrderLogList = (from m in db.SubOrderLogs select m).ToList();
            }
            else
            {
                OrderLogList = (from m in db.SubOrderLogs where m.BatchNo == BatchNo && m.OrderId == OrderId select m).ToList();
            }
            ViewBag.BatchNo = BatchNo;
            ViewBag.Orderid = OrderId;
            ViewBag.companyname = companyname;
            ViewBag.repid = repid;

            //By Vishal
            ViewBag.company = db.tblCompanies.Select(cc => new { cc.CompanyId, cc.Name })
                .ToList()
                .Select(c =>
                new SelectListItem() { Text = c.Name, Value = c.CompanyId.ToString() });
            return View(OrderLogList);
        }

        public JsonResult GetRepid(string repid = "")
        {
            mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();

            var conpmayId = db.tblCompanies.Single(c => c.CompanyCode == repid).CompanyId;
            var result = (from m in db.TblUsers where m.CompanyId == conpmayId && m.IsActive == true && m.IsDeleted == false select new SelectListItem() { Text = m.FirstName, Value = m.Rep_Id }).ToList();//   new SelectList(db.TblUsers, "Account", "Account");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDeviceLog(string CompanyCode = "")
        {
            mSellerDemoLiveEntities context = new mSellerDemoLiveEntities();
            con = new SqlConnection(context.Database.Connection.ConnectionString);
            string query = "select * from FullUPdateLog where  DATEPART(month,LogDate)=" + DateTime.Now.Day + " and DATEPART(year,LogDate)=" + DateTime.Now.Year + " and DATEPART(day,LogDate)=" + DateTime.Now.Month;
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            List<FullUPdateLog> obj = new List<FullUPdateLog>();
            foreach (DataRow item in dt.Rows)
            {
                FullUPdateLog newobj = new FullUPdateLog();
                newobj.CompanyName = item[1].ToString();
                newobj.LicenseKey = item[2].ToString();
                newobj.DeviceId = item[3].ToString();
                newobj.LicenseKey = item[4].ToString();
                obj.Add(newobj);
            }
            var count = dt.Rows.Count;
            dt.Rows.Clear();
            DateTime day5 = DateTime.Now.AddDays(-1);
            DateTime day4 = DateTime.Now.AddDays(-2);
            DateTime day3 = DateTime.Now.AddDays(-3);
            DateTime day2 = DateTime.Now.AddDays(-4);
            DateTime day1 = DateTime.Now.AddDays(-5);
            string query5 = "select * from FullUPdateLog where  DATEPART(month,LogDate)=" + day5.Day + " and DATEPART(year,LogDate)=" + day5.Year + " and DATEPART(day,LogDate)=" + day5.Month;
            string query4 = "select * from FullUPdateLog where  DATEPART(month,LogDate)=" + day4.Day + " and DATEPART(year,LogDate)=" + day4.Year + " and DATEPART(day,LogDate)=" + day4.Month;
            string query3 = "select * from FullUPdateLog where  DATEPART(month,LogDate)=" + day3.Day + " and DATEPART(year,LogDate)=" + day3.Year + " and DATEPART(day,LogDate)=" + day3.Month;
            string query2 = "select * from FullUPdateLog where  DATEPART(month,LogDate)=" + day2.Day + " and DATEPART(year,LogDate)=" + day2.Year + " and DATEPART(day,LogDate)=" + day2.Month;
            string query1 = "select * from FullUPdateLog where  DATEPART(month,LogDate)=" + day1.Day + " and DATEPART(year,LogDate)=" + day1.Year + " and DATEPART(day,LogDate)=" + day1.Month;

            SqlCommand cmd5 = new SqlCommand(query5, con);
            con.Open();
            DataTable dt5 = new DataTable();
            SqlDataAdapter da5 = new SqlDataAdapter(cmd5);
            da5.Fill(dt);
            con.Close();
            var count5 = dt.Rows.Count;

            dt.Rows.Clear();
            SqlCommand cmd1 = new SqlCommand(query1, con);
            con.Open();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt);
            con.Close();
            var count1 = dt.Rows.Count;
            dt.Rows.Clear();
            SqlCommand cmd2 = new SqlCommand(query2, con);
            con.Open();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt);
            con.Close();
            var count2 = dt.Rows.Count;
            dt.Rows.Clear();

            SqlCommand cmd3 = new SqlCommand(query3, con);
            con.Open();
            DataTable dt3 = new DataTable();
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            da3.Fill(dt);
            con.Close();
            var count3 = dt.Rows.Count;
            dt.Rows.Clear();

            SqlCommand cmd4 = new SqlCommand(query4, con);
            con.Open();
            DataTable dt4 = new DataTable();
            SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
            da4.Fill(dt);
            con.Close();
            var count4 = dt.Rows.Count;
            dt.Rows.Clear();

            ViewBag.Total = count;
            ViewBag.Total1 = count1;
            ViewBag.Total2 = count2;
            ViewBag.Total3 = count3;
            ViewBag.Total4 = count4;
            ViewBag.Total5 = count5;
            //SqlCommand cmd5 = new SqlCommand(query5, con);
            //con.Open();
            //DataTable dt5 = new DataTable();
            //SqlDataAdapter da5 = new SqlDataAdapter(cmd5);
            //da.Fill(dt);
            //con.Close();
            //var count5 = dt.Rows.Count;


            return View(obj);
            //            string[] formats = { "dd/MM/yyyy" };
            //            mSellerDemoLiveEntities db = new mSellerDemoLiveEntities();
            //            DateTime fromdate_1 = DateTime.Now;
            //            string fromdate_1_ = fromdate_1.ToString("dd/MM/yyyy");
            //            DateTime fromdate1 = DateTime.ParseExact(fromdate_1_, formats, new CultureInfo("en"), DateTimeStyles.no
            //);
            //            DateTime fromdate_2 = DateTime.Now.AddDays(-1);
            //            string fromdate_2_ = fromdate_1.ToShortDateString();
            //            var fromdate2 = DateTime.ParseExact(fromdate_2_, formats, new CultureInfo("en-US"), DateTimeStyles.None);
            //            DateTime fromdate_3 = DateTime.Now.AddDays(-2);
            //            string fromdate_3_ = fromdate_1.ToShortDateString();
            //            var fromdate3 = DateTime.ParseExact(fromdate_3_, formats, new CultureInfo("en-US"), DateTimeStyles.None);
            //            DateTime fromdate_4 = DateTime.Now.AddDays(-3);
            //            string fromdate_4_ = fromdate_1.ToShortDateString();
            //            var fromdate4 = DateTime.ParseExact(fromdate_4_, formats, new CultureInfo("en-US"), DateTimeStyles.None);
            //            DateTime fromdate_5 = DateTime.Now.AddDays(-4);
            //            string fromdate_5_ = fromdate_1.ToShortDateString();
            //            var fromdate5 = DateTime.ParseExact(fromdate_5_, formats, new CultureInfo("en-US"), DateTimeStyles.None);
            //            DateTime fromdate_6 = DateTime.Now.AddDays(-5);
            //            string fromdate_6_ = fromdate_1.ToShortDateString();
            //            var fromdate6 = DateTime.ParseExact(fromdate_6_, formats, new CultureInfo("en-US"), DateTimeStyles.None);
            //            ViewBag.Total=0;
            //            if (string.IsNullOrEmpty(CompanyCode))
            //            {
            //                var Loglist = (from m in db.FullUPdateLogs where m.LogDate >= fromdate1 && m.LicenseKey != "59f96ef7-52ea-4048-bf83-0598f3f7ff6d" orderby m.LogDate select m).ToList();
            //                var Loglist1 = (from m in db.FullUPdateLogs where m.LogDate >= fromdate1 && m.LogDate < fromdate_2 && m.LicenseKey != "59f96ef7-52ea-4048-bf83-0598f3f7ff6d" orderby m.LogDate select m).ToList();
            //                var Loglist2 = (from m in db.FullUPdateLogs where m.LogDate >= fromdate2 && m.LogDate < fromdate_3 && m.LicenseKey != "59f96ef7-52ea-4048-bf83-0598f3f7ff6d" orderby m.LogDate select m).ToList();
            //                var Loglist3 = (from m in db.FullUPdateLogs where m.LogDate >= fromdate3 && m.LogDate < fromdate_4 && m.LicenseKey != "59f96ef7-52ea-4048-bf83-0598f3f7ff6d" orderby m.LogDate select m).ToList();
            //                var Loglist4 = (from m in db.FullUPdateLogs where m.LogDate >= fromdate4 && m.LogDate < fromdate_5 && m.LicenseKey != "59f96ef7-52ea-4048-bf83-0598f3f7ff6d" orderby m.LogDate select m).ToList();
            //                var Loglist5 = (from m in db.FullUPdateLogs where m.LogDate >= fromdate5 && m.LogDate < fromdate_6 && m.LicenseKey != "59f96ef7-52ea-4048-bf83-0598f3f7ff6d" orderby m.LogDate select m).ToList();
            //                ViewBag.Total = Loglist.Count;
            //                ViewBag.Total1 = Loglist1.Count;
            //                ViewBag.Total2 = Loglist2.Count;
            //                ViewBag.Total3 = Loglist3.Count;
            //                ViewBag.Total4= Loglist4.Count;
            //                ViewBag.Total5 = Loglist5.Count;
            //                return View(Loglist);
            //            }
            //            return View();
        }

        public static long ToUnixTimestamp(DateTime? dateValue)
        {
            if (dateValue == null)
                return 0;
            return (long)Math.Truncate((dateValue.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        public JsonResult GetRepIds(string Id)
        {
            var cid = db.tblCompanies.Single(c => c.CompanyCode == Id).CompanyId;
            var reps = db.TblUsers.Where(u => u.CompanyId == cid && u.IsActive && u.IsDeleted != true).Select(r => new { r.Rep_Id, r.UserName }).ToList()
                .Select(s => new { RepId = int.Parse(s.Rep_Id), s.UserName });
            return Json(reps, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserIds(int Id)
        {
            var reps = db.TblUsers.Where(u => u.CompanyId == Id && u.IsActive && u.IsDeleted != true).Select(r => new { r.Rep_Id, r.UserName, r.UserId }).ToList()
                .Select(s => new { RepId = int.Parse(s.Rep_Id), s.UserName, s.UserId });
            return Json(reps, JsonRequestBehavior.AllowGet);
        }
        //[OutputCache(Duration = 600)]
        [AllowAnonymous]
        public FileResult Download(string path)
        {
            var content = System.IO.File.ReadAllBytes(path);
            if (path.Split('.').Last() == "csv")
            {
                return File(content, "text/plain");
            }
            return File(content, "application/json");
        }
    }
}
