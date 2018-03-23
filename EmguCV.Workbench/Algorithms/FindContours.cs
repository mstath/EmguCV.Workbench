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
    public class FindContours : ImageAlgorithm
    {
        public override int Order => 10;

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(image.Convert<Gray, byte>(), contours, null, _mode, _method);

                annotatedImage.DrawPolyline(contours.ToArrayOfArray(), false, new Bgr(Color.Red));

                data = contours.ToArrayOfArray().Select(c => new Contour(c)).Cast<object>().ToList();
            }
        }

        private RetrType _mode = RetrType.List;
        [Category("Find Contours")]
        [PropertyOrder(0)]
        [DisplayName(@"Mode")]
        [Description(@"Retrieval mode.")]
        public RetrType Mode
        {
            get { return _mode; }
            set { Set(ref _mode, value); }
        }

        private ChainApproxMethod _method = ChainApproxMethod.ChainApproxSimple;
        [Category("Find Contours")]
        [PropertyOrder(1)]
        [DisplayName(@"Method")]
        [Description(@"Approximation method.")]
        public ChainApproxMethod Method
        {
            get { return _method; }
            set { Set(ref _method, value); }
        }
    }
}
