using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Model;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    public class HoughLines : ImageAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            var lines = image
                .Convert<Gray, byte>()
                .HoughLines(
                    _cannyThreshold,
                    _cannyThresholdLinking,
                    _rhoResolution,
                    _thetaResolution*Math.PI/180,
                    _threshold,
                    _minLineWidth,
                    _gapBetweenLines);
            data = new List<object>();
            foreach (var line in lines[0])
            {
                annotatedImage.Draw(line, new Bgr(Color.Red), 1);
                data.Add(new Segment(line));
            }
        }

        private double _cannyThreshold = 255;
        [Category("Hough Lines")]
        [PropertyOrder(0)]
        [DisplayName(@"Canny Threshold")]
        [Description(@"The threshhold to find initial segments of strong edges.")]
        public double CannyThreshold
        {
            get { return _cannyThreshold; }
            set { Set(ref _cannyThreshold, value); }
        }

        private double _cannyThresholdLinking = 255;
        [Category("Hough Lines")]
        [PropertyOrder(1)]
        [DisplayName(@"Canny Threshold Linking")]
        [Description(@"The threshold used for edge linking.")]
        public double CannyThresholdLinking
        {
            get { return _cannyThresholdLinking; }
            set { Set(ref _cannyThresholdLinking, value); }
        }

        private double _rhoResolution = 1;
        [Category("Hough Lines")]
        [PropertyOrder(2)]
        [DisplayName(@"Rho Resolution")]
        [Description(@"Distance resolution in pixel-related units.")]
        public double RhoResolution
        {
            get { return _rhoResolution; }
            set { Set(ref _rhoResolution, value); }
        }

        private double _thetaResolution = 1;
        [Category("Hough Lines")]
        [PropertyOrder(3)]
        [DisplayName(@"Theta Resolution")]
        [Description(@"Angle resolution measured in degrees.")]
        public double ThetaResolution
        {
            get { return _thetaResolution; }
            set { Set(ref _thetaResolution, value); }
        }

        private int _threshold = 100;
        [Category("Hough Lines")]
        [PropertyOrder(4)]
        [DisplayName(@"Threshold")]
        [Description(@"A line is returned by the function if the corresponding accumulator value is greater than threshold.")]
        public int Threshold
        {
            get { return _threshold; }
            set { Set(ref _threshold, value); }
        }

        private double _minLineWidth;
        [Category("Hough Lines")]
        [PropertyOrder(5)]
        [DisplayName(@"Min Line Width")]
        [Description(@"Minimum width of a line.")]
        public double MinLineWidth
        {
            get { return _minLineWidth; }
            set { Set(ref _minLineWidth, value); }
        }

        private double _gapBetweenLines = 70;
        [Category("Hough Lines")]
        [PropertyOrder(6)]
        [DisplayName(@"Gap Between Lines")]
        [Description(@"Minimum gap between lines.")]
        public double GapBetweenLines
        {
            get { return _gapBetweenLines; }
            set { Set(ref _gapBetweenLines, value); }
        }
    }
}
