﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbGeodataEntities : DbContext
    {
        public dbGeodataEntities()
            : base("name=dbGeodataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<GeoDataProcessedCompany> GeoDataProcessedCompanies { get; set; }
        public DbSet<GeoDataProcessedCustomer> GeoDataProcessedCustomers { get; set; }
        public DbSet<UserActivityTracking> UserActivityTrackings { get; set; }
    }
}
