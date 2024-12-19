using Eticket.Data;
using Eticket.Models;
using Eticket.Repository.IRepository;

namespace Eticket.Repository
{
    public class CinemaRepository:Repository<Cinema> ,ICinemaRepository
    {
        public CinemaRepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbcontext DbContext { get; }
    }
}
