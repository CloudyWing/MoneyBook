using System;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace MoneyBook.Web.Controllers {
    public abstract class BasicController : Controller {
        protected internal virtual HttpStatusCodeResult HttpForbidden(string statusDescription = null) {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden, statusDescription);
        }

        public int PageSize => 20;

        /// <summary>
        /// ref:http://blog.darkthread.net/post-2012-08-30-asp-net-mvc-and-json-net.aspx
        /// </returns>
        protected override JsonResult Json(
            object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior
        ) {
            if (behavior == JsonRequestBehavior.DenyGet &&
                string.Equals(Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)
            ) {
                // Call JsonResult to throw the same exception as JsonResult
                return new JsonResult();
            }
            return new JsonNetResult() {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
    }
}