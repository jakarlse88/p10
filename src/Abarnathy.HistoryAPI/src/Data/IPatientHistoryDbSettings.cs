namespace Abarnathy.HistoryAPI.Data
{
    public interface IPatientHistoryDbSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}