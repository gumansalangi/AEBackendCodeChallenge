namespace AEBackendCodeChallenge.Models.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }

    public class AddUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public List<ShipDto> Ships { get; set; }
    }

    public class AssignShipToUserDto
    {
        public int UsersId { get; set; }
        public List<int> ShipIds { get; set; }
    }

    public class UserWithShipsDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public List<ShipDto> Ships { get; set; }
    }


}
