using System.ComponentModel.DataAnnotations;

namespace Jordnaer.Shared;

public class DataForsyningenOptions
{
    public const string SectionName = "DataForsyningen";

    [Url]
    [Required(ErrorMessage = "P�kr�vet.")]
    public required string BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed search radius, in meters.
    /// </summary>
    public int MaxSearchRadiusKilometers { get; set; } = 50;
}
