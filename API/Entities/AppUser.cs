namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required string Location { get; set; }
}
