using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace OurTeamViewer.Helpers
{
    public class ImageHelper
    {

        public string GetImagePath(byte[] buffer)
        {
            ImageConverter ic = new ImageConverter();
            var data = ic.ConvertFrom(buffer);

            Image img = data as Image;
            if (img != null)
            {
                Bitmap bitmap1 = new Bitmap(img);

                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Images2");
                Directory.CreateDirectory(path);
                var strGuid = Guid.NewGuid().ToString();
                bitmap1.Save($@"{path}\image{strGuid}.png");
                var imagepath = $@"{path}\image{strGuid}.png";
                return imagepath;
            }
            else
            {
                return String.Empty;
            }

        }
        public byte[] GetBytesOfImage(string path)
        {
            var image = new Bitmap(path);
            ImageConverter imageconverter = new ImageConverter();
            var imagebytes = ((byte[])imageconverter.ConvertTo(image, typeof(byte[])));
            return imagebytes;
        }

        public string TakeScreenShot()
        {
            Bitmap bmp = new Bitmap(1920, 1080);
            string path;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(1920, 1080));

                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Images2");

                bmp.Save(path + "\\screenshot" + Guid.NewGuid().ToString() + ".png");  // saves the image
            }
            var source = path + "\\screenshot" + Guid.NewGuid().ToString() + ".png";
            return source;
        }
    }
}
