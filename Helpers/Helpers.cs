using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdegaItalia.Helpers
{
    public static class Helpers
    {
        public static HtmlString PartialResult(this HtmlHelper helper, string path)
        {
            var requestContext = helper.ViewContext.RequestContext;
            UrlHelper url = new UrlHelper(requestContext);
            var client = new WebClient();
            var str = client.DownloadString(new Uri(string.Format("http://{0}{1}", requestContext.HttpContext.Request.Url.Host, url.Content(path))));
            return MvcHtmlString.Create(str);
        }




    }
}