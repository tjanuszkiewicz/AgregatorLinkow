using System.Linq;
using System.Threading.Tasks;
using AgregatorLinkow.Data;
using AgregatorLinkow.Data.Interfaces;
using AgregatorLinkow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace AgregatorLinkow.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<User> _userManager;
        private readonly IVoteRepository _voteRepository;

        public HomeController(
            UserManager<User> userManager, IToastNotification toastNotification, ILinkRepository linkRepository,
            IVoteRepository voteRepository)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _linkRepository = linkRepository;
            _voteRepository = voteRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var links = await _linkRepository.GetHomePageLinks(pageNumber);

            return View(links);
        }

        public async Task<IActionResult> Vote(int linkId, bool voteType)
        {
            var link = await _linkRepository.GetLinkWithVotes(linkId);
            var user = await _userManager.GetUserAsync(User);

            try
            {
                if (voteType)
                {
                    if (link.Points == int.MaxValue)
                    {
                        _toastNotification.AddWarningToastMessage("Link has already maximum number of points");
                        return RedirectToAction(nameof(Index));
                    }

                    if (link.User == user)
                    {
                        _toastNotification.AddWarningToastMessage("You cannot vote for your links");
                        return RedirectToAction(nameof(Index));
                    }

                    if (link.Votes.FirstOrDefault(v => v.User == user) != null)
                    {
                        _toastNotification.AddWarningToastMessage("You already have voted for this link");
                        return RedirectToAction(nameof(Index));
                    }

                    link.Points += 1;
                    var vote = new Vote
                    {
                        Link = link,
                        User = user,
                        Voted = true
                    };

                    await _voteRepository.AddVote(vote);
                }
                else
                {
                    link.Points -= 1;
                    await _linkRepository.Edit(link);

                    var vote = link.Votes.First(v => v.User == user);

                    await _voteRepository.RemoveVote(vote.Id);
                }

                _toastNotification.AddSuccessToastMessage("Successfully voted");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(int linkId)
        {
            var link = await _linkRepository.GetLink(linkId);
            if (link == null) return NotFound();

            return View(link);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPost([Bind("Id, Title, URL")] Link link)
        {
            var linkToUpdate = await _linkRepository.GetLink(link.Id);
            if (linkToUpdate == null)
            {
                _toastNotification.AddErrorToastMessage("Couldn't find this link");
                return RedirectToAction(nameof(Index));
            }

            linkToUpdate.Title = link.Title;
            linkToUpdate.URL = link.URL;

            try
            {
                await _linkRepository.Edit(linkToUpdate);
                _toastNotification.AddSuccessToastMessage("Successfully edited link");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                _toastNotification.AddErrorToastMessage("Error while edditing link");
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Delete(int linkId)
        {
            try
            {
                await _linkRepository.Delete(linkId);
                _toastNotification.AddSuccessToastMessage("Successfully deleted link");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                _toastNotification.AddErrorToastMessage("Error while deleting link");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}