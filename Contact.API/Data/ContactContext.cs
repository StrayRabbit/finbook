using System.Collections.Generic;
using Contact.API.Dto;
using Contact.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class ContactContext
    {
        private IMongoDatabase _database;
        private IMongoCollection<Models.Contact> _collection;
        private AppSettings _appSettings;

        public ContactContext(IMongoDatabase database, IMongoCollection<Models.Contact> collection, AppSettings appSettings)
        {
            _database = database;
            _collection = collection;
            _appSettings = appSettings;
        }

        public ContactContext(IOptionsSnapshot<AppSettings> settings)
        {
            _appSettings = settings.Value;
            var client = new MongoClient(_appSettings.MongoContactConnectionString);
            if (client != null)
            {
                //it will actually creatr the database if it has not already been created
                _database = client.GetDatabase(_appSettings.ContactDatabaseName);
            }
            //MongoDB.Driver.Core.Configuration.ClusterBuilder
            //var settings1 = new MongoClientSettings
            //{
            //    ClusterConfigurator = cb =>
            //    {
            //        //var textWriter = TextWriter.Synchronized(new StreamWriter("mylogfile.txt"));
            //        //cb.AddListener(new LogListener(textWriter));
            //    }
            //};
        }

        public void CheckAndCreate(string collectionName)
        {
            var collections = _database.ListCollections().ToList();
            var collectionNames = new List<string>();
            collections.ForEach(d => collectionNames.Add(d["name"].AsString));
            if (!collectionNames.Contains(collectionName))
            {
                _database.CreateCollection(collectionName);
            }
        }

        /// <summary>
        /// 好友申请请求记录
        /// </summary>
        public IMongoCollection<ContactApplyRequest> ContactApplyRequests
        {
            get
            {
                CheckAndCreate("ContactApplyRequest");
                return _database.GetCollection<ContactApplyRequest>("ContactApplyRequest");
            }
        }

        /// <summary>
        /// 用户通讯录
        /// </summary>
        public IMongoCollection<ContactBook> ContactBooks
        {
            get
            {
                CheckAndCreate("ContactBook");
                return _database.GetCollection<ContactBook>("ContactBook");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IMongoCollection<Models.Contact> Contacts
        {
            get
            {
                CheckAndCreate("Contact");
                return _database.GetCollection<Models.Contact>("Contact");
            }
        }
    }
}
