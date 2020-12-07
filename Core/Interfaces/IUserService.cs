using Common;


namespace Core.Interfaces
{
    public interface IUserService
    {
        UserDto GetByUserName(string userName);

        UserDto GetById(int Id);
    }
}
