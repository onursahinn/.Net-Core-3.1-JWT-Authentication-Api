using Common;
using Context;
using Core.Interfaces;
using System.Linq;

namespace Core
{
    public class UserService : IUserService
    {
        private readonly CurrentContext _contex;
        public UserService(CurrentContext context)
        {
            _contex = context;
        }


        public UserDto GetByUserName(string userName)
        {
            return _contex.Users.Where(x => x.UserName == userName && x.IsActive && !x.IsDeleted).Select(t => new UserDto
            {
                Id = t.Id,
                Username = t.UserName,
                FirstName = t.Name,
                LastName = t.LastName,
                Password = t.PasswordHash

            }).SingleOrDefault();
        }

        public UserDto GetById(int Id)
        {
            return _contex.Users.Where(x => x.Id == Id && x.IsActive && !x.IsDeleted).Select(t => new UserDto
            {
                Id = t.Id,
                Username = t.UserName,
                FirstName = t.Name,
                LastName = t.LastName,
                Password = t.PasswordHash

            }).SingleOrDefault();
        }

    }
}
