using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        public void SetImage(Image<Bgr,byte> annotatedImage)
        {
            if (annotatedImage != null)
            {
                var bitmapImage = new BitmapImage();
                using (var memoryStream = new MemoryStream())
                {
                    annotatedImage.Bitmap.Save(memoryStream, ImageFormat.Bmp);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                }
                bitmapImage.Freeze();
                Image = bitmapImage;
            }
            else
                Image = null;
        }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }

        private string _frameTimeStatus;
        public string FrameTimeStatus
        {
            get { return _frameTimeStatus; }
            set { Set(ref _frameTimeStatus, value);}
        }

        private string _frameSizeStatus;
        public string FrameSizeStatus
        {
            get { return _frameSizeStatus; }
            set { Set(ref _frameSizeStatus, value); }
        }

        private IEnumerable<object> _data;
        public IEnumerable<object> Data
        {
            get { return _data; }
            set { Set(ref _data, value);}
        }
    }
}
