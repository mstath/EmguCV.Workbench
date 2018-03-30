using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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

        bool ClearTemplate { get; set; }

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

        protected Color _annoColor = Colors.Red;
        [Category("Annotations")]
        [PropertyOrder(100)]
        [DisplayName(@"Annotation Color")]
        public virtual Color AnnoColor
        {
            get { return _annoColor; }
            set { Set(ref _annoColor, value); }
        }

        protected int _lineThick = 1;
        [Category("Annotations")]
        [PropertyOrder(110)]
        [DisplayName(@"Line Thickness")]
        public virtual int LineThick
        {
            get { return _lineThick; }
            set { Set(ref _lineThick, value.Clamp(1, int.MaxValue)); }
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

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            if (!_clearTemplate) return;

            Template = null;
            ClearTemplate = false;
        }

        protected bool _clearTemplate;
        [Category("Template")]
        [PropertyOrder(100)]
        [DisplayName(@"Clear Template")]
        public bool ClearTemplate
        {
            get { return _clearTemplate; }
            set { Set(ref _clearTemplate, value); }
        }
    }
}
