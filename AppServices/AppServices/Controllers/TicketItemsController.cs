﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppServices;

namespace AppServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketItemsController : ControllerBase
    {
        private readonly TicketContext _context;

        public TicketItemsController(TicketContext context)
        {
            _context = context;
        }

        // GET: api/TicketItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketItem>>> GetTicketItems()
        {
            return await _context.TicketItems.ToListAsync();
        }

        // GET: api/TicketItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketItem>> GetTicketItem(long id)
        {
            var ticketItem = await _context.TicketItems.FindAsync(id);

            if (ticketItem == null)
            {
                return NotFound();
            }

            return ticketItem;
        }

        // PUT: api/TicketItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicketItem(long id, TicketItem ticketItem)
        {
            if (id != ticketItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticketItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TicketItems
        [HttpPost]
        public async Task<ActionResult<TicketItem>> PostTicketItem(TicketItem ticketItem)
        {
            _context.TicketItems.Add(ticketItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicketItem", new { id = ticketItem.Id }, ticketItem);
        }

        // DELETE: api/TicketItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TicketItem>> DeleteTicketItem(long id)
        {
            var ticketItem = await _context.TicketItems.FindAsync(id);
            if (ticketItem == null)
            {
                return NotFound();
            }

            _context.TicketItems.Remove(ticketItem);
            await _context.SaveChangesAsync();

            return ticketItem;
        }

        private bool TicketItemExists(long id)
        {
            return _context.TicketItems.Any(e => e.Id == id);
        }
    }
}
