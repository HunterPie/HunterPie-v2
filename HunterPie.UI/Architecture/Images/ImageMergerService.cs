using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace HunterPie.UI.Architecture.Images;
public static class ImageMergerService
{

    /// <summary>
    /// Merges two images and saves it to the desired path
    /// </summary>
    /// <param name="outputPath">Path where the image should be saved</param>
    /// <param name="image">Image to be rendered under the mask</param>
    /// <param name="mask">The mask that will be rendered on top of image</param>
    /// <returns>Path to the saved file</returns>
    public static Task<string> MergeAsync(string outputPath, string image, string mask)
    {
        using var backgroundImage = Image.FromFile(image);
        using var maskImage = Image.FromFile(mask);


        using var graphics = Graphics.FromImage(backgroundImage);
        graphics.DrawImage(maskImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height));
        _ = graphics.Save();

        using var output = new Bitmap(backgroundImage);

        output.Save(outputPath, ImageFormat.Png);
        return Task.FromResult(outputPath);
    }
}