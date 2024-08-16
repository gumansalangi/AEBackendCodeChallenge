namespace AEBackendCodeChallenge.Models
{
    public class UserShip
    {
        public int UserId { get; set; }
        public User User { get; set; } 


        public int ShipId { get; set; }
        public Ship Ship { get; set; } 
        

    }
}
