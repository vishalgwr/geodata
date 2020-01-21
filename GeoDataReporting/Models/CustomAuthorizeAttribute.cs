using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoDataReporting.Models
{
    public class CustomAuthorizeAttribute:AuthorizeAttribute
    {
        mSellerDemoLiveEntities context = new mSellerDemoLiveEntities();
      
        public CustomAuthorizeAttribute(params string[] roles)
        {
            
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
           
                var user = context.tblAdminLogins.Where(x => x.LoginName == GnSession.CustomerName && x.Loginid == GnSession.CustomerId && x.IsActive == true).ToList();
                if (user.Count() > 0)
                {
                    authorize = true; 
                }
            
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }  
    }
}