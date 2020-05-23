using System;
using Abarnathy.HistoryAPI.Models;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace Abarnathy.HistoryAPI.Test.Integration
{
    public class MongoIntegrationTest
    {
        internal static MongoDbRunner _runner;
        internal static IMongoCollection<Note> _collection;

        internal static void CreateConnection()
        {
            _runner = MongoDbRunner.Start();
            
            var client = new MongoClient(_runner.ConnectionString);
            var db = client.GetDatabase("IntegrationTest");
            _collection = db.GetCollection<Note>("Notes");
        }
    }
}