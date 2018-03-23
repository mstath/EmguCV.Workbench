using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCV.Workbench.Model;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    public class ConvexHull : ImageAlgorithm
    {
        public override int Order => 11;

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(
                    image.Convert<Gray, byte>(),
                    contours,
                    null,
                    RetrType.List,
                    ChainApproxMethod.ChainApproxSimple);

                if (_showContours)
                    annotatedImage.DrawPolyline(contours.ToArrayOfArray(), false, new Bgr(Color.LimeGreen));

                var points = contours
                    .ToArrayOfArray()
                    .SelectMany(c => c)
                    .Select(p => new PointF(p.X, p.Y))
                    .ToArray();

                var convexHull = CvInvoke.ConvexHull(points);

                annotatedImage.DrawPolyline(
                    convexHull.Select(Point.Round).ToArray(),
                    true, 
                    new Bgr(Color.Red));

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
    }
}
