using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
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

                var points =
                    contours
                    .ToArrayOfArray()
                        .SelectMany(contour => contour)
                        .Select(p => new MKeyPoint {Point = new PointF(p.X, p.Y)})
                        .ToArray();

                var convexHull =
                    CvInvoke
                        .ConvexHull(
                            points
                                .Select(p => new PointF(p.Point.X, p.Point.Y))
                                .ToArray())
                        .Select(Point.Round)
                        .ToArray();

                Features2DToolbox.DrawKeypoints(
                    annotatedImage,
                    new VectorOfKeyPoint(points),
                    annotatedImage,
                    new Bgr(Color.LimeGreen));

                annotatedImage.DrawPolyline(convexHull, true, new Bgr(Color.Red));

                data = new List<object> {new Contour(convexHull)};
            }
        }
    }
}
