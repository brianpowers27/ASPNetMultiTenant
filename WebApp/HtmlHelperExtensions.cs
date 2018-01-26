using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp
{
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// Encode html such that if tags (like script tags) are put in then they are encoded for
        /// safe displaying to web pages
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="inString"></param>
        /// <param name="allowHtml">if true, then don't encode the incoming string</param>
        /// <returns></returns>
        public static MvcHtmlString SafeEncode
            (this HtmlHelper helper, string inString,
                bool? allowHtml = false)
        {

            string s =
                allowHtml.HasValue && allowHtml.Value
                    ? inString
                    : HttpUtility.HtmlEncode(inString);
            s = inString;
            var ss = new MvcHtmlString(s);
            return ss;
        }

        public static MvcHtmlString
            ReplaceAllAmpAndSemiWithComma
            (this HtmlHelper helper, string inString)
        {
            return new
                MvcHtmlString
                (inString);
        }

    }
}