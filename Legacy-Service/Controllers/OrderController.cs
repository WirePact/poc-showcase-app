using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace Legacy_Service.Controllers
{
    [Authorize]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private static readonly Dictionary<int, Order> Orders = new()
        {
            { 1, new Order(1, "ZZZ Top Inc.", "Blade Server", 15) },
            { 2, new Order(2, "ZZZ Top Inc.", "Blade Server Gen 2", 35) },
            { 3, new Order(3, "Acme Ltd.", "Mount Screws", 1337) },
        };

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            _logger.LogInformation("Received call to GetOrders() [GET /orders]");
            _logger.LogInformation(
                @"The received Auth is ""{basicAuth}"".",
                HttpContext.Request.Headers["Authorization"].ToString());

            return Ok(Orders.Values);
        }
    }
}
