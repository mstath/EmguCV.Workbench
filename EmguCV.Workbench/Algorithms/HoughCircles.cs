using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Model;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    public class HoughCircles : ImageAlgorithm
    {
        public override int Order => 7;

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            var circles = image
                .Convert<Gray, byte>()
                .HoughCircles(
                    new Gray(_cannyThreshold),
                    new Gray(_accumulatorThreshold),
                    _dp,
                    _minDist,
                    _minRadius,
                    _maxRadius);
            data = new List<object>();
            foreach (var circle in circles[0])
            {
                annotatedImage.Draw(circle, new Bgr(Color.Red));
                data.Add(new Circle(circle));
            }
        }

        private byte _cannyThreshold = 255;
        [Category("Hough Circles")]
        [PropertyOrder(0)]
        [DisplayName(@"Canny Threshold")]
        [Description(@"The higher threshold of the two passed to Canny edge detector.")]
        public byte CannyThreshold
        {
            get { return _cannyThreshold; }
            set { Set(ref _cannyThreshold, value); }
        }

        private byte _accumulatorThreshold = 255;
        [Category("Hough Circles")]
        [PropertyOrder(1)]
        [DisplayName(@"Accumulator Threshold")]
        [Description(@"Accumulator threshold at the center detection stage.")]
        public byte AccumulatorThreshold
        {
            get { return _accumulatorThreshold; }
            set { Set(ref _accumulatorThreshold, value); }
        }

        private double _dp = 127;
        [Category("Hough Circles")]
        [PropertyOrder(2)]
        [DisplayName(@"dp")]
        [Description(@"Resolution of the accumulator used to detect centers of the circles.")]
        public double Dp
        {
            get { return _dp; }
            set { Set(ref _dp, value); }
        }

        private double _minDist = 1;
        [Category("Hough Circles")]
        [PropertyOrder(3)]
        [DisplayName(@"Min Distance")]
        [Description(@"Minimum distance between centers of the detected circles.")]
        public double MinDist
        {
            get { return _minDist; }
            set { Set(ref _minDist, value); }
        }

        private int _minRadius;
        [Category("Hough Circles")]
        [PropertyOrder(4)]
        [DisplayName(@"Min Radius")]
        [Description(@"Minimal radius of the circles to search for.")]
        public int MinRadius
        {
            get { return _minRadius; }
            set { Set(ref _minRadius, value); }
        }

        private int _maxRadius;
        [Category("Hough Circles")]
        [PropertyOrder(5)]
        [DisplayName(@"Max Radius")]
        [Description(@"Maximal radius of the circles to search for.")]
        public int MaxRadius
        {
            get { return _maxRadius; }
            set { Set(ref _maxRadius, value); }
        }
    }
}
