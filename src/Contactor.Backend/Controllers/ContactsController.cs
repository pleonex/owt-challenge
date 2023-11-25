namespace Contactor.Backend.Controllers;

using Contactor.Backend.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ContactsController(IContactsRepository contactsRepo) : ControllerBase
{
    // GET: api/<ContactsController>
    [HttpGet]
    public async Task<IEnumerable<ContactDto>> Get()
    {
        return await contactsRepo.GetAll();
    }

    // GET api/<ContactsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> Get(int id)
    {
        ContactDto? contact = await contactsRepo.GetById(id);
        return contact is null ? NotFound() : contact;
    }

    // POST api/<ContactsController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ContactDto>> Post([FromBody] ContactDto value)
    {
        if (ModelState.IsValid) {
            int itemId = await contactsRepo.Create(value);
            return CreatedAtAction(nameof(Get), new { id = itemId }, value);
        }

        return BadRequest();
    }

    // PUT api/<ContactsController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Put(int id, [FromBody] ContactDto value)
    {
        if (id != value.Id) {
            return BadRequest();
        }

        bool result = await contactsRepo.UpdateById(value);
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
}
