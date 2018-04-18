using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Color = System.Windows.Media.Color;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Use Cascade Classifiers to detect faces.
    /// https://docs.opencv.org/master/db/d28/tutorial_cascade_classifier.html
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageAlgorithm" />
    [Export(typeof(IImageAlgorithm))]
    public class FaceDetection : ImageAlgorithm
    {
        private readonly CascadeClassifier _face = new CascadeClassifier(@"Resources\haarcascade_frontalface_default.xml");
        private readonly CascadeClassifier _eyes = new CascadeClassifier(@"Resources\haarcascade_eye.xml");

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            // gray scale the image
            var gray = image.Convert<Gray, byte>();

            // find bounding rectangle for face
            var faceRects = _face.DetectMultiScale(gray, _faceScaleFactor, _faceMinNeighbors);

            data = new List<object>();

            // foreach face rect
            foreach (var faceRect in faceRects)
            {
                // get a bounding circle for each face rectangle
                var faceCircle = CircleForRectange(faceRect);
                
                // draw the face circle and add to data object
                annotatedImage.Draw(faceCircle, new Bgr(_faceColor.Color()), _lineThick);
                data.Add(new Circle(faceCircle));

                // find bounding rectangles for eyes
                var eyesRects = _eyes.DetectMultiScale(gray, _eyesScaleFactor, _eyesMinNeighbors);

                // foreach eye rect
                foreach (var eyeRect in eyesRects)
                {
                    // get a bounding circle for each eye
                    var eyeCircle = CircleForRectange(eyeRect);

                    // draw each eye circle and add to data object
                    annotatedImage.Draw(eyeCircle, new Bgr(_eyeColor.Color()), _lineThick);
                    data.Add(new Circle(eyeCircle));
                }
            }
        }

        /// <summary>
        /// Get bounding circle for a given rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>The bounding circle with diameter matching width of rectangle.</returns>
        private CircleF CircleForRectange(Rectangle rect)
        {
            // calculate the center
            var center = new PointF
            {
                X = rect.X + rect.Width/2,
                Y = rect.Y + rect.Height/2
            };

            // calculate the radius
            var radius = rect.Width/2;

            // return the new circle
            return new CircleF(center, radius);
        }

        private double _faceScaleFactor = 1.1;
        [Category("Face Detection")]
        [PropertyOrder(0)]
        [DisplayName(@"Face Scale Factor")]
        [Description(@"The factor by which the search window is scaled between the subsequent scans.")]
        public double FaceScaleFactor
        {
            get { return _faceScaleFactor; }
            set { Set(ref _faceScaleFactor, value); }
        }

        private int _faceMinNeighbors = 10;
        [Category("Face Detection")]
        [PropertyOrder(1)]
        [DisplayName(@"Face Min Neighbors")]
        [Description(@"Minimum number (minus 1) of neighbor rectangles that makes up an object.")]
        public int FaceMinNeighbors
        {
            get { return _faceMinNeighbors; }
            set { Set(ref _faceMinNeighbors, value); }
        }

        private double _eyesScaleFactor = 1.1;
        [Category("Face Detection")]
        [PropertyOrder(2)]
        [DisplayName(@"Eyes Scale Factor")]
        [Description(@"The factor by which the search window is scaled between the subsequent scans.")]
        public double EyesScaleFactor
        {
            get { return _eyesScaleFactor; }
            set { Set(ref _eyesScaleFactor, value); }
        }

        private int _eyesMinNeighbors = 10;
        [Category("Face Detection")]
        [PropertyOrder(3)]
        [DisplayName(@"Eyes Min Neighbors")]
        [Description(@"Minimum number (minus 1) of neighbor rectangles that makes up an object.")]
        public int EyesMinNeighbors
        {
            get { return _eyesMinNeighbors; }
            set { Set(ref _eyesMinNeighbors, value); }
        }

        [Browsable(false)]
        public override Color AnnoColor { get; set; }

        private Color _faceColor = Colors.DodgerBlue;
        [Category("Annotations")]
        [PropertyOrder(101)]
        [DisplayName(@"Face Color")]
        public Color FaceColor
        {
            get { return _faceColor; }
            set { Set(ref _faceColor, value); }
        }

        private Color _eyeColor = Colors.Red;
        [Category("Annotations")]
        [PropertyOrder(102)]
        [DisplayName(@"Eye Color")]
        public Color EyeColor
        {
            get { return _eyeColor; }
            set { Set(ref _eyeColor, value); }
        }
    }
}
