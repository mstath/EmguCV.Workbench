using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Compute the complement image (invert).
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class Not : ImageProcessor
    {
        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.Not();
        }
    }
}
