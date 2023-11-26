namespace Contactor.Tests.Controllers;

using Contactor.Backend.Models.Dto.Skills;

internal static class SkillsData
{
    private static SkillDtoOut skill1;
    private static SkillDtoOut skill2;
    private static SkillDtoOut[] allSkills;
    private static SkillDtoIn invalidSkill;

    public static SkillDtoOut Skill1 => skill1 ??= new() {
        Id = 1,
        Name = "csharp",
        Level = 5,
        Contacts = [],
    };
    public static SkillDtoOut Skill2 => skill2 ??= new() {
        Id = 2,
        Name = "java",
        Level = 3,
        Contacts = [],
    };

    public static SkillDtoOut[] AllSkills => allSkills ??= [Skill1, Skill2];
    public static SkillDtoIn InvalidSkill => invalidSkill ??= new() {
        Name = string.Empty,
        Level = 6,
    };
}
