using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Normalizes brightness and increases contrast of Gray Scale image.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class EqualizeHist : ImageProcessor
    {
        public override void Process(ref Image<Bgr, byte> image)
        {
            var dst = new Image<Gray, byte>(image.Width, image.Height);
            CvInvoke.EqualizeHist(image.Convert<Gray, byte>(), dst);
            image = dst.Convert<Bgr, byte>();
        }
    }
}
