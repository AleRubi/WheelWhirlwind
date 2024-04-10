using Microsoft.EntityFrameworkCore;

public static class FilterExtention{
    public static (List<VehicleListing> listings, List<VehicleListing> TotalListings) Filter(this ApplicationDbContext db, int currentPage, int pageSize, string? type, string? brand, string? model, string? fuel, string? emissions, string? location){
        List<VehicleListing> listings = new();
        List<VehicleListing> TotalListings = new();

        listings = db.VehicleListings
            .Where(vl => db.Vehicles.Any(v => 
                v.VehicleId == vl.VehicleId 
                && (type == null || v.Type == type) 
                && (brand == null || v.Brand == brand)
                && (model == null || v.Model == model)
                && (fuel == null || v.Fuel == fuel)
                && (emissions == null || v.Emissions == emissions)
                && (location == null || v.Location == location)
            ))
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        TotalListings = db.VehicleListings
            .Where(vl => db.Vehicles.Any(v =>
                v.VehicleId == vl.VehicleId 
                && (type == null || v.Type == type) 
                && (brand == null || v.Brand == brand)
                && (model == null || v.Model == model)
                && (fuel == null || v.Fuel == fuel)
                && (emissions == null || v.Emissions == emissions)
                && (location == null || v.Location == location)
            ))
            .ToList();

        return (listings, TotalListings);
    }
}