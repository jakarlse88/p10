using Abarnathy.HistoryAPI.Models;
using MongoDB.Driver;

namespace Abarnathy.HistoryAPI.Data
{
    /// <summary>
    /// Application DB context.
    /// </summary>
    public class PatientHistoryDbContext
    {
        private readonly IMongoDatabase _db;

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
        public IMongoCollection<Note> Notes =>
            _db.GetCollection<Note>("Notes");
    }
}