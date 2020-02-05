using System.Threading.Tasks;
using AgregatorLinkow.Data.Interfaces;
using AgregatorLinkow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace AgregatorLinkow.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly ILinkRepository _linkRepository;

        public ProfileController(IToastNotification toastNotification, ILinkRepository linkRepository)
        {
            _toastNotification = toastNotification;
            _linkRepository = linkRepository;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            ViewData["linkList"] = await _linkRepository.GetUserLinks(pageNumber);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title, URL")] Link link)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _linkRepository.Create(link);
                    _toastNotification.AddSuccessToastMessage("Successfully added new link");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}