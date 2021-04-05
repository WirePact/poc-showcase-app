using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zitadel.Authentication;
using Zitadel.Authentication.Credentials;

namespace Modern_Service
{
    public class Startup
    {
        private const string JwtKey = @"{
  ""type"": ""application"",
  ""keyId"": ""102538462581809594"",
  ""key"": ""-----BEGIN RSA PRIVATE KEY-----\nMIIEowIBAAKCAQEA5lmQy3oJg3AYGLK/H359vTFWDUdkPQrsgurD4yktnJoldQGc\n8cyw4QUSKAT8Um04A5bNHjwgM1s502IrqIjNszD1+DB+72/N4kFs/JJcbapsXhQN\nEowM9NqXVcO9m3VcpMcif2XXsKw8a6MemgRqcj0Yt9M4JeFtTpZ0ZL+JGubRrBJy\n6NnWTRwdPR5Ub30EfQ2Lnbm+sbDXkFDRrPPYF4IHi6nosOdkg07SklzpW8htSewn\nvXglNEdVaOiApeuS4V/wwJMywlSqgU5gfCN8SD/RhZVZhfM4TDu7jEdoRSZN6r7x\nZFVt5KJm3gHDGc0vbD1DVn2VAKqazRdlD08r6QIDAQABAoIBAC8j/UHnA7NmaUgE\nrKBhXpItWpL1HUpwxd409Umzz6XQ1gGQBvJlFX23b/PIsWqc922kmu2pGF3qOXdN\nC+o9tPsK9guIwTF3DAdVpFw8B7ZZvjuylspI5w3k/juLB26dIgyGxESWLwH0/Zh5\nlXWnmbEvV7OnAkKeU6RVnhRQLlrWpoL0ZXOTLvpyaaCqWln9mbJnpLvk4aHo5/In\nIR2vYYwAuCumaADCA1KzsvPvLc+SgnTMDf867mx+/zpmSZTyW4Bdljt8W9ZV9wS0\nSDMspvPMDGr9yxrBenf805oRlxf0kp1SMeJBgGpfJLOgvAPmhl+aROFWfCaP4/KU\naBLT9EECgYEA7249mzgmEIehdUBuAYTZuAC4tJgWOB2zpAwKwwwTNwK0WdxirilI\nPLYHsL207TL3/QtQ3dZXSn997JxL3v60nUvswH8YmTWPwOC4FP0ROu3Sv6LO00lB\ngDjHC7gjK4DSi6jm2d8OZEFYIbDGnyk5xM9Q9UC6d5d3ZFCvMtoKoK0CgYEA9kpz\nLwjfs34kYTqIQleW76UACDMwSPyxMrHfOnABF6nyecKLpPobhv8ABOa12OjnTfoS\nuO3Axmh8h9jMgMLauHPCUDnLY/o4U8dPRcfQ75MFJ29K82gMtE6QVUyZLsPzqXva\ns/PEWkPwHn2tD4rUbECHIfZlBhq2QdCdIc66060CgYBbrIUQGnaQm63ZXBsCn+BQ\n0I3oL9dKEy1GIYo5VjVBOdreEUEDWDEddcEKDgjpTTugeqWy2q+/iYMohkuSjUmQ\ndJKovcEoYFazTheNibwAKTEpSOgSBBl9Q8AKn61vqbpz2O7S+tHi1xYsiCf0pu9E\niPCBhxAeXDNNiIscWFn7XQKBgQCL8SWdlhc1r/kP3ehKeeZjaIeqIRvQfPRab8L9\nO2MDhScnlCkwpoQ5om7qWgT9qOi03+D/fuhIVFpd/gvFJfKNWAkS+KPevPCAegFL\nDwxi9FC7ZXta7sY5NWLBdPKdJe4vYRaVpW7uMygeyx4odKPalpex7oTcgRUKNHT0\nHrj9SQKBgGCgcRlzNErdAMRMVmVVlCCuG3PT1VPdUk0TXRAhHwi6YayAugOFBwq5\nHCBr77zgf1lNSCal3+KAsd8ju5FYUOm8AXEmOr3rN63RuSbGYQWU7kAi8q2nNWrz\n2uQhc7z+maWYfSNoM0EaKbsjLhF5KFdZwmZ0esaoKwm0ShbPaCWx\n-----END RSA PRIVATE KEY-----\n"",
  ""appId"": ""102538455518604021"",
  ""clientId"": ""102538455518669557@poc_showcase""
}";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services
                .AddAuthorization()
                .AddAuthentication(ZitadelDefaults.ApiAuthenticationScheme)
                .AddZitadelApi(
                    o =>
                    {
                        o.ValidAudiences = new[] { "102537783607878074" };
                        o.JwtProfileKey = new JwtPrivateKeyContent(JwtKey);
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
