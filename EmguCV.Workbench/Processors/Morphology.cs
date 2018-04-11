using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Perform advanced morphological transformations using erosion and dilation as basic operations.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    public class Morphology : ImageProcessor
    {
        private MorphOp _morphOp = MorphOp.Erode;
        [Category("Morphology")]
        [PropertyOrder(0)]
        [DisplayName(@"Morphology Operation")]
        [Description(@"Type of morphological operation.")]
        public MorphOp MorphOp
        {
            get { return _morphOp; }
            set { Set(ref _morphOp, value); }
        }

        private ElementShape _elementShape = ElementShape.Rectangle;
        [Category("Morphology")]
        [PropertyOrder(1)]
        [DisplayName(@"Element Shape")]
        [Description(@"Element shape.")]
        public ElementShape ElementShape
        {
            get { return _elementShape; }
            set { Set(ref _elementShape, value); }
        }

        private int _sizeWidth = 3;
        [Category("Morphology")]
        [PropertyOrder(2)]
        [DisplayName(@"Size Width")]
        [Description(@"Size of the structuring element.")]
        public int SizeWidth
        {
            get { return _sizeWidth; }
            set { Set(ref _sizeWidth, value.Clamp(1, int.MaxValue)); }
        }

        private int _sizeHeight = 3;
        [Category("Morphology")]
        [PropertyOrder(3)]
        [DisplayName(@"Size Height")]
        [Description(@"Size of the structuring element.")]
        public int SizeHeight
        {
            get { return _sizeHeight; }
            set { Set(ref _sizeHeight, value.Clamp(1, int.MaxValue)); }
        }

        private int _anchorX = -1;
        [Category("Morphology")]
        [PropertyOrder(4)]
        [DisplayName(@"Anchor X")]
        [Description(@"Anchor position within the element. The value (-1, -1) means that the anchor is at the center.")]
        public int AnchorX
        {
            get { return _anchorX; }
            set { Set(ref _anchorX, value.Clamp(-1, _sizeWidth - 1)); }
        }

        private int _anchorY = -1;
        [Category("Morphology")]
        [PropertyOrder(5)]
        [DisplayName(@"Anchor Y")]
        [Description(@"Anchor position within the element. The value (-1, -1) means that the anchor is at the center.")]
        public int AnchorY
        {
            get { return _anchorY; }
            set { Set(ref _anchorY, value.Clamp(-1, _sizeHeight - 1)); }
        }

        private int _iterations = 1;
        [Category("Morphology")]
        [PropertyOrder(6)]
        [DisplayName(@"Iterations")]
        [Description(@"Number of times erosion and dilation are applied.")]
        public int Iterations
        {
            get { return _iterations; }
            set { Set(ref _iterations, value.Clamp(0, int.MaxValue)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            // get the structuring element
            var element = CvInvoke.GetStructuringElement(_elementShape, new Size(_sizeWidth, _sizeHeight),
                new Point(_anchorX, _anchorY));
            
            // apply morphology
            image = image.MorphologyEx(
                _morphOp,
                element,
                new Point(_anchorX, _anchorY),
                _iterations,
                BorderType.Default,
                new MCvScalar());
        }
    }
}
