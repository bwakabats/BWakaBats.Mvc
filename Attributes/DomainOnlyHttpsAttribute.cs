using System;
using System.Web.Mvc;

namespace BWakaBats.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class DomainOnlyHttpsAttribute : RequireHttpsAttribute
    {
        public DomainOnlyHttpsAttribute(string domain)
        {
            Domain = domain;
        }

        public string Domain { get; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string hostname = filterContext.HttpContext.Request.Url.Host;
            if (hostname.IndexOf("www." + Domain, StringComparison.OrdinalIgnoreCase) == 0)
            {
                base.OnAuthorization(filterContext);
            }
            else if (hostname.IndexOf(Domain, StringComparison.OrdinalIgnoreCase) == 0)
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}
