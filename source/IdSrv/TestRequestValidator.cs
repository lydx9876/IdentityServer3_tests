using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Validation;

namespace IdSrv
{
    public class TestRequestValidator : ICustomRequestValidator
    {
        public TestRequestValidator()
        {
            
        }

        #region ICustomRequestValidator

        public Task<AuthorizeRequestValidationResult> ValidateAuthorizeRequestAsync(ValidatedAuthorizeRequest request)
        {
            return Task.FromResult(new AuthorizeRequestValidationResult
            {
                IsError = false
            });
        }

        public Task<TokenRequestValidationResult> ValidateTokenRequestAsync(ValidatedTokenRequest request)
        {
            //request.Scopes = new List<string>();
            string stackTrace = Environment.StackTrace;

            return Task.FromResult(new TokenRequestValidationResult
            {
                IsError = false,
                //IsError = true
            });
        }

        #endregion ICustomRequestValidator
    }
}
