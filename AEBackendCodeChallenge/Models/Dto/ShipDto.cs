namespace AEBackendCodeChallenge.Models.Dto
{
    public class ShipDto
    {
        public int ShipId { get; set; }
        public string ShipCode { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Velocity { get; set; }
    }

    public class AddShipDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShipCode { get; set; }
        public string ShipId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Velocity { get; set; }

        // Navigation property
        public List<UserDto> Users { get; set; }
    }


    public class UpdateShipVelocityDto
    {
        public int ShipId { get; set; }
        public double Velocity { get; set; }

    }

    public class AssignUserToShipDto
    {
        public int ShipId { get; set; }
        public List<int> UserIds { get; set; }
    }

    public class ShipWithUsersDto
    {
        public int ShipId { get; set; }
        public string Name { get; set; }
        public string ShipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Velocity { get; set; }
        public List<UserDto> Users { get; set; }
    }

    public class ShipClosestPortDto
    {
        public ShipClosestPortInformationDto PortInformation { get; set; }
        public ShipWithUsersDto shipDetailInfo { get; set; }
        

    }

    public class ShipClosestPortInformationDto
    {
        public Port PortInfo { get; set; }
        public string EstimatedDistance { get; set; }
        public string EstimatedArrivalTime { get; set; }
    }
    public class GetClosestPortDto
    {
        public int id { get; set; }
    }

}
