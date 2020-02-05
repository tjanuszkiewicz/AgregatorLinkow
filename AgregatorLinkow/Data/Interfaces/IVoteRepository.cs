using System.Threading.Tasks;
using AgregatorLinkow.Models;

namespace AgregatorLinkow.Data.Interfaces
{
    public interface IVoteRepository
    {
        Task AddVote(Vote vote);
        Task RemoveVote(int id);
    }
}