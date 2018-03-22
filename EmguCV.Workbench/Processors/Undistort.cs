using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EmguCV.Workbench.Processors
{
    public class Undistort : ImageProcessor
    {
        private Mat _cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
        private Mat _distCoeffs = new Mat(8, 1, DepthType.Cv64F, 1);

        public Undistort()
        {
            try
            {
                LoadParemeters();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void LoadParemeters()
        {
            var cmFile = $"{nameof(_cameraMatrix)}.xml";
            var dcFile = $"{nameof(_distCoeffs)}.xml";
            if (File.Exists(cmFile) && File.Exists(dcFile))
            {
                using (var cmFs = File.Open(cmFile, FileMode.Open))
                using (var dcFs = File.Open(dcFile, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    _cameraMatrix = (Mat) formatter.Deserialize(cmFs);
                    _distCoeffs = (Mat) formatter.Deserialize(dcFs);
                }
                ParametersLoaded = true;
            }
        }

        private bool _parametersLoaded;
        [Category("Undistort")]
        [PropertyOrder(0)]
        [DisplayName(@"Parameters Loaded")]
        [Description(@"Calibration parameter files loaded; can undistort.")]
        [ReadOnly(true)]
        public bool ParametersLoaded
        {
            get { return _parametersLoaded; }
            set { Set(ref _parametersLoaded, value); }
        }

        public override void Process(ref Image<Bgr, byte> image)
        {
            if (!_parametersLoaded) return;

            var undistoredImage = image.Clone();
            CvInvoke.Undistort(image, undistoredImage, _cameraMatrix, _distCoeffs);
            image = undistoredImage;
        }
    }
}
