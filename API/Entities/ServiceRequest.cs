using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class ServiceRequest
{
    public int Id { get; set; }

    [Required]
    public required string Description { get; set; } 
    [Required]
    public required string Location { get; set; }
    [Required]
    public DateTime RequestDate { get; set; }
    [Required]
    public required string Priority { get; set; }
    public required string Status { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int? TechnicianId { get; set; }
    public Technician? Technician { get; set; }
}
