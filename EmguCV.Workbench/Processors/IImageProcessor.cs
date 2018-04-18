using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Image processor interface.
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Applies the processor to the specified image.
        /// </summary>
        /// <param name="image">The image to process.</param>
        void Process(ref Image<Bgr, byte> image);
    }

    public abstract class ImageProcessor : ViewModelBase, IImageProcessor
    {
        public abstract void Process(ref Image<Bgr, byte> image);

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}