using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recommend.API.Dtos;

namespace Recommend.API.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetContactsByUserId(int userId);
    }
}
