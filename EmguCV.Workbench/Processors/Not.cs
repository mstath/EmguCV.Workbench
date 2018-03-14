using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV.Workbench.Processors
{
    public class Not : ImageProcessor
    {
        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.Not();
        }
    }
}
