using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace Modern_Service.Controllers
{
    [Authorize]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        private static readonly Dictionary<int, Customer> Customers = new()
        {
            { 1, new Customer(1, "ZZZ Top Inc.") },
            { 2, new Customer(2, "Acme Ltd.") },
        };

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            _logger.LogInformation("Received call to GetCustomers() [GET /customers]");
            return Ok(Customers.Values);
        }
    }
}
