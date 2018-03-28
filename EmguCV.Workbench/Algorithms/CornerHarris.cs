﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    public class CornerHarris : ImageAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            var corners = new Image<Gray, float>(image.Size);

            CvInvoke.CornerHarris(
                image.Convert<Gray, byte>(),
                corners,
                _blockSize,
                _apertureSize);

            CvInvoke.Normalize(corners, corners);

            if (_viewCorners)
            {
                annotatedImage = corners.Convert<Bgr, byte>();
                return;
            }

            var gray = corners.Convert<Gray, byte>();
            data = new List<object>();

            for (var j = 0; j < gray.Rows; j++)
            {
                for (var i = 0; i < gray.Cols; i++)
                {
                    if (!(gray[j, i].Intensity > _threshold))
                        continue;

                    var circle = new CircleF(new PointF(i, j), 1);
                    annotatedImage.Draw(circle, new Bgr(_annoColor.Color()), _lineThick);
                    data.Add(new Circle(circle));
                }
            }
        }

        private int _blockSize = 2;
        [Category("Corner Harris")]
        [PropertyOrder(0)]
        [DisplayName(@"Block Size")]
        [Description(@"Neighborhood size.")]
        public int BlockSize
        {
            get { return _blockSize; }
            set { Set(ref _blockSize, value.Clamp(1, int.MaxValue)); }
        }

        private int _apertureSize = 3;
        [Category("Corner Harris")]
        [PropertyOrder(1)]
        [DisplayName(@"Aperture Size")]
        [Description(@"Aperture parameter for Sobel operator.")]
        public int ApertureSize
        {
            get { return _apertureSize; }
            set { Set(ref _apertureSize, value.ClampOdd(_apertureSize, 1, 31)); }
        }

        private byte _threshold = 127;
        [Category("Corner Harris")]
        [PropertyOrder(2)]
        [DisplayName(@"Threshold")]
        [Description(@"The threshold which represents a corner.")]
        public byte Threshold
        {
            get { return _threshold; }
            set { Set(ref _threshold, value); }
        }

        private bool _viewCorners;
        [Category("Corner Harris")]
        [PropertyOrder(3)]
        [DisplayName(@"View Corners")]
        [Description(@"View the corner response image.")]
        public bool ViewCorners
        {
            get { return _viewCorners; }
            set { Set(ref _viewCorners, value); }
        }
    }
}