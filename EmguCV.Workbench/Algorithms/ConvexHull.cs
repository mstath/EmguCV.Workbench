using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCV.Workbench.Model;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Windows.Media;
using EmguCV.Workbench.Util;
using Color = System.Windows.Media.Color;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Draws a convex hull around a group of contours.
    /// https://docs.opencv.org/master/d7/d1d/tutorial_hull.html
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageAlgorithm" />
    [Export(typeof(IImageAlgorithm))]
    public class ConvexHull : ImageAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            using (var contours = new VectorOfVectorOfPoint())
            {
                // find the contours
                CvInvoke.FindContours(
                    image.Convert<Gray, byte>(),
                    contours,
                    null,
                    RetrType.List,
                    ChainApproxMethod.ChainApproxSimple);

                // optionally show the contours
                if (_showContours)
                    annotatedImage.DrawPolyline(contours.ToArrayOfArray(), false, new Bgr(_contourColor.Color()), _lineThick);

                // flatten the points of the contours into a single array
                var points = contours
                    .ToArrayOfArray()
                    .SelectMany(c => c)
                    .Select(p => new PointF(p.X, p.Y))
                    .ToArray();

                // get the convex hull
                var convexHull = CvInvoke.ConvexHull(points);

                // draw the convex hull
                annotatedImage.DrawPolyline(
                    convexHull.Select(Point.Round).ToArray(),
                    true, 
                    new Bgr(_annoColor.Color()),
                    _lineThick);

                // set the data as a single contour for the convex hull
                data = new List<object> {new Contour(convexHull)};
            }
        }

        private bool _showContours;
        [Category("Convex Hull")]
        [PropertyOrder(0)]
        [DisplayName(@"Show Contours")]
        public bool ShowContours
        {
            get { return _showContours; }
            set { Set(ref _showContours, value); }
        }

        private Color _contourColor = Colors.LimeGreen;
        [Category("Annotations")]
        [PropertyOrder(101)]
        [DisplayName(@"Contour Color")]
        public Color ContourColor
        {
            get { return _contourColor; }
            set { Set(ref _contourColor, value); }
        }
    }
}
