using System.Threading.Tasks;
using AgregatorLinkow.Models;

namespace AgregatorLinkow.Data.Interfaces
{
    public interface ILinkRepository
    {
        Task<PagedResponse<Link>> GetHomePageLinks(int? pageNumber);
        Task<PagedResponse<Link>> GetUserLinks(int? pageNumber);
        Task<Link> GetLinkWithVotes(int id);
        Task<Link> GetLink(int id);
        Task Create(Link link);
        Task Edit(Link link);
        Task Delete(int id);
    }
}