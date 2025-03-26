using Microsoft.AspNetCore.Mvc;
using CRMSystem.Api.Models;
using CRMSystem.Api.Services;
using CRMSystem.Api.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CRMSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService customerService, ILogger<CustomersController> logger) : ControllerBase {
    private readonly ICustomerService _customerService = customerService;
    private readonly ILogger<CustomersController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers() {
        try {
            var customers = await _customerService.GetListAsync();
            return Ok(customers);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving customers");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id) {
        try {
            var customer = await _customerService.GetAsync(c => c.Id == id);
            if (customer == null) {
                return NotFound();
            }
            return Ok(customer);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving customer with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers([FromQuery] string? query) {
        try {
            if (string.IsNullOrEmpty(query)) {
                return BadRequest("search parameter is required");
            }
            var customers = await _customerService.GetListAsync(c => c.FirstName.Contains(query) || c.LastName.Contains(query) || c.Email.Contains(query) || c.Region.Contains(query));
            return Ok(customers);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error searching customers with search {query}", query);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(CustomerDto customerDto) {
        try {
            var customer = customerDto.ToCustomer();
            var newCustomer = await _customerService.CreateAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, newCustomer);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Customer>> UpdateCustomer(int id, CustomerDto customerDto) {
        try {
            var customer = await _customerService.GetAsync(c => c.Id == id);
            if (customer == null) {
                return NotFound();
            }
            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.Region = customerDto.Region;
            customer.RegistrationDate = customerDto.RegistrationDate;
            var updatedCustomer = await _customerService.UpdateAsync(customer);
            return Ok(updatedCustomer);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error updating customer with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id) {
        try {
            var customer = await _customerService.GetAsync(c => c.Id == id);
            if (customer == null) {
                return NotFound();
            }
            _customerService.Delete(id);
            return NoContent();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error deleting customer with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}