using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

using HtmlAgilityPack;

using Microsoft.Extensions.Logging;

namespace HtmlParser.Common
{
    public class Utils
    {
        private readonly ILogger<Utils> _logger;
        public Utils(ILogger<Utils> logger)
        {
            _logger = logger;
        }

        public Task<HtmlDocument> GetHtmlDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            try
            {
                document = web.Load(url);
            }
            catch (Exception err)
            {
                _logger.LogError(message: $"Error from: {MethodBase.GetCurrentMethod()?.Name} Error message: {err}", args: err);
            }
            return Task.FromResult(document);
        }

        /*        public void CompressResourceImages(string path, string fileName)
                {
                    try
                    {
                        const long qualityLevel = 50L;
                        const int size = 1300;
                        string imagePath = Path.Combine(path, fileName);
                        using Bitmap bmp = new(imagePath);
                        Bitmap resizedBmp = new(size, size);
                        using var graphics = Graphics.FromImage(resizedBmp);
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                        graphics.DrawImage(bmp, 0, 0, size, size);

                        Encoder qualityParamId = Encoder.Quality;
                        EncoderParameters encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, qualityLevel);
                        ImageCodecInfo? codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                        try
                        {
                            FileInfo info = new FileInfo(imagePath);
                            using (FileStream fstream = info.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                fstream.Close();

                                if (File.Exists(imagePath))
                                    File.Delete(imagePath);
                                resizedBmp.Save(imagePath, codec, encoderParameters);
                                resizedBmp.Dispose();
                            }
                        }
                        catch (IOException err)
                        {
                            Console.WriteLine($"File Access Error: {err}");
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err);
                    }
                }*/

        public void ResizeImage(string path, string name)
        {
            int width = 1080, height = 812;
            string filename = Path.Combine(path, name);
            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
                if (!File.Exists(filename))
                    throw new FileNotFoundException($"No image file with {name} on the path {path}");
                Image image = Image.FromFile(filename);

                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);
                const long qualityLevel = 50L;
                Encoder qualityParamId = Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(qualityParamId, qualityLevel);
                ImageCodecInfo? codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                try
                {
                    FileInfo info = new FileInfo(filename);
                    using (FileStream fstream = info.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fstream.Close();

                        if (File.Exists(filename))
                            File.Delete(filename);
                        destImage.Save(filename, codec, encoderParameters);
                        destImage.Dispose();
                    }
                }
                catch (IOException err)
                {
                    _logger.LogInformation($"File Access Error: {err}", err);
                }
#pragma warning restore CA1416 // Validate platform compatibility
            }
            catch (Exception err)
            {
                _logger.LogError($"Error {err} from {MethodBase.GetCurrentMethod()?.Name}", err);
            }
            finally
            {
                GC.Collect();
                _logger.LogInformation($"Garbage Collector called from {MethodBase.GetCurrentMethod()?.Name}");
            }
        }

        public void TrackMoviesUpdate()
        {
            DateTime currentDate = DateTime.UtcNow;
        }
    }
}
