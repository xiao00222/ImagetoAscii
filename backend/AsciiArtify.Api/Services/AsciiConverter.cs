using System;

namespace AsciiArtify.Api.Services;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

public static class AsciiConverter
{
    private const string Ramp = "@%#*+=-:. "; // dark -> light

    public static string Convert(Image<Rgba32> image, int outputWidth = 100)
    {
        int width = outputWidth;
        // character cells are taller than wide, so shrink height to compensate
        int height = (int)(image.Height * (width / (double)image.Width) * 0.55);
        if (height < 1) height = 1;

        image.Mutate(x => x.Resize(width, height).Grayscale());

        var sb = new StringBuilder();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                double brightness = image[x, y].R / 255.0;
                int index = (int)((1 - brightness) * (Ramp.Length - 1));
                sb.Append(Ramp[index]);
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }
}