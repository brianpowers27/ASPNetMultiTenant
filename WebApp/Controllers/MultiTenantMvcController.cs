using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class MultiTenantMvcController : Controller
    {
        public Tenant Tenant
        {
            get
            {
                object multiTenant;
                if (!Request.GetOwinContext().Environment.TryGetValue("MultiTenant",
                    out multiTenant))
                {
                    throw new ApplicationException("Could not find Tenant");
                }

                return (Tenant)multiTenant;
            }
        }
    }
}