using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class SessionController : MultiTenantMvcController
    {

        [MultiTenantControllerAllow("svcc")]
        public async Task<ActionResult> Index()
        {
            using (var context = new MultiTenantContext())
            {
                var sessions = await context.Sessions.Where(a => a.Tenant.Name == Tenant.Name).
                    OrderBy(a => a.Title.ToLower()).ToListAsync();

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

        [MultiTenantControllerAllow("svcc,cssc")]
        public ActionResult Detail(string id = null)
        {
            using (var context = new MultiTenantContext())
            {
                // add cache here
                var sessions = Utils.FilterSessionsByTenant(context.Sessions.ToList(), Tenant.Name);

                var sessionUrlDictionary = sessions.ToDictionary(k => k.SessionUrl);
                var session = new Session();
                if (sessionUrlDictionary.ContainsKey(id))
                {
                    session = sessionUrlDictionary[id];
                    foreach (Speaker speaker in session.Speakers)
                    {
                        speaker.ImageUrl = $"/Content/Images/Speakers/Speaker-{speaker.PictureId}-75.jpg";
                    }
                }

                return View("Detail", "_Layout", session);
            }
        }

    }
}