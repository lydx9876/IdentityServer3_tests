using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Validation;

namespace IdSrv
{
    public class TestClaimProvider : DefaultClaimsProvider
    {
        public TestClaimProvider(IUserService users) :
            base(users)
        {
            
        }

        public override async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ClaimsPrincipal subject, Client client,
            IEnumerable<Scope> scopes, bool includeAllIdentityClaims, ValidatedRequest request)
        {
            IEnumerable<Claim> result =
                await base.GetIdentityTokenClaimsAsync(subject, client, scopes, includeAllIdentityClaims, request);
            return result;
        }

        public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, Client client,
            IEnumerable<Scope> scopes, ValidatedRequest request)
        {
            //IEnumerable<Scope> someScopes = scopes;
            IEnumerable<Scope> someScopes = new Scope[] {};

            IEnumerable <Claim> result = await base.GetAccessTokenClaimsAsync(subject, client, someScopes, request);
            return result;
        }
    }
}
