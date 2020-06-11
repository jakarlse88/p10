namespace Abarnathy.DemographicsService.Models
{
    public class AddressInputModel
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

    }
}
