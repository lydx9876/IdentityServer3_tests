//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.Controllers;
//using Thinktecture.IdentityModel.WebApi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Apis
{
    public class AdvancedAuthorizeAttribute : AuthorizeAttribute {
        private const string SCOPE = "scope";
        private const string ROLE = "role";

        public string[] ScopesAll { get; set; }

        public string[] ScopesAnyOf { get; set; }

        public string[] RolesAll { get; set; }

        public string[] RolesAnyOf { get; set; }

        public AdvancedAuthorizeAttribute() { }

        private bool CheckScopes(ClaimsPrincipal claimsPrincipal) {
            var scopes = claimsPrincipal.FindAll(SCOPE).Select(claim => claim.Value).ToList();
            if (ScopesAll != null && ScopesAll.Length != 0) {
                foreach (var scope in ScopesAll) {
                    if (!scopes.Contains(scope, StringComparer.OrdinalIgnoreCase)) {
                        return false;
                    }
                }
            }
            if (ScopesAnyOf != null && ScopesAnyOf.Length != 0) {
                bool anyFound = false;
                foreach (var scope in ScopesAnyOf) {
                    if (scopes.Contains(scope, StringComparer.OrdinalIgnoreCase)) {
                        anyFound = true;
                        break;
                    }
                }
                if (!anyFound) {
                    return false;
                }
            }
            return true;
        }

        private bool CheckRoles(ClaimsPrincipal claimsPrincipal) {
            var roles = claimsPrincipal.FindAll(ROLE).Select(claim => claim.Value).ToList();
            if (RolesAll != null && RolesAll.Length != 0) {
                foreach (var role in RolesAll) {
                    if (!roles.Contains(role, StringComparer.OrdinalIgnoreCase)) {
                        return false;
                    }
                }
            }
            if (RolesAnyOf != null && RolesAnyOf.Length != 0) {
                bool anyFound = false;
                foreach (var role in RolesAnyOf) {
                    if (roles.Contains(role, StringComparer.OrdinalIgnoreCase)) {
                        anyFound = true;
                        break;
                    }
                }
                if (!anyFound) {
                    return false;
                }
            }
            return true;
        }

        private bool CheckUser(HttpActionContext actionContext) {
            bool result = base.IsAuthorized(actionContext);
            return result;
        }

        private static bool IsContextAuthorized(HttpActionContext actionContext) {
            IPrincipal principal = actionContext.ControllerContext.RequestContext.Principal;
            bool result = principal != null && principal.Identity != null && principal.Identity.IsAuthenticated;
            return result;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext) {
            ClaimsPrincipal claimsPrincipal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;
            if (claimsPrincipal == null) {
                return false;
            }
            if (!IsContextAuthorized(actionContext)) {
                return false;
            }
            if (!CheckScopes(claimsPrincipal)) {
                return false;
            }
            if (!CheckRoles(claimsPrincipal)) {
                return false;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext) {
            HttpResponseMessage response;
            if (actionContext.RequestContext.Principal != null && actionContext.RequestContext.Principal.Identity.IsAuthenticated) {
                response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "insufficient_scope");
                response.Headers.Add("WWW-Authenticate", "Bearer error=\"insufficient_scope\"");
            }
            else {
                response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            actionContext.Response = response;
        }
    }
}