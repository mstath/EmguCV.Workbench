using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EmguCV.Workbench.Model;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    public class TemplateMatch : ImageTemplateAlgorithm
    {
        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            if (Template == null)
                return;

            var result = image.MatchTemplate(Template, _method);

            if (_viewResult)
            {
                annotatedImage = result.Convert<Bgr, byte>();
                return;
            }

            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
            result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            Response = maxValues[0];

            var rect = new Rectangle(maxLocations[0], Template.Size);
            annotatedImage.Draw(rect, new Bgr(Color.Red));
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
