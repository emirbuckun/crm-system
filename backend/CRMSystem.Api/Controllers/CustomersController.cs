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
        _logger.LogInformation("Fetching all customers");
        try {
            var customers = await _customerService.GetListAsync();
            _logger.LogInformation("Successfully retrieved {Count} customers", customers.Count);
            return Ok(customers);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving customers");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id) {
        _logger.LogInformation("Fetching customer with id: {Id}", id);
        try {
            var customer = await _customerService.GetAsync(c => c.Id == id);
            if (customer == null) {
                _logger.LogWarning("Customer with id {Id} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Successfully retrieved customer with id: {Id}", id);
            return Ok(customer);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving customer with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers([FromQuery] string? query) {
        _logger.LogInformation("Searching customers with query: {Query}", query);
        try {
            if (string.IsNullOrEmpty(query)) {
                _logger.LogWarning("Search failed - query parameter is required");
                return BadRequest("search parameter is required");
            }
            var customers = await _customerService.GetListAsync(c => c.FirstName.Contains(query) || c.LastName.Contains(query) || c.Email.Contains(query) || c.Region.Contains(query));
            _logger.LogInformation("Successfully found {Count} customers matching query: {Query}", customers.Count(), query);
            return Ok(customers);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error searching customers with query {Query}", query);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(CustomerDto customerDto) {
        _logger.LogInformation("Creating a new customer");
        try {
            var customer = customerDto.ToCustomer();
            var newCustomer = await _customerService.CreateAsync(customer);
            _logger.LogInformation("Successfully created customer with id: {Id}", newCustomer.Id);
            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, newCustomer);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Customer>> UpdateCustomer(int id, CustomerDto customerDto) {
        _logger.LogInformation("Updating customer with id: {Id}", id);
        try {
            var customer = await _customerService.GetAsync(c => c.Id == id);
            if (customer == null) {
                _logger.LogWarning("Update failed - Customer with id {Id} not found", id);
                return NotFound();
            }
            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.Region = customerDto.Region;
            customer.RegistrationDate = customerDto.RegistrationDate;
            var updatedCustomer = await _customerService.UpdateAsync(customer);
            _logger.LogInformation("Successfully updated customer with id: {Id}", id);
            return Ok(updatedCustomer);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error updating customer with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id) {
        _logger.LogInformation("Deleting customer with id: {Id}", id);
        try {
            var customer = await _customerService.GetAsync(c => c.Id == id);
            if (customer == null) {
                _logger.LogWarning("Delete failed - Customer with id {Id} not found", id);
                return NotFound();
            }
            var deleted = await _customerService.Delete(id);
            if (!deleted) {
                _logger.LogWarning("Delete failed - Customer with id {Id} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Successfully deleted customer with id: {Id}", id);
            return NoContent();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error deleting customer with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}