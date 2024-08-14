namespace AEBackendCodeChallenge.Models
{
    public class Port
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
