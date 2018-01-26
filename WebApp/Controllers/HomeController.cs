using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : MultiTenantMvcController
    {
        public ActionResult Index()
        {
            if (Tenant.Name == "CSSC")
            {
                using (var context = new MultiTenantContext())
                {
                    var sessions = context.Sessions.Where(a => a.Tenant.Name == Tenant.Name).
                        OrderBy(a => a.Title.ToLower()).ToList();

                    foreach (var session in sessions)
                    {
                        foreach (var speaker in session.Speakers)
                        {
                            speaker.ImageUrl = $"/Content/Images/Speakers/Speaker-{speaker.PictureId}-75.jpg";
                        }
                    }
                    return View("Index", "_Layout", sessions);
                }
            }

            return View();
        }

        [MultiTenantControllerAllow("cssc")]
        public ActionResult CodeOfConduct()
        {
            return View();
        }

    }
}