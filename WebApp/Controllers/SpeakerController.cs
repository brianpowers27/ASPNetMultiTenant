using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class SpeakerController : MultiTenantMvcController
    {
        private readonly MultiTenantContext _context = new MultiTenantContext();


        [MultiTenantControllerAllow("svcc")]
        public async Task<ActionResult> Detail(string id = null)
        {
            using (var context = new MultiTenantContext())
            {
                // add cache here
                var speakers = await context.Speakers.ToListAsync();

                var speakerUrlDictionary = speakers.ToDictionary(k => k.SpeakerUrl);
                Speaker speaker = new Speaker();
                if (speakerUrlDictionary.ContainsKey(id))
                {
                    speaker = speakerUrlDictionary[id];

                    speaker.ImageUrl = $"/Content/Images/Speakers/Speaker-{speaker.PictureId}-75.jpg";
                    var sessions =
                        speaker.Sessions.
                            Where(a => a.Tenant.Name == Tenant.Name).
                            OrderBy(a => a.Title).ToList();
                    speaker.Sessions = sessions;
                }

                return View("Detail", "_Layout", speaker);
            }
        }



        [MultiTenantControllerAllow("angu,svcc")]
        public async Task<ActionResult> Index()
        {
            var speakersAll =
                new TCache<Task<List<Speaker>>>().
                    Get("s-cache", 20,
                        () =>
                        {
                            var speakersAll1 =
                                _context.Speakers.ToListAsync();
                            return speakersAll1;
                        });


            var speakers = new List<Speaker>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var speaker in await speakersAll)
            {
                var speakerInTenant =
                    speaker.Sessions.
                        Any(a => a.Tenant.Name == Tenant.Name);
                if (speakerInTenant)
                {
                    speakers.Add(new Speaker
                    {
                        FirstName = speaker.FirstName,
                        LastName = speaker.LastName,
                        Id = speaker.Id,
                        PictureId = speaker.PictureId,
                        Bio = speaker.Bio,
                        AllowHtml = speaker.AllowHtml,
                        WebSite = speaker.WebSite,

                        ImageUrl =
                            $"/Content/Images/Speakers/Speaker-{speaker.PictureId}-75.jpg",
                        Sessions =
                            speaker.Sessions.
                                Where(a => a.Tenant.Name == Tenant.Name).
                                OrderBy(a => a.Title).ToList()
                    });
                }
            }
            return View("Index", "_Layout", speakers);
        }
    }
}