﻿using System.ComponentModel;
using System.ComponentModel.Composition;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    /// <summary>
    /// Smooth for median of size x size neighborhood.
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Processors.ImageProcessor" />
    [Export(typeof(IImageProcessor))]
    public class SmoothMedian : ImageProcessor
    {
        private int _size = 1;
        [Category("Smooth Median")]
        [PropertyOrder(0)]
        [DisplayName(@"Size")]
        [Description(@"The size (width & height) of the window.")]
        public int Size
        {
            get { return _size; }
            set { Set(ref _size, value.ClampOdd(_size, 1, int.MaxValue)); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            image = image.SmoothMedian(_size);
        }
    }
}
