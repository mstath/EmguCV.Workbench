using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.Algorithms
{
    public interface IImageAlgorithm
    {
        string Name { get; }

        int Order { get; }

        void Process(Image<Gray, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data);
    }

    public abstract class ImageAlgorithm : ViewModelBase, IImageAlgorithm
    {
        [Browsable(false)]
        public string Name => Regex.Replace(GetType().Name, @"(\B[A-Z])", " $1");

        [Browsable(false)]
        public abstract int Order { get; }

        public virtual void Process(Image<Gray, byte> image, out Image<Bgr, byte> annotatedImage,
            out List<object> data)
        {
            annotatedImage = image.Convert<Bgr, byte>();
            data = null;
        }
    }
}
