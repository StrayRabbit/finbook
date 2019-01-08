using System.Threading.Tasks;
using User.Identity.Dtos;

namespace User.Identity.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 检查手机号是否已注册，如果没有注册的话就注册一个用户
        /// </summary>
        /// <param name="phone"></param>
        Task<UserInfo> CheckOrCreate(string phone);
    }
}