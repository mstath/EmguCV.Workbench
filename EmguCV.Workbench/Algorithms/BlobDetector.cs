using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCV.Workbench.Model;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Algorithms
{
    public class BlobDetector : ImageAlgorithm
    {
        private readonly SimpleBlobDetectorParams _params = new SimpleBlobDetectorParams();

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            using (var dt = new SimpleBlobDetector(_params))
            using (var kp = new VectorOfKeyPoint())
            {
                dt.DetectRaw(image, kp);
                Features2DToolbox.DrawKeypoints(
                    image,
                    kp,
                    annotatedImage,
                    new Bgr(_annoColor.Color()),
                    Features2DToolbox.KeypointDrawType.DrawRichKeypoints);

                data = new List<object>();
                data.AddRange(kp.ToArray().Select(k => new KeyPoint(k)));
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(0)]
        [DisplayName(@"Filter By Color")]
        public bool FilterByColor
        {
            get { return _params.FilterByColor; }
            set
            {
                _params.FilterByColor = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(1)]
        [DisplayName(@"Blob Color")]
        [Description(@"The gray-scale value (0-255) for the blob color.")]
        public byte BlobColor
        {
            get { return _params.blobColor; }
            set
            {
                _params.blobColor = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(2)]
        [DisplayName(@"Min Threshold")]
        [Description(@"Minimum binary threshold for image.")]
        public float MinThreshold
        {
            get { return _params.MinThreshold; }
            set
            {
                _params.MinThreshold = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(3)]
        [DisplayName(@"Max Threshold")]
        [Description(@"Maximum binary threshold for image.")]
        public float MaxThreshold
        {
            get { return _params.MaxThreshold; }
            set
            {
                _params.MaxThreshold = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(4)]
        [DisplayName(@"Threshold Step")]
        [Description(@"Threshold step when going from Min. Threshold to Max. Threshold.")]
        public float ThresholdStep
        {
            get { return _params.ThresholdStep; }
            set
            {
                _params.ThresholdStep = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(5)]
        [DisplayName(@"Filter By Area")]
        public bool FilterByArea
        {
            get { return _params.FilterByArea; }
            set
            {
                _params.FilterByArea = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(6)]
        [DisplayName(@"Min Area")]
        [Description(@"Minimum area of blob in pixels.")]
        public float MinArea
        {
            get { return _params.MinArea; }
            set
            {
                _params.MinArea = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(7)]
        [DisplayName(@"Max Area")]
        [Description(@"Maximum area of blob in pixels.")]
        public float MaxArea
        {
            get { return _params.MaxArea; }
            set
            {
                _params.MaxArea = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(8)]
        [DisplayName(@"Filter By Circularity")]
        public bool FilterByCircularity
        {
            get { return _params.FilterByCircularity; }
            set
            {
                _params.FilterByCircularity = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(9)]
        [DisplayName(@"Min Circularity")]
        [Description(@"Minimum circularity as defined by 4*PI*Area / perimeter²")]
        public float MinCircularity
        {
            get { return _params.MinCircularity; }
            set
            {
                _params.MinCircularity = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(10)]
        [DisplayName(@"Max Circularity")]
        [Description(@"Maximum circularity as defined by 4*PI*Area / perimeter²")]
        public float MaxCircularity
        {
            get { return _params.MaxCircularity; }
            set
            {
                _params.MaxCircularity = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(11)]
        [DisplayName(@"Filter By Convexity")]
        public bool FilterByConvexity
        {
            get { return _params.FilterByConvexity; }
            set
            {
                _params.FilterByConvexity = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(12)]
        [DisplayName(@"Min Convexity")]
        [Description(@"Minimum convexity as defined by area / area of blob convex hull.")]
        public float MinConvexity
        {
            get { return _params.MinConvexity; }
            set
            {
                _params.MinConvexity = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(13)]
        [DisplayName(@"Max Convexity")]
        [Description(@"Maximum convexity as defined by area / area of blob convex hull.")]
        public float MaxConvexity
        {
            get { return _params.MaxConvexity; }
            set
            {
                _params.MaxConvexity = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(14)]
        [DisplayName(@"Filter By Inertia")]
        public bool FilterByInertia
        {
            get { return _params.FilterByInertia; }
            set
            {
                _params.FilterByInertia = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(15)]
        [DisplayName(@"Min Inertia Ratio")]
        [Description(@"Minimum ratio of inertia.")]
        public float MinInertiaRatio
        {
            get { return _params.MinInertiaRatio; }
            set
            {
                _params.MinInertiaRatio = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(16)]
        [DisplayName(@"Max Inertia Ratio")]
        [Description(@"Maximum ratio of inertia.")]
        public float MaxInertiaRatio
        {
            get { return _params.MaxInertiaRatio; }
            set
            {
                _params.MaxInertiaRatio = value;
                RaisePropertyChanged();
            }
        }

        [Category("Blob Detector")]
        [PropertyOrder(17)]
        [DisplayName(@"Min Dist Between Blobs")]
        [Description(@"The minimum distance in pixels between blobs to be included.")]
        public float MinDistBetweenBlobs
        {
            get { return _params.MinDistBetweenBlobs; }
            set
            {
                _params.MinDistBetweenBlobs = value;
                RaisePropertyChanged();
            }
        }

        [Browsable(false)]
        public override int LineThick { get; set; }
    }
}
