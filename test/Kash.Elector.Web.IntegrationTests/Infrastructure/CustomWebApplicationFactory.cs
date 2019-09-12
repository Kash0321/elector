using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Kash.Elector.Web.IntegrationTests.Infrastructure
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        private ITestOutputHelper _output;

        public IServiceCollection Services { get; private set; }

        public CustomWebApplicationFactory()
        {
        }

        public void SetOutput(ITestOutputHelper output)
        {
            _output = output;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            //builder.ConfigureServices(services =>
            //{
            //    services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>>(new PostConfigureOptionsForTesting<JwtBearerOptions>(options =>
            //    {
            //        options.Authority = "";
            //        options.RequireHttpsMetadata = false;
            //        //options.Audience = AuthUtilities.AUDIENCE;
            //        //options.TokenValidationParameters.SignatureValidator = (token, validationParameters) => AuthUtilities.DeserializeToken(token);
            //        //options.TokenValidationParameters.IssuerValidator = (issuer, securityToken, validationParameters) => AuthUtilities.ISSUER;
            //        options.TokenValidationParameters.AudienceValidator = (audiences, securityToken, validationParameters) => true;
            //    }));
            //});
        }
    }
}
