using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace LiteApp.MySpace.Web.Helpers
{
    public static class ImageProcessor
    {
        public static Image ResampleAsImage(this Image image, int maxWidth, int maxHeight, InterpolationMode quality = InterpolationMode.High)
        {
            return Image.FromStream(ResampleAsStream(image, maxWidth, maxHeight, quality));
        }

        public static Stream ResampleAsStream(this Image image, int maxWidth, int maxHeight, InterpolationMode quality = InterpolationMode.High)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            double nPercent = Math.Min((double)maxWidth / sourceWidth, (double)maxHeight / sourceHeight);
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap canvas = new Bitmap(destWidth, destHeight, PixelFormat.Format32bppPArgb);
            canvas.SetResolution(96, 96);
            using (var pen = Graphics.FromImage(canvas))
            {
                pen.InterpolationMode = quality;
                pen.DrawImage(image, new Rectangle(0, 0, destWidth, destHeight),
                    new Rectangle(0, 0, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            }
            var ms = new MemoryStream();
            canvas.Save(ms, ImageFormat.Jpeg);
            canvas.Dispose();
            return ms;
        }
    }
}