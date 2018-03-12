﻿using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class SmoothGaussian : ImageProcessor
    {
        public SmoothGaussian()
        {
            KernelSize = 1;
        }

        private int _kernelSize;
        [Category("Smooth Gaussian")]
        [PropertyOrder(0)]
        [DisplayName(@"Kernel Size")]
        [Description(@"The size of the Gaussian kernel.")]
        [DefaultValue(1)]
        public int KernelSize
        {
            get { return _kernelSize; }
            set { Set(ref _kernelSize, value.ClampOdd(_kernelSize, 1, 639)); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.SmoothGaussian(_kernelSize);
        }
    }
}