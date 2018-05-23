using GFAPP.Framework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFAPP.Web
{
    public class CookiesSession : IAppSession
    {
        private IHttpContextAccessor httpContextAccessor;

        public CookiesSession(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string Username { get => httpContextAccessor.HttpContext.User.Identity.Name; }
        public string Name { get => throw new NotImplementedException(); }

        public int CompanyId { get {
                var claim = httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == AppClaimTypes.CompanyId);
                if(claim != null)
                {
                    return Convert.ToInt32(claim.Value);
                }

                return -1;
            } }
    }
}
