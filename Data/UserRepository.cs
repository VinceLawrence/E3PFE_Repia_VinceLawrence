using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context; //Adding the dbcontext

        public UserRepository(UserContext context) //Adding the constructor
        {
            _context = context;
        }

        public AppUser Create(AppUser appUser) //Adding the user
        {
            _context.AppUsers.Add(appUser);
            appUser.Id = _context.SaveChanges();

            return appUser;
        }

        public AppUser GetByEmail(string email) //Getting the user using email
        {
            return _context.AppUsers.FirstOrDefault(u => u.Email == email);
        }

        public AppUser GetById(int id) //Getting the user by id
        {
            return _context.AppUsers.FirstOrDefault(u => u.Id == id);
        }
    }
}
