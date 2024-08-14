namespace AEBackendCodeChallenge.Models
{
    public class Ship
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public  string Name { get; set; }
        public string ShipId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Velocity { get; set; }

        // Foreign key
        public int? UserId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        // Navigation property
        public User? User { get; set; }
    }

}
