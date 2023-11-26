namespace Contactor.Models.Business.Skills;

using Contactor.Models.Domain;

public record SkillDtoOut : SkillDtoIn
{
    public required int Id { get; init; }

    public required ICollection<ContactDto> Contacts { get; init; }

    public static SkillDtoOut FromModel(Skill model)
    {
        static ContactDto FromModel(Contact model) =>
            new(model.FirstName, model.LastName, model.Address, model.Email, model.MobilePhone);

        return new SkillDtoOut {
            Id = model.Id,
            Name = model.Name,
            Level = model.Level,
            Contacts = model.Contacts.Select(FromModel).ToList(),
        };
    }

    public record ContactDto(
        string FirstName,
        string LastName,
        string Address,
        string Email,
        string MobilePhone);
}
