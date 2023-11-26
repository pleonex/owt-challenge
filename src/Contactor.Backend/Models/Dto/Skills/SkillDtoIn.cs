﻿namespace Contactor.Backend.Models.Dto.Skills;

using System.ComponentModel.DataAnnotations;
using Contactor.Backend.Models.Domain;

public record SkillDtoIn
{
    [Required]
    public required string Name { get; set; }

    [Required]
    [Range(0, 5)]
    public required int Level { get; set; }

    public void UpdateModel(Skill model)
    {
        model.Name = Name;
        model.Level = Level;
    }
}
