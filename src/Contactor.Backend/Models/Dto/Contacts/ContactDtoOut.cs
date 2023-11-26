namespace Contactor.Backend.Models.Dto.Contacts;

using System.Collections.ObjectModel;
using Contactor.Backend.Models.Domain;

/// <summary>
/// Data transfer object for <see cref="Contact"/> to export / provide all the
/// internal information.
/// </summary>
public record ContactDtoOut : ContactDtoIn
{
    public required int Id { get; init; }

    public string FullName => $"{LastName}, {FirstName}";

    public ICollection<SkillDto> Skills { get; init; } = new Collection<SkillDto>();

    public static ContactDtoOut FromModel(Contact model)
    {
        static SkillDto FromModel(Skill skill) =>
            new() { Id = skill.Id, Name = skill.Name, Level = skill.Level };

        return new ContactDtoOut {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            Email = model.Email,
            MobilePhone = model.MobilePhone,
            Skills = model.Skills.Select(FromModel).ToList(),
        };
    }

    public record SkillDto
    {
        public required int Id { get; init; }

        public required string Name { get; set; }

        public required int Level { get; set; }
    }
}
