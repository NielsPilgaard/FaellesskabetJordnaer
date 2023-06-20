namespace Jordnaer.Shared;

public class ProfileDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public int? ZipCode { get; set; }

    public string? City { get; set; }

    public string? Description { get; set; }

    public List<LookingFor> LookingFor { get; set; } = new();

    public List<ChildProfileDto> ChildProfiles { get; set; } = new();

    public DateTime? DateOfBirth { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public int? GetAge() => DateOfBirth.GetAge();

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public string DisplayLocation()
    {
        if (ZipCode is not null && City is not null)
        {
            return $"{ZipCode}, {City}";
        }

        if (ZipCode is not null)
        {
            return ZipCode.ToString()!;
        }

        return City ?? "Område ikke angivet";
    }
}
