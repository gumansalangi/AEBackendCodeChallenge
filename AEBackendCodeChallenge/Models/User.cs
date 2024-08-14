namespace AEBackendCodeChallenge.Models
{
    public class User
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        // Navigation property
        [System.Text.Json.Serialization.JsonIgnore]
        public List<Ship>? Ships { get; set; }
    }

}
