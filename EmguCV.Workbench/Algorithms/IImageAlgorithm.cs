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

        void Process(ref Image<Bgr, byte> image, out List<object> data);
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

        [Browsable(false)]
        public abstract int Order { get; }

        public virtual void Process(ref Image<Bgr, byte> image, out List<object> data)
        {
            data = null;
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
