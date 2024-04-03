public class User
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public int Stars { get; set; }
    public int NumberStars { get; set; }
    public ICollection<VehicleListing>? VehicleListings { get; set; }
    public ICollection<VehicleUserFavourite>? VehicleUserFavourites { get; set; }
}