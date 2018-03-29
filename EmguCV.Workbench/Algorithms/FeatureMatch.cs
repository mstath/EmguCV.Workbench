using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
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
using Color = System.Windows.Media.Color;

namespace EmguCV.Workbench.Algorithms
{
    public class FeatureMatch : ImageTemplateAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            if (Template == null)
                return;

            using (var detector = new SURF(300))
            using (var modelKeyPoints = new VectorOfKeyPoint())
            using (var imageKeyPoints = new VectorOfKeyPoint())
            using (var modelDescriptors = new Mat())
            using (var imageDescriptors = new Mat())
            using (var matcher = new FlannBasedMatcher(new AutotunedIndexParams(), new SearchParams()))
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
                        new Bgr(_keypointColor.Color()),
                        Features2DToolbox.KeypointDrawType.DrawRichKeypoints);
                    data = new List<object>();
                    data.AddRange(imageKeyPoints.ToArray().Select(k => new KeyPoint(k)));
                    return;
                }

                // get features from object
                detector.DetectAndCompute(
                    Template.Convert<Gray, byte>(),
                    null,
                    modelKeyPoints,
                    modelDescriptors,
                    false);

                // match
                matcher.Add(modelDescriptors);
                matcher.KnnMatch(
                    imageDescriptors,
                    matches,
                    2,
                    null);

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
                    data = new List<object> {new RotatedBox(rotRect)};
                }
            }
        }

        private bool _showKeypoints;
        [Category("Feature Match (selectable)")]
        [PropertyOrder(0)]
        [DisplayName(@"Show Image Keypoints")]
        public bool ShowKeypoints
        {
            get { return _showKeypoints; }
            set { Set(ref _showKeypoints, value); }
        }

        private Color _keypointColor = Colors.LimeGreen;
        [Category("Annotations")]
        [PropertyOrder(101)]
        [DisplayName(@"Keypoint Color")]
        public Color KeypointColor
        {
            get { return _keypointColor; }
            set { Set(ref _keypointColor, value); }
        }
    }
}
