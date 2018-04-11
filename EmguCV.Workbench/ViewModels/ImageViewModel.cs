using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCV.Workbench.Algorithms;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        public EngineViewModel EngineVm { get; private set; }
        public AlgorithmViewModel AlgorithmVm { get; private set; }

        /// <summary>
        /// Initializes the image view model.
        /// </summary>
        public void Initialize(EngineViewModel engineVm, AlgorithmViewModel algorithmVm)
        {
            // set references to the other view models
            EngineVm = engineVm;
            AlgorithmVm = algorithmVm;
        }

        /// <summary>
        /// Converts and sets the image to be presented
        /// in the bound WPF image object.
        /// </summary>
        /// <param name="annotatedImage">The image to be presented.</param>
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

        /// <summary>
        /// Grabs a template image from engine vm.
        /// </summary>
        /// <param name="rect">The ROI of the grabbed image.</param>
        public void GrabTemplate(Rectangle rect)
        {
            try
            {
                // proceed if ROI is realistic
                if (rect.Width < 2 || rect.Height < 2 || rect.X < 0 || rect.Y < 0)
                    return;

                // clone last image from engine vm
                var template = EngineVm.Image.Clone();
                // set ROI on the clone
                template.ROI = rect;
                // set template on selected algorithm
                (AlgorithmVm.SelectedAlgorithm as IImageTemplateAlgorithm)?.SetTemplate(template);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Draws data objects onto image and presents
        /// (for selected data objects in data grid).
        /// </summary>
        /// <param name="objects">The data objects.</param>
        public void DrawObjects(IList objects)
        {
            // clone last image from engine vm
            var image = EngineVm.Image.Clone();

            // create MKeyPoint[] if
            // of type KeyPoint
            var keypoints = objects
                .OfType<KeyPoint>()
                .Select(o => o.GetKeyPoint())
                .ToArray();

            // draw selected keypoints
            Features2DToolbox
                .DrawKeypoints(
                    image,
                    new VectorOfKeyPoint(keypoints),
                    image,
                    new Bgr(Color.Red),
                    Features2DToolbox.KeypointDrawType.DrawRichKeypoints);

            // draw selected boxes
            foreach (var obj in objects.OfType<Box>())
                image.Draw(obj.GetBox(), new Bgr(Color.Red));

            // draw selected circles
            foreach (var obj in objects.OfType<Circle>())
                image.Draw(obj.GetCircle(), new Bgr(Color.Red));

            // draw selected contours
            foreach (var obj in objects.OfType<Contour>())
                image.Draw(obj.GetContour(), new Bgr(Color.Red));

            // draw selected ortated boxes
            foreach (var obj in objects.OfType<RotatedBox>())
                image.Draw(obj.GetBox(), new Bgr(Color.Red), 1);

            // draw selected segments
            foreach (var obj in objects.OfType<Segment>())
                image.Draw(obj.GetSegment(), new Bgr(Color.Red), 1);

            // draw selected ellipses
            foreach (var obj in objects.OfType<RotBoxEllipse>())
                image.Draw(obj.GetEllipse(), new Bgr(Color.Red));

            // present the annoated image
            SetImage(image);
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
