namespace Contactor.Models.Business.Contacts;

using System.ComponentModel.DataAnnotations;
using Contactor.Models.Domain;

/// <summary>
/// Data transfer object to receive <see cref="Contact"/> info from APIs with
/// data validation..
/// </summary>
/// <remarks>
/// The _in_ model does not contain the ID as it's set by the DB or
/// route parameter and the skills as they are set by different endpoint.
/// </remarks>
public record ContactDtoIn
{
    [Required]
    public required string FirstName { get; init; }

    [Required]
    public required string LastName { get; init; }

    [Required]
    public required string Address { get; init; }

    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    [Phone]
    public required string MobilePhone { get; init; }

    public void UpdateModel(Contact model)
    {
        model.FirstName = FirstName;
        model.LastName = LastName;
        model.Address = Address;
        model.Email = Email;
        model.MobilePhone = MobilePhone;
    }
}
