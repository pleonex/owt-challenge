﻿namespace Contactor.Backend.Models.Dto.Skills;
public interface ISkillRepository
{
    Task<IEnumerable<SkillDtoOut>> GetAll();

    Task<SkillDtoOut?> GetById(int id);

    Task<bool> UpdateById(int id, SkillDtoIn dto);

    Task<bool> RemoveById(int id);
}