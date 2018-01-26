using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApp.Models
{
    public class Utils
    {
        internal static int SlugLength = 75;

        public string GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= SlugLength ? str.Length : SlugLength).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string txt)
        {
            if (String.IsNullOrEmpty(txt))
            {
                return "";
            }
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        public static List<Session> FilterSessionsByTenant(List<Session> inSessions, string tenantName)
        {
            return inSessions.Where(a => a.Tenant.Name == tenantName).ToList();
        }

        public static List<Speaker> FilterSpeakersByTenant(List<Speaker> inSpeakers, string tenantName)
        {
            return
                inSpeakers.
                    Where(speaker => speaker.Sessions.
                        Any(a => a.Tenant.Name == tenantName)).
                    ToList();
        }
    }
}