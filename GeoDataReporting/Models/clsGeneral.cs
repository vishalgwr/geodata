using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoDataReporting.Models
{
    public class clsGeneral
    {

    }

    public class FileProcessedDetails
    {
        public int? CompanyCode { get; set; }
        public string TradingName { get; set; }
        public DateTime? FileModifiedOn { get; set; }
        public DateTime? FileProcessedOn { get; set; }
        public bool? IsFileProcessedSuccessfully { get; set; }
        public int NewProcessedRecords { get; set; }
        public int FailedRecords { get; set; }
    }
    public class OrderList
    {
        public string Company { get; set; }
        public string EmployeeID { get; set; }
        public string OrderID { get; set; }
        public string OrderDate { get; set; }
        public string BatchNo { get; set; }
        public string SaveTransaction { get; set; }
        public string SendTransaction { get; set; }
    }

}