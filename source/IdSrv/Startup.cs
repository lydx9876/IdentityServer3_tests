using Owin;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;

namespace IdSrv
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                {
                    CustomRequestValidator = new Registration<ICustomRequestValidator>(new TestRequestValidator()),
                    ClaimsProvider = new Registration<IClaimsProvider, TestClaimProvider>(),
                }
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get())
                    .UseInMemoryUsers(Users.Get()),

                RequireSsl = false
            };

            app.UseIdentityServer(options);
        }
    }
}