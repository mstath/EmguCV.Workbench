using System.ComponentModel;
using System.Text.RegularExpressions;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.Processors
{
    public interface IImageProcessor
    {
        string Name { get; }

        void Process(ref Image<Gray, byte> image);
    }

    public abstract class ImageProcessor : ViewModelBase, IImageProcessor
    {
        [Browsable(false)]
        public string Name => Regex.Replace(GetType().Name, @"(\B[A-Z])", " $1");

        public abstract void Process(ref Image<Gray, byte> image);
    }
}