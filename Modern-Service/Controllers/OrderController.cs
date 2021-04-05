using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Modern_Service.Controllers
{
    [Authorize]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private static readonly HttpClient Client = new()
        {
            BaseAddress = new Uri(
                Environment.GetEnvironmentVariable("LEGACY_API_URL") ??
                throw new ApplicationException("LEGACY_API_URL not set!")),
        };

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<object> GetOrders()
        {
            _logger.LogInformation("Received call to GetOrders() [GET /orders]");

            if (Environment.GetEnvironmentVariable("USE_WIREPACT") == "false")
            {
                // I must translate the credentials myself.
                // Use some magic to fetch the credentials.
                const string user = "Admin";
                const string pass = "AdminPass";

                var basicAuth = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}")));

                _logger.LogInformation(
                    @"USE_WIREPACT false, sending call to legacy with Basic Auth ""{basicAuth}"".",
                    basicAuth.ToString());

                return await (await Client.SendAsync(
                        new HttpRequestMessage(HttpMethod.Get, "orders")
                        {
                            Headers =
                            {
                                Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") },
                                Authorization = basicAuth,
                            },
                        })).EnsureSuccessStatusCode()
                    .Content.ReadAsStringAsync();
            }

            _logger.LogInformation(
                @"USE_WIREPACT true, sending call to legacy with Auth ""{basicAuth}"".",
                HttpContext.Request.Headers["Authorization"].ToString());

            return await (await Client.SendAsync(
                    new HttpRequestMessage(HttpMethod.Get, "orders")
                    {
                        Headers =
                        {
                            Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") },
                            // "Use Auth Mesh": so just send the same information that I got.
                            Authorization =
                                AuthenticationHeaderValue.Parse(HttpContext.Request.Headers["Authorization"]),
                        },
                    })).EnsureSuccessStatusCode()
                .Content.ReadAsStringAsync();
        }
    }
}
