namespace AEBackendCodeChallenge.Models
{
    public class Ship
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public  string Name { get; set; }
        public string ShipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Velocity { get; set; }

        // Navigation property
        public ICollection<UserShip> UserShips { get; set; }
    }

}
