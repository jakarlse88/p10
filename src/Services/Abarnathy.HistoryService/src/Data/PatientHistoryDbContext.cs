using System;
using Abarnathy.HistoryService.Models;
using MongoDB.Driver;

namespace Abarnathy.HistoryService.Data
{
    /// <summary>
    /// Application DB context.
    /// </summary>
    public class PatientHistoryDbContext : IDisposable
    {
        private readonly IMongoDatabase _db;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public PatientHistoryDbContext()
        {
        }
        
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dbName"></param>
        public PatientHistoryDbContext(IMongoClient client, string dbName)
        {
            _db = client.GetDatabase(dbName);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IMongoCollection<Note> Notes =>
            _db.GetCollection<Note>("Notes");

        public void Dispose()
        {
        }
    }
}