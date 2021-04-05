using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zitadel.Authentication;

namespace Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services
                .AddAuthorization()
                .AddAuthentication(ZitadelDefaults.AuthenticationScheme)
                .AddZitadelWithSession(
                    o =>
                    {
                        o.ClientId = "102538020334461370@poc_showcase";
                        o.GetClaimsFromUserInfoEndpoint = true;
                        o.SaveTokens = true;
                    });

            services.AddLogging(b => b.AddSystemdConsole());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }

            app.UseForwardedHeaders(
                new()
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedProto,
                    KnownNetworks = { new IPNetwork(IPAddress.Parse("0.0.0.0"), 0) },
                });

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}
