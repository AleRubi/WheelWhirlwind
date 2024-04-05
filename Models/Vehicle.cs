public class Vehicle
{
    public int VehicleId { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? RegistrationMonth { get; set; }
    public int? RegistrationYear { get; set; }
    public int? Mileage  { get; set; }
    public string? Type { get; set; }
    public int? Displacement { get; set; }
    public int? HorsePower { get; set; }
    public string? Fuel { get; set; }
    public string? Transmission { get; set; }
    public string? Emissions { get; set; }
    public int? Seats { get; set; }
    public int? Doors { get; set; }
    public int? Price { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public ICollection<VehicleImage>? VehicleImages { get; set; }
    public ICollection<VehicleListing>? VehicleListings { get; set; }
    public ICollection<VehicleUserFavourite>? VehicleUserFavourites { get; set; }
}