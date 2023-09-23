using System;
using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Agrin2.Helper.UIHelper.Image
{
    //public class ImageHelper
    //{
    //    const int size = 150;
    //    const int quality = 75;
    //    public static void createThumbnailImage(byte[] content,string directory,string originFilePath,string fileName)
    //    { 

    //        using (var image = new Bitmap(System.Drawing.Image.FromFile(originFilePath)))
    //        {
    //            int width, height;
    //            if (image.Width > image.Height)
    //            {
    //                width = size;
    //                height = Convert.ToInt32(image.Height * size / (double)image.Width);
    //            }
    //            else
    //            {
    //                width = Convert.ToInt32(image.Width * size / (double)image.Height);
    //                height = size;
    //            }
    //            var resized = new Bitmap(width, height);
    //            using (var graphics = Graphics.FromImage(resized))
    //            {
    //                graphics.CompositingQuality = CompositingQuality.HighSpeed;
    //                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
    //                graphics.CompositingMode = CompositingMode.SourceCopy;
    //                graphics.DrawImage(image, 0, 0, width, height);
    //                if (!Directory.Exists(directory))
    //                    Directory.CreateDirectory(directory);
    //                var filePath = directory + "/" + fileName;
    //                using (var output = File.Open(filePath, FileMode.Create))
    //                {
    //                    var qualityParamId = Encoder.Quality;
    //                    var encoderParameters = new EncoderParameters(1);
    //                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
    //                    var codec = ImageCodecInfo.GetImageDecoders()
    //                        .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
    //                    resized.Save(output, codec, encoderParameters);
    //                }
    //            }
    //        }
    //    }

    //}
}
