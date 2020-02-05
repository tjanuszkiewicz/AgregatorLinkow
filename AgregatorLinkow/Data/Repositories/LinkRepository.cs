using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AgregatorLinkow.Data.Interfaces;
using AgregatorLinkow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgregatorLinkow.Data.Repositories
{
    public class LinkRepository : ILinkRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public LinkRepository(ApplicationDbContext dbContext, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResponse<Link>> GetHomePageLinks(int? pageNumber)
        {
            var links = _dbContext.Links
                .Include(u => u.User)
                .Include(v => v.Votes)
                .ThenInclude(u => u.User)
                .Where(l => l.UploadDate >= DateTime.Now.AddDays(-5))
                .OrderByDescending(l => l.Points);

            var pageSize = 100;

            return await PagedResponse<Link>.CreateAsync(links.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<PagedResponse<Link>> GetUserLinks(int? pageNumber)
        {
            var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await _userManager.FindByEmailAsync(userEmail);

            var pageSize = 100;

            var links = await PagedResponse<Link>.CreateAsync(_dbContext.Links
                    .Include(u => u.User)
                    .Include(v => v.Votes)
                    .ThenInclude(u => u.User)
                    .Where(l => l.User == user)
                    .OrderByDescending(l => l.Points), pageNumber ?? 1, pageSize);

            return links;
        }

        public async Task<Link> GetLinkWithVotes(int id)
        {
            var link = await _dbContext.Links.Include(v => v.Votes).FirstOrDefaultAsync(l => l.Id == id);
            return link;
        }

        public async Task<Link> GetLink(int id)
        {
            var link = await _dbContext.Links.FindAsync(id);
            return link;
        }

        public async Task Create(Link link)
        {
            var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await _userManager.FindByEmailAsync(userEmail);

            link.UploadDate = DateTime.Now;
            link.User = user;

            await _dbContext.Links.AddAsync(link);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Edit(Link link)
        {
            _dbContext.Links.Update(link);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var link = await _dbContext.Links.FindAsync(id);
            _dbContext.Links.Remove(link);
            await _dbContext.SaveChangesAsync();
        }
    }
}