namespace Contactor.Backend.Controllers;

using Contactor.Backend.Models.Dto.Skills;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SkillsController(ISkillRepository repository) : ControllerBase
{
    // GET: api/<SkillsController>
    [HttpGet]
    public async Task<IEnumerable<SkillDtoOut>> Get()
    {
        return await repository.GetAll().ConfigureAwait(false);
    }

    // GET api/<SkillsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SkillDtoOut>> Get(int id)
    {
        SkillDtoOut? skill = await repository.GetById(id).ConfigureAwait(false);
        return skill is null ? NotFound() : skill;
    }

    // DELETE api/<SkillsController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool result = await repository.RemoveById(id).ConfigureAwait(false);
        return result ? NoContent() : NotFound();
    }

    // PUT api/<SkillsController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(int id, [FromBody] SkillDtoIn value)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        bool result = await repository.UpdateById(id, value).ConfigureAwait(false);
        return result ? NoContent() : NotFound();
    }
}
