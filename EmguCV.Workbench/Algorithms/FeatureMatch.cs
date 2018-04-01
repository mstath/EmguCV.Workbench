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

                if (Template == null)
                    return;

                // get features from object
                detector.DetectAndCompute(
                    Template.Convert<Gray, byte>(),
                    null,
                    modelKeyPoints,
                    modelDescriptors,
                    false);

                // match
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

                    mask.SetTo(new MCvScalar(255));
                    Features2DToolbox.VoteForUniqueness(matches, 0.8, mask);

                    var nonZeroCount = CvInvoke.CountNonZero(mask);
                    if (nonZeroCount >= 4)
                    {
                        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, imageKeyPoints,
                            matches, mask, 1.5, 20);

                        if (nonZeroCount >= 4)
                            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                                imageKeyPoints, matches, mask, 2);
                    }

                    if (homography == null) return;

                    var rect = new Rectangle(Point.Empty, Template.Size);
                    var pts = new[]
                    {
                        new PointF(rect.Left, rect.Bottom),
                        new PointF(rect.Right, rect.Bottom),
                        new PointF(rect.Right, rect.Top),
                        new PointF(rect.Left, rect.Top)
                    };
                    pts = CvInvoke.PerspectiveTransform(pts, homography);
                    var rotRect = CvInvoke.MinAreaRect(pts);

                    annotatedImage.Draw(rotRect, new Bgr(_annoColor.Color()), _lineThick);
                    data = new List<object> { new RotatedBox(rotRect) };
                }
            }
        }

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
