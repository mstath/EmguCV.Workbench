using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    public class BoundingRectangle : ImageAlgorithm
    {
        public override int Order => 12;

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

                data = new List<object>();

                BoundContours(contours, ref annotatedImage, ref data);
            }
        }

        private void BoundContours(VectorOfVectorOfPoint contours, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            if (_showContours)
                annotatedImage.DrawPolyline(contours.ToArrayOfArray(), false, new Bgr(Color.LimeGreen));

            if (!_foreachContour)
            {
                var points = contours.ToArrayOfArray().SelectMany(p => p).ToArray();
                FindRect(new VectorOfPoint(points), ref annotatedImage, ref data);
            }
            else
                foreach (var contour in contours.ToArrayOfArray())
                    FindRect(new VectorOfPoint(contour), ref annotatedImage, ref data);
        }

        private void FindRect(VectorOfPoint contour, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            switch (_rectType)
            {
                case BoundingRectType.Upright:
                    var rect = CvInvoke.BoundingRectangle(contour);
                    SetRect(rect, ref annotatedImage, ref data);
                    break;

                case BoundingRectType.Rotated:
                    var rotRect = CvInvoke.MinAreaRect(contour);
                    SetRect(rotRect, ref annotatedImage, ref data);
                    break;
            }
        }

        private void SetRect(Rectangle rect, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            annotatedImage.Draw(rect, new Bgr(Color.Red));
            data.Add(new Box(rect));
        }

        private void SetRect(RotatedRect rect, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            var vertices = rect.GetVertices().Select(Point.Round).ToArray();
            annotatedImage.DrawPolyline(vertices, true, new Bgr(Color.Red));
            data.Add(new RotatedBox(rect));
        }

        private BoundingRectType _rectType = BoundingRectType.Upright;
        [Category("Bounding Rectangle")]
        [PropertyOrder(0)]
        [DisplayName(@"Rectangle Type")]
        [Description(@"Upright or rotated rectangle.")]
        public BoundingRectType RectType
        {
            get { return _rectType; }
            set { Set(ref _rectType, value); }
        }

        private bool _foreachContour;
        [Category("Bounding Rectangle")]
        [PropertyOrder(1)]
        [DisplayName(@"Foreach Contour")]
        [Description(@"Unchecked: box around all contours; checked: box around each contour.")]
        public bool ForeachContour
        {
            get { return _foreachContour; }
            set { Set(ref _foreachContour, value); }
        }

        private bool _showContours;
        [Category("Bounding Rectangle")]
        [PropertyOrder(2)]
        [DisplayName(@"Show Contours")]
        public bool ShowContours
        {
            get { return _showContours; }
            set { Set(ref _showContours, value); }
        }
    }
}
