using Eticket.Data;
using Eticket.Models;
using Eticket.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Eticket.Repository
{
    public class CartRepository:Repository<Cart>,ICartRepository
    {
        private ApplicationDbcontext DbContext;

        public CartRepository(ApplicationDbcontext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }
    }
}
