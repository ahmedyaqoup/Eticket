using Eticket.Data;
using Eticket.Models;
using Eticket.Repository.IRepository;

namespace Eticket.Repository
{
    public class MovieRepository:Repository<Movie> ,IMovieRepository
    {
        public MovieRepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbcontext DbContext { get; }
    }
}
