using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models;

namespace Frontend.Pages
{
    [Authorize]
    public class Dashboard : PageModel
    {
        private readonly ILogger<Dashboard> _logger;

        private static readonly HttpClient Client = new()
        {
            BaseAddress = new Uri(
                Environment.GetEnvironmentVariable("API_BASE_URL") ??
                throw new ApplicationException("API_BASE_URL not set!")),
        };

        public Dashboard(ILogger<Dashboard> logger)
        {
            _logger = logger;
        }

        public string ApiUrl { get; } = Client.BaseAddress?.ToString() ?? string.Empty;
        public IList<Customer> Customers { get; set; } = new List<Customer>();
        public IList<Order> Orders { get; set; } = new List<Order>();

        public void OnGet()
        {
            if (TempData.ContainsKey("customers"))
            {
                try
                {
                    Customers = JsonSerializer.Deserialize<List<Customer>>(
                        TempData["customers"] as string ?? string.Empty,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }
                catch
                {
                    TempData.Clear();
                }
            }

            if (TempData.ContainsKey("orders"))
            {
                try
                {
                    Orders = JsonSerializer.Deserialize<List<Order>>(
                        TempData["orders"] as string ?? string.Empty,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }
                catch
                {
                    TempData.Clear();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // The "Frontend" only knows OIDC and calls the Modern Service.
            var token = await HttpContext.GetTokenAsync("access_token");
            if (token == null)
            {
                throw new ApplicationException("No Access Token found");
            }

            _logger.LogInformation("Send GET /customers to legacy api.");

            TempData["customers"] = await (await Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, "customers")
                {
                    Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
                })).Content.ReadAsStringAsync();

            _logger.LogInformation("Send GET /orders to legacy api.");

            TempData["orders"] = await (await Client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, "orders")
                {
                    Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
                })).Content.ReadAsStringAsync();

            return RedirectToPage();
        }
    }
}
