using Jordnaer.Shared.Contracts;

namespace Jordnaer.Shared;

public class ChildProfileDto
{
    public required string FirstName { get; set; }

    public string? LastName { get; set; }

    public Gender Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Description { get; set; }

    public string PictureUrl { get; set; } = ProfileConstants.Default_Profile_Picture;

    public int? GetAge() => DateOfBirth.GetAge();
}
