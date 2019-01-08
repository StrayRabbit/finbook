using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dto;
using Contact.API.Dtos;

namespace Contact.API.Data
{
    public interface IContactRepository
    {
        /// <summary>
        /// 更新联系人信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<bool> UpdateContactInfo(UserIdentity userInfo, CancellationToken cancellationToken);

        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddContact(int userId, UserIdentity contact, CancellationToken cancellationToken);

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Models.Contact>> GetContactsAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// 更新好友标签
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<bool> TagContactAsync(int userId, int contactId, List<string> tags, CancellationToken cancellationToken);
    }
}
