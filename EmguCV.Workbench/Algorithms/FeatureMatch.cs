using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Uses feature detection and matching to track selected objects.
    /// https://docs.opencv.org/master/d7/dff/tutorial_feature_homography.html
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageTemplateAlgorithm" />
    public class FeatureMatch : ImageTemplateAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            using (var detector = GetDetector())
            using (var modelKeyPoints = new VectorOfKeyPoint())
            using (var imageKeyPoints = new VectorOfKeyPoint())
            using (var modelDescriptors = new Mat())
            using (var imageDescriptors = new Mat())
            using (var flannMatcher = new FlannBasedMatcher(GetIndexParams(), new SearchParams()))
            using (var bfMatcher = new BFMatcher(_distanceType))
            using (var matches = new VectorOfVectorOfDMatch())
            {
                // get features from image
                detector.DetectAndCompute(
                    image.Convert<Gray, byte>(),
                    null,
                    imageKeyPoints,
                    imageDescriptors,
                    false);

                // optionally view image keypoints and return
                if (_showKeypoints)
                {
                    Features2DToolbox.DrawKeypoints(
                        annotatedImage,
                        imageKeyPoints,
                        annotatedImage,
                        new Bgr(_annoColor.Color()),
                        Features2DToolbox.KeypointDrawType.DrawRichKeypoints);
                    data = new List<object>();
                    data.AddRange(imageKeyPoints.ToArray().Select(k => new KeyPoint(k)));
                    return;
                }

                // do not proceed if there is no template
                if (Template == null)
                    return;

                // get features from object
                detector.DetectAndCompute(
                    Template.Convert<Gray, byte>(),
                    null,
                    modelKeyPoints,
                    modelDescriptors,
                    false);

                // perform match with selected matcher
                if (_matcherType == MatcherType.Flann)
                {
                    flannMatcher.Add(modelDescriptors);
                    flannMatcher.KnnMatch(
                        imageDescriptors,
                        matches,
                        2,
                        null);
                }
                else
                {
                    bfMatcher.Add(modelDescriptors);
                    bfMatcher.KnnMatch(
                        imageDescriptors,
                        matches,
                        2,
                        null);
                }
                
                // find homography
                using (var mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1))
                {
                    Mat homography = null;

                    // filter for unique matches
                    mask.SetTo(new MCvScalar(255));
                    Features2DToolbox.VoteForUniqueness(matches, 0.8, mask);

                    // if 4 or more patches continue
                    var nonZeroCount = CvInvoke.CountNonZero(mask);
                    if (nonZeroCount >= 4)
                    {
                        // filter for majority scale and rotation
                        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, imageKeyPoints,
                            matches, mask, 1.5, 20);

                        // if 4 or more patches continue
                        if (nonZeroCount >= 4)
                            // get the homography
                            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                                imageKeyPoints, matches, mask, 2);
                    }

                    // if no homography, return
                    if (homography == null) return;

                    // initialize a rectangle of the template size
                    var rect = new Rectangle(Point.Empty, Template.Size);

                    // create points array for the vertices of the template
                    var pts = new[]
                    {
                        new PointF(rect.Left, rect.Bottom),
                        new PointF(rect.Right, rect.Bottom),
                        new PointF(rect.Right, rect.Top),
                        new PointF(rect.Left, rect.Top)
                    };

                    // transform the perspective of the points array based on the homography
                    // and get a rotated rectangle for the homography
                    pts = CvInvoke.PerspectiveTransform(pts, homography);
                    var rotRect = CvInvoke.MinAreaRect(pts);

                    // annotate the image and return the rotated rectangle model
                    annotatedImage.Draw(rotRect, new Bgr(_annoColor.Color()), _lineThick);
                    data = new List<object> { new RotatedBox(rotRect) };
                }
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
                case MatcherDetectorType.KAZE:
                    return new KAZE();
                case MatcherDetectorType.SIFT:
                    return new SIFT();
                case MatcherDetectorType.SURF:
                    return new SURF(300);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets an new instance of the selected index parameters for the Flann Based Matcher.
        /// </summary>
        /// <returns>A new index paremeter object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exception for unrecognized selection.</exception>
        private IIndexParams GetIndexParams()
        {
            switch (_indexParamsType)
            {
                case MatcherIndexParamsType.Autotuned:
                    return new AutotunedIndexParams();
                case MatcherIndexParamsType.Composite:
                    return new CompositeIndexParams();
                case MatcherIndexParamsType.HierarchicalClustering:
                    return new HierarchicalClusteringIndexParams();
                case MatcherIndexParamsType.KdTree:
                    return new KdTreeIndexParams();
                case MatcherIndexParamsType.KMeans:
                    return new KMeansIndexParams();
                case MatcherIndexParamsType.Linear:
                    return new LinearIndexParams();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private MatcherDetectorType _detectorType = MatcherDetectorType.SURF;
        [Category("Feature Match (selectable)")]
        [PropertyOrder(0)]
        [DisplayName(@"Detector Type")]
        public MatcherDetectorType DetectorType
        {
            get { return _detectorType; }
            set { Set(ref _detectorType, value); }
        }

        private MatcherType _matcherType = MatcherType.Flann;
        [Category("Feature Match (selectable)")]
        [PropertyOrder(1)]
        [DisplayName(@"Matcher Type")]
        public MatcherType MatcherType
        {
            get { return _matcherType; }
            set { Set(ref _matcherType, value); }
        }

        private MatcherIndexParamsType _indexParamsType = MatcherIndexParamsType.Autotuned;
        [Category("Feature Match (selectable)")]
        [PropertyOrder(2)]
        [DisplayName(@"Index Params Type")]
        [Description(@"Default index parameter types for Flann Based Matcher.")]
        public MatcherIndexParamsType IndexParamsType
        {
            get { return _indexParamsType; }
            set { Set(ref _indexParamsType, value); }
        }

        private DistanceType _distanceType = DistanceType.L2;
        [Category("Feature Match (selectable)")]
        [PropertyOrder(3)]
        [DisplayName(@"Distance Type")]
        [Description(@"Distance type for BF Matcher.")]
        public DistanceType DistanceType
        {
            get { return _distanceType; }
            set { Set(ref _distanceType, value); }
        }

        private bool _showKeypoints;
        [Category("Feature Match (selectable)")]
        [PropertyOrder(4)]
        [DisplayName(@"Show Image Keypoints")]
        public bool ShowKeypoints
        {
            get { return _showKeypoints; }
            set { Set(ref _showKeypoints, value); }
        }
    }
}
