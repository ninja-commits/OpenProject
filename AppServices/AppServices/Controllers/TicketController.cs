using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppServices.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private TicketContext _context { get; }

        public TicketController(TicketContext context)
        {
            _context = context;

            if (!_context.TicketItems.Any())
            {
                _context.TicketItems.Add(new TicketItem { Concert = "Beyonce" });
                _context.SaveChanges();
            }
        }

        [HttpGet("{Id}", Name = "GetTicket")]
        public IActionResult GetById(long id)
        {
            var ticket = _context.TicketItems.FirstOrDefault(x => x.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return new ObjectResult(ticket);
        }

        [HttpGet]
        public IEnumerable<TicketItem> GetAll()
        {
            return _context.TicketItems.AsNoTracking().ToList();
        }
        [HttpPost]
        public IActionResult Create([FromBody]TicketItem ticket)
        {
            if (ticket == null)
                return BadRequest();

            _context.TicketItems.Add(ticket);
            _context.SaveChanges();

            return CreatedAtRoute("GetTicket", new {id = ticket.Id}, ticket);
        }
    }
}