using dbs.blog.Application.Commands;
using dbs.blog.Application.Queries;
using dbs.blog.DTOs;
using dbs.core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : Controller
    {
        private const int PAGE_SIZE = 10;
        private readonly IMediatorHandler _mediatorHandler;

        public ContactsController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;

            var contactsQueryResult = await _mediatorHandler.ProjectionQuery<ContactsQuery, IEnumerable<ContactDTO>>(
                new ContactsQuery() { PageNumber = page });
            
            var totalContactsResult = await _mediatorHandler.ProjectionQuery<CountContactsQuery, int>(new CountContactsQuery());
            var totalContacts = totalContactsResult.ValidationResult.IsValid ? totalContactsResult.Response : 0;
            var totalPages = (int)Math.Ceiling(totalContacts / (double)PAGE_SIZE);

            var contacts = contactsQueryResult.ValidationResult.IsValid 
                ? (contactsQueryResult.Response?.ToList() ?? new List<ContactDTO>()) 
                : new List<ContactDTO>();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalContacts;
            ViewBag.PageSize = PAGE_SIZE;

            return View(contacts);
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var contactQueryResult = await _mediatorHandler.ProjectionQuery<ContactQuery, ContactDTO>(
                new ContactQuery() { ContactId = id });

            if (!contactQueryResult.ValidationResult.IsValid || contactQueryResult.Response == null)
            {
                return NotFound();
            }

            return View(contactQueryResult.Response);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsReceived(Guid contactId)
        {
            var command = new MarkContactAsReceivedCommand
            {
                ContactId = contactId
            };

            var result = await _mediatorHandler.CallCommand<MarkContactAsReceivedCommand>(command);
            
            if (result.IsValid)
            {
                return Ok();
            }

            return BadRequest(result.Errors.Select(e => e.ErrorMessage));
        }
    }
}

