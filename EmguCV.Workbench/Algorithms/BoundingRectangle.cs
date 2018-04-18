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
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Create bounding (rotated) rectangles around contours.
    /// https://docs.opencv.org/master/da/d0c/tutorial_bounding_rects_circles.html
    /// https://docs.opencv.org/master/de/d62/tutorial_bounding_rotated_ellipses.html
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageAlgorithm" />
    [Export(typeof(IImageAlgorithm))]
    public class BoundingRectangle : ImageAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            using (var contours = new VectorOfVectorOfPoint())
            {
                // find the contours for the image
                CvInvoke.FindContours(
                    image.Convert<Gray, byte>(),
                    contours,
                    null,
                    RetrType.List,
                    ChainApproxMethod.ChainApproxSimple);

                data = new List<object>();

                // bound the contours
                BoundContours(contours, ref annotatedImage, ref data);
            }
        }

        /// <summary>
        /// Bound the contours.
        /// </summary>
        /// <param name="contours">The contours.</param>
        /// <param name="annotatedImage">The image with bound rectangles.</param>
        /// <param name="data">The raw data for rectangles.</param>
        private void BoundContours(VectorOfVectorOfPoint contours, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            // optionally show the contours
            if (_showContours)
                annotatedImage.DrawPolyline(contours.ToArrayOfArray(), false, new Bgr(_contourColor.Color()), _lineThick);

            // bound object for each contour or all contours
            if (!_foreachContour)
            {
                var points = contours.ToArrayOfArray().SelectMany(p => p).ToArray();
                FindRect(new VectorOfPoint(points), ref annotatedImage, ref data);
            }
            else
                foreach (var contour in contours.ToArrayOfArray())
                    FindRect(new VectorOfPoint(contour), ref annotatedImage, ref data);
        }

        /// <summary>
        /// Finds the bounding rectangle for the contour.
        /// </summary>
        /// <param name="contour">The contour.</param>
        /// <param name="annotatedImage">The image with bound rectangles.</param>
        /// <param name="data">The raw data for rectangles.</param>
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

        /// <summary>
        /// Draws the bound rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="annotatedImage">The image with bound rectangles.</param>
        /// <param name="data">The raw data for rectangles.</param>
        private void SetRect(Rectangle rect, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            annotatedImage.Draw(rect, new Bgr(_annoColor.Color()), _lineThick);
            data.Add(new Box(rect));
        }

        /// <summary>
        /// Draws the bound rotated rectangle.
        /// </summary>
        /// <param name="rect">The rotated rectangle.</param>
        /// <param name="annotatedImage">The image with bound rectangles.</param>
        /// <param name="data">The raw data for rectangles.</param>
        private void SetRect(RotatedRect rect, ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            var vertices = rect.GetVertices().Select(Point.Round).ToArray();
            annotatedImage.DrawPolyline(vertices, true, new Bgr(_annoColor.Color()), _lineThick);
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
