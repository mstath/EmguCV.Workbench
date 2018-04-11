using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    /// <summary>
    /// Match a slected template against the image.
    /// https://docs.opencv.org/master/de/da9/tutorial_template_matching.html
    /// </summary>
    /// <seealso cref="EmguCV.Workbench.Algorithms.ImageTemplateAlgorithm" />
    public class TemplateMatch : ImageTemplateAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            // return if no template
            if (Template == null)
                return;

            // match template
            var result = image.MatchTemplate(Template, _method);

            // optionally view the results image and return
            if (_viewResult)
            {
                annotatedImage = result.Convert<Bgr, byte>();
                return;
            }

            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;

            // find min/max values/locaitons from result image
            result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            Response = maxValues[0];

            // draw bounding rectangle around matched template in image
            var rect = new Rectangle(maxLocations[0], Template.Size);
            annotatedImage.Draw(rect, new Bgr(_annoColor.Color()), _lineThick);
            data = new List<object> {new Box(rect)};
        }

        private TemplateMatchingType _method = TemplateMatchingType.CcoeffNormed;
        [Category("Template Match (selectable)")]
        [PropertyOrder(0)]
        [DisplayName(@"Method")]
        [Description(@"Specifies the way the template must be compared with image regions.")]
        public TemplateMatchingType Method
        {
            get { return _method; }
            set { Set(ref _method, value); }
        }

        private bool _viewResult;
        [Category("Template Match (selectable)")]
        [PropertyOrder(1)]
        [DisplayName(@"View Result")]
        [Description(@"View the comparison result image.")]
        public bool ViewResult
        {
            get { return _viewResult; }
            set { Set(ref _viewResult, value); }
        }

        private double _response;
        [Category("Template Match (selectable)")]
        [PropertyOrder(2)]
        [DisplayName(@"Response")]
        [Description(@"Max value of the result image.")]
        [ReadOnly(true)]
        public double Response
        {
            get { return _response; }
            set { Set(ref _response, value); }
        }
    }
}
