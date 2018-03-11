using Emgu.CV;
using Emgu.CV.Structure;

namespace EmguCV.Workbench.Processors
{
    public class EqualizeHist : ImageProcessor
    {
        public override void Process(ref Image<Gray, byte> image)
        {
            var eqh = new Image<Gray, byte>(image.Width, image.Height);
            CvInvoke.EqualizeHist(image, eqh);
            image = eqh;
        }
    }
}
