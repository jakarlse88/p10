namespace Abarnathy.BlazorClient.Client.Models
{
    public class AddressInputModel
    {
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public override string ToString()
        {
            return $"{StreetName} {HouseNumber}, {Town}";
        }
    }
}
