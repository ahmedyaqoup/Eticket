using Eticket.Data;
using Eticket.Models;
using Eticket.Repository.IRepository;

namespace Eticket.Repository
{
    public class ActorMovieRepository : Repository<ActorMovies>, IActorMovieRepository
    {
        private readonly ApplicationDbcontext dbContext;

        public ActorMovieRepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
