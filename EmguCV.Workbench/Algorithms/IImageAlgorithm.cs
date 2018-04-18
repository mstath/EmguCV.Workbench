using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Image algorithm interface.
    /// </summary>
    public interface IImageAlgorithm
    {
        /// <summary>
        /// Runs the image through the algorithm and produces an annotated image of the results
        /// as well as the raw data of the results.
        /// </summary>
        /// <param name="image">The image to run through the algorithm.</param>
        /// <param name="annotatedImage">The annotated image of the algorithm results.</param>
        /// <param name="data">The raw data of the algorithm results.</param>
        void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data);
    }

    /// <summary>
    /// Image algorithm interface supporting template image.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.IImageAlgorithm" />
    public interface IImageTemplateAlgorithm : IImageAlgorithm
    {
        /// <summary>
        /// Gets the template image for the algorithm.
        /// </summary>
        Image<Bgr, byte> Template { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to clear the template.
        /// </summary>
        bool ClearTemplate { get; set; }

        /// <summary>
        /// Sets the template image for the algorithm.
        /// </summary>
        /// <param name="template">The template image to set.</param>
        void SetTemplate(Image<Bgr, byte> template);
    }

    public abstract class ImageAlgorithm : ViewModelBase, IImageAlgorithm
    {
        public virtual void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            data = null;
            annotatedImage = image.Clone();
        }

        public override string ToString()
        {
            return GetType().Name;
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
