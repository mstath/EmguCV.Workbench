using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static Emgu.CV.Features2D.Features2DToolbox;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Enables testing of various feature detectors from the 2D Features framework.
    /// https://docs.opencv.org/master/d7/d66/tutorial_feature_detection.html
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageAlgorithm" />
    public class FeatureDetection : ImageAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            // get an instance of the selected detector
            using (var detector = GetDetector())
            {
                // detect the features
                var kps = detector.Detect(image);

                // draw the keypoints
                DrawKeypoints(
                    annotatedImage,
                    new VectorOfKeyPoint(kps),
                    annotatedImage,
                    new Bgr(_annoColor.Color()),
                    _keypointDrawType);

                // add keypoints to data colleciton
                data = new List<object>();
                data.AddRange(kps.Select(k => new KeyPoint(k)));
            }
        }

        /// <summary>
        /// Gets a new instance of the selected detector.
        /// </summary>
        /// <returns>A new feature detector.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception for unrecognized selection.</exception>
        private Feature2D GetDetector()
        {
            switch (_detectorType)
            {
                case DetectorType.AKAZE:
                    return new AKAZE();
                case DetectorType.Brisk:
                    return new Brisk();
                case DetectorType.FastDetector:
                    return new FastDetector();
                case DetectorType.GFTTDetector:
                    return new GFTTDetector();
                case DetectorType.KAZE:
                    return new KAZE();
                case DetectorType.MSERDetector:
                    return new MSERDetector();
                case DetectorType.ORBDetector:
                    return new ORBDetector();
                case DetectorType.SIFT:
                    return new SIFT();
                case DetectorType.StarDetector:
                    return new StarDetector();
                case DetectorType.SURF:
                    return new SURF(300);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private DetectorType _detectorType = DetectorType.AKAZE;
        [Category("Feature Detection")]
        [PropertyOrder(0)]
        [DisplayName(@"Detector Type")]
        public DetectorType DetectorType
        {
            get { return _detectorType; }
            set { Set(ref _detectorType, value); }
        }

        [Browsable(false)]
        public override int LineThick { get; set; }

        private KeypointDrawType _keypointDrawType = KeypointDrawType.DrawRichKeypoints;
        [Category("Annotations")]
        [PropertyOrder(111)]
        [DisplayName(@"Keypoint Draw Type")]
        public KeypointDrawType KeypointDrawType
        {
            get { return _keypointDrawType; }
            set { Set(ref _keypointDrawType, value); }
        }
    }
}
