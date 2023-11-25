namespace Contactor.Backend.Models;

using System.ComponentModel.DataAnnotations;

public record ContactDto
{
    public required int Id { get; init; }

    [Required]
    public required string FirstName { get; init; }

    [Required]
    public required string LastName { get; init; }

    public string FullName => $"{LastName}, {FirstName}";

    [Required]
    public required string Address { get; init; }

    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    [Phone]
    public required string MobilePhone { get; init; }
}
