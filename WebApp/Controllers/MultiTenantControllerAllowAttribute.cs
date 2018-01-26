using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class MultiTenantControllerAllowAttribute : 
        ActionFilterAttribute

    {
        private readonly List<string> _tenantList;

        public MultiTenantControllerAllowAttribute(string confArray)
        {
            _tenantList = confArray.ToLower().Split(',').ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (MultiTenantMvcController)
                filterContext.Controller;

            if (controller.Tenant == null ||
                !_tenantList.Contains(controller.Tenant.Name.ToLower()))
            {
                throw
                    new HttpException(404, "Tenant Not Found");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}