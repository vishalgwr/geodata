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
    
    public partial class TblUser
    {
        public int UserId { get; set; }
        public string Rep_Id { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Password { get; set; }
        public Nullable<bool> Allow_Order_Price_Edit { get; set; }
        public Nullable<bool> Check_Against_Min_Price { get; set; }
        public Nullable<bool> Show_Cost_Margin { get; set; }
        public Nullable<bool> Allow_Order_Price_Edit_with_Warning { get; set; }
        public Nullable<bool> Check_Against_Min_Price_with_Warning { get; set; }
        public Nullable<bool> Use_same_option_to_define_qty_On_PO { get; set; }
        public Nullable<bool> Show_Stock_Figures { get; set; }
        public Nullable<bool> Show_Qty_Figure { get; set; }
        public string Select_Min_Price_List { get; set; }
        public string Next_New_Cust_No { get; set; }
        public string Next_New_Batch_No { get; set; }
        public string Next_New_Order_No { get; set; }
        public string Allow_max_discount { get; set; }
        public string Against_price_list { get; set; }
        public string Email_Confirmation { get; set; }
        public Nullable<System.DateTime> Last_Full_Sync { get; set; }
        public Nullable<System.DateTime> Last_Part_Sync { get; set; }
        public Nullable<System.DateTime> Last_Order_Transmission { get; set; }
        public bool IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<decimal> LoggedUserId { get; set; }
        public Nullable<decimal> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> Show_FOB_cost { get; set; }
        public Nullable<bool> Enable_Price_Tab_Selection { get; set; }
        public Nullable<bool> Show_2nd_product_screen_at_startup { get; set; }
        public Nullable<bool> Hide_Price_Rows { get; set; }
        public Nullable<bool> Search_InCategory { get; set; }
        public Nullable<bool> AgainstCost { get; set; }
        public string Default_catalogue_screen { get; set; }
        public Nullable<bool> DefaultOff { get; set; }
        public Nullable<bool> IsSupervisor { get; set; }
        public Nullable<bool> IsReviewUser { get; set; }
        public string USerAppVersion { get; set; }
        public Nullable<bool> default_ord_price_to_last_price_paid { get; set; }
        public Nullable<bool> isEncryptPrice { get; set; }
        public Nullable<bool> inSufficientStockNotify { get; set; }
        public Nullable<bool> inSufficientStockDisallow { get; set; }
        public string inSufficientStockField { get; set; }
        public Nullable<bool> IsSyncCurrentOrder { get; set; }
        public Nullable<bool> IsForceFullUpdate { get; set; }
        public Nullable<bool> DisAllow_Customer_Price_Edit { get; set; }
        public Nullable<bool> ShowHideLastPricePaid { get; set; }
        public string SuperVisorforReps { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
