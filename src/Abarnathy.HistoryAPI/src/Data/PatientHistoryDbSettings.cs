namespace Abarnathy.HistoryAPI.Data
{
    public class PatientHistoryDbSettings : IPatientHistoryDbSettings
    {
        public const string SectionName = "PatientHistoryDbSettings";
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}