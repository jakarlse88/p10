namespace Abarnathy.DemographicsAPI.Models
{
    public class AddressInputModel
    {
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public int Id { get; set; }
    }
}
