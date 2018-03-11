using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Morphology : ImageProcessor
    {
        public Morphology()
        {
            MorphOp = MorphOp.Erode;
            ElementShape = ElementShape.Rectangle;
            SizeWidth = 3;
            SizeHeight = 3;
            AnchorX = -1;
            AnchorY = -1;
            Iterations = 1;
        }

        private MorphOp _morphOp;
        [Category("Morphology")]
        [PropertyOrder(0)]
        [DisplayName(@"Morphology Operation")]
        [Description(@"Type of morphological operation.")]
        [DefaultValue(MorphOp.Erode)]
        public MorphOp MorphOp
        {
            get { return _morphOp; }
            set { Set(ref _morphOp, value); }
        }

        private ElementShape _elementShape;
        [Category("Morphology")]
        [PropertyOrder(1)]
        [DisplayName(@"Element Shape")]
        [Description(@"Element shape.")]
        [DefaultValue(ElementShape.Rectangle)]
        public ElementShape ElementShape
        {
            get { return _elementShape; }
            set { Set(ref _elementShape, value); }
        }

        private int _sizeWidth;
        [Category("Morphology")]
        [PropertyOrder(2)]
        [DisplayName(@"Size Width")]
        [Description(@"Size of the structuring element.")]
        [DefaultValue(3)]
        public int SizeWidth
        {
            get { return _sizeWidth; }
            set { Set(ref _sizeWidth, value.Clamp(1, int.MaxValue)); }
        }

        private int _sizeHeight;
        [Category("Morphology")]
        [PropertyOrder(3)]
        [DisplayName(@"Size Height")]
        [Description(@"Size of the structuring element.")]
        [DefaultValue(3)]
        public int SizeHeight
        {
            get { return _sizeHeight; }
            set { Set(ref _sizeHeight, value.Clamp(1, int.MaxValue)); }
        }

        private int _anchorX;
        [Category("Morphology")]
        [PropertyOrder(4)]
        [DisplayName(@"Anchor X")]
        [Description(@"Anchor position within the element. The value (-1, -1) means that the anchor is at the center.")]
        [DefaultValue(-1)]
        public int AnchorX
        {
            get { return _anchorX; }
            set { Set(ref _anchorX, value.Clamp(-1, _sizeWidth - 1)); }
        }

        private int _anchorY;
        [Category("Morphology")]
        [PropertyOrder(5)]
        [DisplayName(@"Anchor Y")]
        [Description(@"Anchor position within the element. The value (-1, -1) means that the anchor is at the center.")]
        [DefaultValue(-1)]
        public int AnchorY
        {
            get { return _anchorY; }
            set { Set(ref _anchorY, value.Clamp(-1, _sizeHeight - 1)); }
        }

        private int _iterations;
        [Category("Morphology")]
        [PropertyOrder(6)]
        [DisplayName(@"Iterations")]
        [Description(@"Number of times erosion and dilation are applied.")]
        [DefaultValue(1)]
        public int Iterations
        {
            get { return _iterations; }
            set { Set(ref _iterations, value.Clamp(0, int.MaxValue)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            var element = CvInvoke.GetStructuringElement(_elementShape, new Size(_sizeWidth, _sizeHeight),
                new Point(_anchorX, _anchorY));
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
