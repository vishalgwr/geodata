//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeoDataReporting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GeoDataProcessedCustomer
    {
        public int Id { get; set; }
        public int CompanyCode { get; set; }
        public string Acc_ref { get; set; }
        public string DeliveryAddr { get; set; }
        public string CustName { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Addr3 { get; set; }
        public string Addr4 { get; set; }
        public string Addr5 { get; set; }
        public string PostCode { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> LastProcessed { get; set; }
        public bool Hit { get; set; }
        public bool IsResultFound { get; set; }
        public string LookupAddress { get; set; }
    
        public virtual GeoDataProcessedCompany GeoDataProcessedCompany { get; set; }
    }
}