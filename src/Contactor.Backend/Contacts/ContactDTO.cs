namespace Contactor.Backend.Contacts;

using System.ComponentModel.DataAnnotations;

public class ContactDto
{
    public required int Id { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    public string FullName => $"{LastName}, {FirstName}";

    [Required]
    public required string Address { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [Phone]
    public required string MobilePhone { get; set; }
}
