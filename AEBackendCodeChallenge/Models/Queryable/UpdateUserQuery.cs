namespace AEBackendCodeChallenge.Models.Queryable
{
    public class UpdateUserQuery
    {
        public int? UsersId { get; set; }
        public string? ShipId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }

    }
}
