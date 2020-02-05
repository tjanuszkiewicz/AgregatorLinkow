using System.Threading.Tasks;
using AgregatorLinkow.Data.Interfaces;
using AgregatorLinkow.Models;

namespace AgregatorLinkow.Data.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VoteRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddVote(Vote vote)
        {
            await _dbContext.Votes.AddAsync(vote);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveVote(int id)
        {
            var vote = await _dbContext.Votes.FindAsync(id);
            _dbContext.Votes.Remove(vote);
            await _dbContext.SaveChangesAsync();
        }
    }
}