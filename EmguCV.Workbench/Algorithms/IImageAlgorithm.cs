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

        void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data);
    }

    public interface IImageTemplateAlgorithm : IImageAlgorithm
    {
        Image<Bgr, byte> Template { get; }

        void SetTemplate(Image<Bgr, byte> template);
    }

    public abstract class ImageAlgorithm : ViewModelBase, IImageAlgorithm
    {
        [Browsable(false)]
        public string Name => Regex.Replace(GetType().Name, @"(\B[A-Z])", " $1");

        public virtual void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            data = null;
            annotatedImage = image.Clone();
        }
    }

    public abstract class ImageTemplateAlgorithm : ImageAlgorithm, IImageTemplateAlgorithm
    {
        [Browsable(false)]
        public Image<Bgr, byte> Template { get; protected set; }

        public virtual void SetTemplate(Image<Bgr, byte> template)
        {
            Template = template;
        }
    }
}
