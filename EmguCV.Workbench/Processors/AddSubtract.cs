using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    class AddSubtract : ImageProcessor
    {
        private byte _color;
        [Category("Add Subtract")]
        [PropertyOrder(0)]
        [DisplayName(@"Color")]
        [Description(@"The color value to be added/subtracted to the image.")]
        public byte Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        private bool _subtract;
        [Category("Add Subtract")]
        [PropertyOrder(1)]
        [DisplayName(@"Subtract")]
        [Description(@"Unchecked: value added to image.  Checked: value subtracted from image.")]
        public bool Subtract
        {
            get { return _subtract; }
            set { Set(ref _subtract, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = !_subtract ? image.Add(new Gray(_color)) : image.Sub(new Gray(_color));
        }
    }
}
