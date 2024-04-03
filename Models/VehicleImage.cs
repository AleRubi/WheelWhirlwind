public class VehicleImage
{
    public int? VehicleImageId { get; set; }
    public string? Path { get; set; }

    public int? VehicleId { get; set; }
    public virtual Vehicle? Vehicle { get; set; }
}