using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCV.Workbench.Model;

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

                var points = contours
                    .ToArrayOfArray()
                    .SelectMany(c => c)
                    .Select(p => new PointF(p.X, p.Y))
                    .ToArray();

                foreach (var point in points)
                    annotatedImage.Draw(new CircleF {Center = point, Radius = 1}, new Bgr(Color.Lime));

                var convexHull = CvInvoke.ConvexHull(points);

                annotatedImage.DrawPolyline(
                    convexHull.Select(Point.Round).ToArray(),
                    true, 
                    new Bgr(Color.Red));

                data = new List<object> {new Contour(convexHull)};
            }
        }
    }
}
