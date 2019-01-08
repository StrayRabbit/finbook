using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dto;
using Contact.API.Dtos;
using Contact.API.Models;
using MongoDB.Driver;
using Contact = Contact.API.Models.Contact;

namespace Contact.API.Data
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly ContactContext _contactContext;

        public MongoContactRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }

        public async Task<bool> UpdateContactInfo(UserIdentity userInfo, CancellationToken cancellationToken)
        {
            //查找该用户的通讯录，如果没没有通讯录则返回
            var contactBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == userInfo.UserId, null, cancellationToken)).FirstOrDefault(cancellationToken);//.ToList(cancellationToken);
            if (contactBook == null)
            {
                return true;
            }

            //取出该用户的 所有联系人的id
            var contactIds = contactBook.Contacts.Select(c => c.UserId);
            //有通讯录，用mongodb的关联内部查询方式匹配
            //定义 filterdifinition 用and 条件连接符
            //所有联系人的contactBook 
            //contackbook 中的 contact.UserId==userInfo.UserId 
            var filter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.In(c => c.UserId, contactIds),
                Builders<ContactBook>.Filter.ElemMatch(c => c.Contacts, contact => contact.UserId == userInfo.UserId)
            );

            //定义 updatedefinition
            var update = Builders<ContactBook>.Update
                .Set("Contacts.$.Name", userInfo.Name)
                .Set("Contacts.$.Avatar", userInfo.Avatar)
                .Set("Contacts.$.Company", userInfo.Company)
                .Set("Contacts.$.Title", userInfo.Title);

            var updateResult = _contactContext.ContactBooks.UpdateMany(filter, update);
            return updateResult.MatchedCount == updateResult.ModifiedCount && updateResult.MatchedCount == long.Parse("1");
        }

        public async Task<bool> AddContact(int userId, UserIdentity contact, CancellationToken cancellationToken)
        {
            if (await _contactContext.ContactBooks.CountAsync(c => c.UserId == userId, cancellationToken: cancellationToken) == 0)
            {
                await _contactContext.ContactBooks.InsertOneAsync(new ContactBook() { UserId = userId }, cancellationToken: cancellationToken);
            }

            var filter = Builders<ContactBook>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<ContactBook>.Update.AddToSet(c => c.Contacts, new Models.Contact()
            {
                UserId = contact.UserId,
                Name = contact.Name,
                Avatar = contact.Avatar,
                Company = contact.Company,
                Title = contact.Title,
            });

            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken);

            return result.MatchedCount == result.ModifiedCount && result.MatchedCount == long.Parse("1");
        }

        public async Task<List<Models.Contact>> GetContactsAsync(int userId, CancellationToken cancellationToken)
        {
            var contactBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == userId, cancellationToken: cancellationToken)).FirstOrDefault();
            if (contactBook == null)
            {
                return new List<Models.Contact>();
            }

            return contactBook.Contacts;
        }

        public async Task<bool> TagContactAsync(int userId, int contactId, List<string> tags, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.Eq(c => c.UserId, userId),
                Builders<ContactBook>.Filter.Eq("Contacts.UserId", contactId));

            var update = Builders<ContactBook>.Update
                .Set("Contact.$.Tags", tags);

            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken);
            return result.MatchedCount == result.ModifiedCount && result.MatchedCount == 1;
        }
    }
}
