using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace Apis
{
    [Route("testsecond")]
    //[Authorize(Roles = "role1")]
    //[ScopeAuthorize(new string[] { "api1", "api2" }, Roles = "role1 role2", Users = "bob")]
    //[ScopeAuthorize(new string[] { "api1", "api2" })]
    //[Authorize(Roles = "role1 role2", Users = "bob")]
    //[Authorize(Roles = "role1, role2")]
    [AdvancedAuthorize(ScopesAll = new string[] {"api1", "api2"},
        RolesAll = new string[] {"role1", "role2"},
        RolesAnyOf = new string[] {"role1", "role2"})]
public class TestSecondController : ApiController
    {
        public IHttpActionResult Get()
        {
            var caller = User as ClaimsPrincipal;

            return Json(new
            {
                message = "Hello",
            });
        }
    }
}