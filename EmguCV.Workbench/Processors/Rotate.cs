﻿using System.ComponentModel;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Rotate : ImageProcessor
    {
        public Rotate()
        {
            Angle = 0;
            Crop = false;
        }

        private double _angle;
        [Category("Rotate")]
        [PropertyOrder(0)]
        [DisplayName(@"Angle")]
        [Description(@"The angle of rotation in degrees.")]
        [DefaultValue(0.0)]
        public double Angle
        {
            get { return _angle; }
            set { Set(ref _angle, value.Clamp(-360, 360)); }
        }

        private bool _crop;
        [Category("Rotate")]
        [PropertyOrder(1)]
        [DisplayName(@"Crop")]
        [Description(@"If set to true the image is cropped to its original size. If set to false all rotation information is preserved.")]
        [DefaultValue(false)]
        public bool Crop
        {
            get { return _crop; }
            set { Set(ref _crop, value); }
        }

        public override void Process(ref Image<Gray, byte> image)
        {
            image = image.Rotate(_angle, new Gray(), _crop);
        }
    }
}