using System.ComponentModel.Composition;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Converts image to Gray Scale.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class GrayScale : ImageProcessor
    {
        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Convert<Gray, byte>().Convert<Bgr, byte>();
        }
    }
}
