using System;

namespace AsciiArtify.Api;

public class ConversionRecord
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}