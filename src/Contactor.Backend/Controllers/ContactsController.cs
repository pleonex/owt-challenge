namespace Contactor.Backend.Controllers;

using Contactor.Backend.Models.Dto.Contacts;
using Contactor.Backend.Models.Dto.Skills;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ContactsController(IContactsRepository contactsRepo) : ControllerBase
{
    // GET: api/<ContactsController>
    [HttpGet]
    public async Task<IEnumerable<ContactDtoOut>> Get()
    {
        return await contactsRepo.GetAll().ConfigureAwait(false);
    }

    // GET api/<ContactsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDtoOut>> Get(int id)
    {
        ContactDtoOut? contact = await contactsRepo.GetById(id);
        return contact is null ? NotFound() : contact;
    }

    // POST api/<ContactsController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Post([FromBody] ContactDtoIn value)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        int itemId = await contactsRepo.Create(value);
        return CreatedAtAction(nameof(Get), new { id = itemId }, value);
    }

    // PUT api/<ContactsController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Put(int id, [FromBody] ContactDtoIn value)
    {
        bool result = await contactsRepo.UpdateById(id, value);
        if (!result) {
            return BadRequest();
        }

        return NoContent();
    }

    // DELETE api/<ContactsController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        bool result = await contactsRepo.RemoveById(id);
        if (!result) {
            return NotFound();
        }

        return NoContent();
    }

    // POST api/<ContactsController>/5/skills
    [HttpPost("{userId}/skills")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> PostSkill(int userId, [FromBody] SkillDtoIn value)
    {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        int result = await contactsRepo.CreateSkill(userId, value);
        if (result < 0) {
            return NotFound();
        }

        return CreatedAtAction(nameof(Get), new { id = userId }, value);
    }

    // DELETE api/<ContactsController>/5/skills/1
    [HttpDelete("{userId}/skills/{skillId}")]
    public async Task<IActionResult> DeleteSkill(int userId, int skillId)
    {
        bool result = await contactsRepo.DeleteSkill(userId, skillId);

        return result ? NoContent() : NotFound();
    }
}
