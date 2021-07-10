using CarShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Services
{
    public class UserService : IUserService
    {
        private readonly CarShopDbContext dbContext;

        public UserService(CarShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool IsMechanic(string userId)
        {
            return this.dbContext.Users.Any(u => u.Id == userId && u.IsMechanic);
        }
    }
}
