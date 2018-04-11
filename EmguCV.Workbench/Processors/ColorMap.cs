using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Apply color map to the image.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class ColorMap : ImageProcessor
    {
        private ColorMapType _colorMapType = ColorMapType.Autumn;
        [Category("ApplyColorMap")]
        [PropertyOrder(0)]
        [DisplayName(@"Color Map Type")]
        public ColorMapType ColorMapType
        {
            get { return _colorMapType; }
            set { Set(ref _colorMapType, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            CvInvoke.ApplyColorMap(image, image, _colorMapType);
        }
    }
}
