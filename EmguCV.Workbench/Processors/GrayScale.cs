using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV.Workbench.Processors
{
    public class GrayScale : ImageProcessor
    {
        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Convert<Gray, byte>().Convert<Bgr, byte>();
        }
    }
}
