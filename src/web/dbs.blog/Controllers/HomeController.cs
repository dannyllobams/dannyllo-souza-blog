using System.Diagnostics;
using dbs.blog.Application.Commands;
using dbs.blog.Models;
using dbs.core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace dbs.blog.Controllers
{

    public class HomeController : Controller
    {
        private readonly IMediatorHandler _mediator;

        public HomeController(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(SubmitContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var submitContactResponse = await _mediator.CallCommand<SubmitContactCommand>(new SubmitContactCommand()
            {
                Name = model.Name,
                Email = model.Email,
                Message = model.Message
            });

            if (!submitContactResponse.IsValid)
            {
                foreach (var error in submitContactResponse.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

                return View(model);
            }

            TempData["MessageReceived"] = true;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
