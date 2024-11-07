namespace API.Entities;

public class Technician
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Specialization { get; set; }
    public required string Location { get; set; }
    public bool IsAvailable { get; set; } = true; //default to true
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
