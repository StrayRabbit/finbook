using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Contact.API.Models
{
    /// <summary>
    /// 通讯录
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ContactBook
    {
        public ContactBook()
        {
            Contacts = new List<Contact>();
        }

        public int UserId { get; set; }

        /// <summary>
        /// 联系人列表
        /// </summary>
        public List<Contact> Contacts { get; set; }
    }
}
