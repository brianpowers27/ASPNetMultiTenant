using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using WebApp.Models;

[assembly: OwinStartup(typeof(WebApp.Startup))]

namespace WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                Tenant tenant = GetTenantBasedOnUrl(ctx.Request.Uri.Host);
                if (tenant == null)
                {
                    throw new ApplicationException("tenant not found");
                }

                ctx.Environment.Add("MultiTenant", tenant);
                await next();

            });
        }

        internal static readonly object Locker = new object();

        private Tenant GetTenantBasedOnUrl(string urlHost)
        {
            if (String.IsNullOrEmpty(urlHost))
            {
                throw new ApplicationException(
                    "urlHost must be specified");
            }

            Tenant tenant;

            string cacheName = "all-tenants-cache-name";
            int cacheTimeOutSeconds = 30;

            List<Tenant> tenants =
                new TCache<List<Tenant>>().Get(
                    cacheName, cacheTimeOutSeconds,
                    () =>
                    {
                        List<Tenant> tenants1;
                        using (var context = new MultiTenantContext())
                        {
                            tenants1 = context.Tenants.ToList();
                        }
                        return tenants1;
                    });


                //List<Tenant> tenants =
            //    (List<Tenant>) HttpContext.Current.Cache.Get(cacheName);

            //if (tenants == null)
            //{

            //    lock (Locker)
            //    {
            //        if (tenants == null)
            //        {
            //            using (var context = new MultiTenantContext())
            //            {
            //                tenants = context.Tenants.ToList();
            //                HttpContext.Current.Cache.Insert(cacheName, tenants, null,
            //                    DateTime.Now.Add(new TimeSpan(0, 0, cacheTimeOutSeconds)),
            //                    TimeSpan.Zero);
            //            }
            //        }
            //    }
            //}

            tenant = tenants.
                FirstOrDefault(a => a.DomainName.ToLower().Equals(urlHost)) ??
                     tenants.FirstOrDefault(a => a.Default);
            if (tenant == null)
            {
                throw new ApplicationException
                    ("tenant not found based on URL, no default found");
            }


            return tenant;
        }
    }
}