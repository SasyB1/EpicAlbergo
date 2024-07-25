using EpicAlbergo.Models.Dto;
using EpicAlbergo.Models;

namespace EpicAlbergo.Interfaces
{
    public interface IUserService
    {
        User GetUser(UserDto userDto);
        Task Login(User user);
        Task Logout();
    }
}
