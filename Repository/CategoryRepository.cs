using Eticket.Data;
using Eticket.Models;
using Eticket.Repository.IRepository;

namespace Eticket.Repository
{
    public class CategoryRepository:Repository<Category> ,ICategoryRepository
    {
        private readonly ApplicationDbcontext dbContext;

        public CategoryRepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
