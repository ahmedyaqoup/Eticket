using Eticket.Data;
using Eticket.Models;
using Eticket.Repository.IRepository;

namespace Eticket.Repository
{
    public class ActorRepository:Repository<Actor> ,IActorRepository
    {
        public ActorRepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbcontext DbContext { get; }
    }
}
