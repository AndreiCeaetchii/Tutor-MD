namespace Tutor.Application.Features.Photos.DTOs;

public class PhotoDto
{
    public required string PublicId { get; set; }
    public required string Url { get; set; }
    public required string MimeType { get; set; }
    public required string Provider { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public long? Bytes { get; set; }
}