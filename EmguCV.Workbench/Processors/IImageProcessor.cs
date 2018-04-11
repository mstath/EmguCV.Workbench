using System.ComponentModel;
using System.Text.RegularExpressions;
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
        /// Gets the name of the image processor.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Applies the processor to the specified image.
        /// </summary>
        /// <param name="image">The image to process.</param>
        void Process(ref Image<Bgr, byte> image);
    }

    public abstract class ImageProcessor : ViewModelBase, IImageProcessor
    {
        [Browsable(false)]
        public string Name => Regex.Replace(GetType().Name, @"(\B[A-Z])", " $1");

        public abstract void Process(ref Image<Bgr, byte> image);
    }
}