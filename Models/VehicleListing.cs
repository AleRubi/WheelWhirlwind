using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class VehicleListing
{
    [Key]
    public int? VehicleListingId  { get; set; }
    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    
    public int? UserId { get; set; }
    public User? User { get; set; }
}