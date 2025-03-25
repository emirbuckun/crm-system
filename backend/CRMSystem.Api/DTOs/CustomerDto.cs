using CRMSystem.Api.Models;

namespace CRMSystem.Api.DTOs;
public class CustomerDto {
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Region { get; set; }
    public DateTime RegistrationDate { get; set; }

    public Customer ToCustomer() {
        return new Customer {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Region = Region,
            RegistrationDate = RegistrationDate
        };
    }
}