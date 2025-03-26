namespace CRMSystem.Api.Models;

public class Customer {
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Region { get; set; }
    public required DateTime RegistrationDate { get; set; }
}