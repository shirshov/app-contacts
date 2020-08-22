using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyContacts.API.Models;
using MyContacts.Shared.Models;

namespace MyContacts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        readonly ContactContext context;

        public ContactsController(ContactContext context)
        {
            this.context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return await context.Contacts.ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(string id)
        {
            var contact = await context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public async Task<IActionResult> PutContact([FromBody]Contact contact)
        {

            context.Entry(contact).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!ContactExists(contact.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact([FromBody]Contact contact)
        {
            contact = contact.With(id: Guid.NewGuid().ToString());
            context.Contacts.Add(contact);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ContactExists(contact.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw ex;
                }
            }

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact>> DeleteContact(string id)
        {
            var contact = await context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            context.Contacts.Remove(contact);
            await context.SaveChangesAsync();

            return contact;
        }

        bool ContactExists(string id)
        {
            return context.Contacts.Any(e => e.Id == id);
        }
    }
}
