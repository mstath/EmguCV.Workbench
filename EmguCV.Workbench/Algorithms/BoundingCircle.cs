using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    public class BoundingCircle : ImageAlgorithm
    {
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
                annotatedImage.DrawPolyline(contours.ToArrayOfArray(), false, new Bgr(_contourColor.Color()), _lineThick);

            if (!_foreachContour)
            {
                var points = contours.ToArrayOfArray().SelectMany(p => p).ToArray();
                FindCircle(new VectorOfPoint(points), ref annotatedImage, ref data);
            }
            else
                foreach (var contour in contours.ToArrayOfArray())
                    FindCircle(new VectorOfPoint(contour), ref annotatedImage, ref data);
        }

        private void FindCircle(VectorOfPoint contour, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            switch (_circleType)
            {
                case BoundingCircleType.Circle:
                    var circle = CvInvoke.MinEnclosingCircle(contour);
                    SetCircle(circle, ref annotatedImage, ref data);
                    break;

                case BoundingCircleType.Ellipse:
                    var rotRect = CvInvoke.MinAreaRect(contour);
                    SetEllipse(rotRect, ref annotatedImage, ref data);
                    break;
            }
        }

        private void SetCircle(CircleF circle, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            annotatedImage.Draw(circle, new Bgr(_contourColor.Color()), _lineThick);
            data.Add(new Circle(circle));
        }

        private void SetEllipse(RotatedRect rect, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            annotatedImage.Draw(new Ellipse(rect), new Bgr(_contourColor.Color()), _lineThick);
            data.Add(new RotBoxEllipse(rect));
        }

        private BoundingCircleType _circleType = BoundingCircleType.Circle;
        [Category("Bounding Circle")]
        [PropertyOrder(0)]
        [DisplayName(@"Circle Type")]
        [Description(@"Circle or ellipse.")]
        public BoundingCircleType CircleType
        {
            get { return _circleType; }
            set { Set(ref _circleType, value); }
        }

        private bool _foreachContour;
        [Category("Bounding Circle")]
        [PropertyOrder(1)]
        [DisplayName(@"Foreach Contour")]
        [Description(@"Unchecked: circle around all contours; checked: circle around each contour.")]
        public bool ForeachContour
        {
            get { return _foreachContour; }
            set { Set(ref _foreachContour, value); }
        }

        private bool _showContours;
        [Category("Bounding Circle")]
        [PropertyOrder(2)]
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
