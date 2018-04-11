using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.VideoSurveillance;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Background subtraction (BS) is a common and widely used technique for generating a foreground mask 
    /// (namely, a binary image containing the pixels belonging to moving objects in the scene) by using static cameras.
    /// https://docs.opencv.org/master/d1/dc5/tutorial_background_subtraction.html
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageAlgorithm" />
    public class BackgroundSubtraction : ImageAlgorithm
    {
        private readonly BackgroundSubtractor _subtractor = new BackgroundSubtractorMOG2();
        private readonly MotionHistory _motionHistory = new MotionHistory(1.0, 0.05, 0.5);
        private readonly Mat _foregroundMask = new Mat();
        private readonly Mat _segMask = new Mat();

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            // apply the current image to the foreground mask
            _subtractor.Apply(image, _foregroundMask);

            // add the mask to the motion history
            _motionHistory.Update(_foregroundMask);

            // selector for desired image to be viewed
            switch (_bgSubImageType)
            {
                case BgSubImageType.FgMask:
                    annotatedImage = _foregroundMask.ToImage<Bgr, byte>();
                    break;
                case BgSubImageType.Background:
                    _subtractor.GetBackgroundImage(annotatedImage);
                    break;
                default:
                    data = new List<object>();
                    DrawMotion(ref annotatedImage, ref data);
                    break;
            }
        }

        /// <summary>
        /// Draws bounding rectangles around objects in motion.
        /// </summary>
        /// <param name="annotatedImage">The image with the bounded rectangles.</param>
        /// <param name="data">The raw data for the bounding rectangles.</param>
        private void DrawMotion(ref Image<Bgr, byte> annotatedImage, ref List<object> data)
        {
            using (var boundingRect = new VectorOfRect())
            {
                // get the motion components (and their bounding rectangles)
                _motionHistory.GetMotionComponents(_segMask, boundingRect);
                var rects = boundingRect.ToArray();

                // draw the rectangles and populate the data
                foreach (var rect in rects.Where(r => r.Width * r.Height >= _minArea))
                {
                    annotatedImage.Draw(rect, new Bgr(_annoColor.Color()), _lineThick);
                    data.Add(new Box(rect));
                }
            }
        }

        private BgSubImageType _bgSubImageType = BgSubImageType.Motion;
        [Category("Background Subtraction")]
        [PropertyOrder(0)]
        [DisplayName(@"Image Type")]
        [Description(@"Select which image to view.")]
        public BgSubImageType BgSubImageType
        {
            get { return _bgSubImageType; }
            set { Set(ref _bgSubImageType, value); }
        }

        private double _minArea = 10000;
        [Category("Background Subtraction")]
        [PropertyOrder(1)]
        [DisplayName(@"Min Area")]
        [Description(@"The min area by which to consider a motion object.")]
        public double MinArea
        {
            get { return _minArea; }
            set { Set(ref _minArea, value); }
        }
    }
}
