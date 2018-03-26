using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EmguCV.Workbench.Util;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Color = System.Windows.Media.Color;

namespace EmguCV.Workbench.Algorithms
{
    public class CameraCalibration : ImageAlgorithm
    {
        private Task _calTask;

        public override void Process(Image<Bgr, byte> image, out Image<Bgr, byte> annotatedImage, out List<object> data)
        {
            base.Process(image, out annotatedImage, out data);

            if (_performCal && _calTask?.Status != TaskStatus.Running)
            {
                _calTask = new Task(PerformCalibration);
                _calTask.Start();
            }

            if (_find)
                FindCorners(image, ref annotatedImage, new VectorOfPointF());

            if (_snap)
                SnapCalibrate(image, ref annotatedImage);

            if (_undistort)
            {
                var undistoredImage = image.Clone();
                CvInvoke.Undistort(image, undistoredImage, _cameraMatrix, _distCoeffs);
                annotatedImage = undistoredImage;
            }
        }

        private void PerformCalibration()
        {
            Find = Undistort = false;
            while (!_undistort && _performCal)
            {
                Thread.Sleep(TimeSpan.FromSeconds(_snapDelay));
                Snap = true;
                while (_snap && _performCal) { }
            }
            PerformCal = false;
        }

        private bool FindCorners(Image<Bgr, byte> image, ref Image<Bgr, byte> annotatedImage, VectorOfPointF corners)
        {
            var found = false;
            using (var det = new SimpleBlobDetector())
            {
                var size = new Size(_cornersPerRow, _cornersPerCol);
                switch (_targetType)
                {
                    case CalibTargetType.ChessBoard:
                        found = CvInvoke.FindChessboardCorners(image, size, corners);
                        if (found)
                            CvInvoke.CornerSubPix(image.Convert<Gray, byte>(), corners, new Size(11, 11), new Size(-1, -1),
                                new MCvTermCriteria(30, 0.1));
                        break;

                    case CalibTargetType.CirclesGrid:
                        found = CvInvoke.FindCirclesGrid(image, size, corners, _circlesGridType, det);
                        break;
                }
                CvInvoke.DrawChessboardCorners(annotatedImage, size, corners, found);
                return found;
            }
        }

        private Mat _cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
        private Mat _distCoeffs = new Mat(8, 1, DepthType.Cv64F, 1);
        private VectorOfVectorOfPoint3D32F _objectPoints;
        private VectorOfVectorOfPointF _imagePoints;
        private Mat _rvecs;
        private Mat _tvecs;

        private void SnapCalibrate(Image<Bgr, byte> image, ref Image<Bgr, byte> annotatedImage)
        {
            try
            {
                // initialize variables if first snap
                if (_imageIndex == 0)
                {
                    _cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
                    _distCoeffs = new Mat(8, 1, DepthType.Cv64F, 1);
                    _objectPoints = new VectorOfVectorOfPoint3D32F();
                    _imagePoints = new VectorOfVectorOfPointF();
                    _rvecs = new Mat();
                    _tvecs = new Mat();
                    Find = false;
                    Undistort = false;
                }

                // find corners/circles
                var corners = new VectorOfPointF();
                var found = FindCorners(image, ref annotatedImage, corners);
                if (!found) return;

                // flash outpout image
                annotatedImage = annotatedImage.Not();

                // add corners to image points vector
                _imagePoints.Push(corners);

                // construct object points
                var objectList = new List<MCvPoint3D32f>();
                for (var col = 0; col < _cornersPerCol; col++)
                    for (var row = 0; row < _cornersPerRow; row++)
                        objectList.Add(new MCvPoint3D32f(row * _objWidth, col * _objHeight, 0.0F));

                // add constructed object points to object points vector
                _objectPoints.Push(new VectorOfPoint3D32F(objectList.ToArray()));

                // increment image index
                ImageIndex++;

                // exti if haven't reached number of images
                if (_imageIndex < _numberOfImages) return;

                // estimate intrinsic/extrinsic parameters
                ProjectionError = CvInvoke.CalibrateCamera(
                    _objectPoints,
                    _imagePoints,
                    image.Size,
                    _cameraMatrix,
                    _distCoeffs,
                    _rvecs,
                    _tvecs,
                    _calibType,
                    new MCvTermCriteria(30, 0.1));

                // latch undistort; reset image index
                Undistort = true;
                ImageIndex = 0;

                // save parameters to file to be used by undistort image processor
                SaveParameters();
            }
            finally
            {
                Snap = false;
            }
        }

        private void SaveParameters()
        {
            var cmFile = $"{nameof(_cameraMatrix)}.xml";
            var dcFile = $"{nameof(_distCoeffs)}.xml";
            using (var cmFs = File.Open(cmFile, FileMode.Create, FileAccess.Write))
            using (var dcFs = File.Open(dcFile, FileMode.Create, FileAccess.Write))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(cmFs, _cameraMatrix);
                formatter.Serialize(dcFs, _distCoeffs);
            }
        }

        private CalibTargetType _targetType = CalibTargetType.ChessBoard;
        [Category("Camera Calibration")]
        [PropertyOrder(0)]
        [DisplayName(@"Target Type")]
        [Description(@"Chessboard or Circles Grid calibration target.")]
        public CalibTargetType TargetType
        {
            get { return _targetType; }
            set { Set(ref _targetType, value); }
        }

        private int _cornersPerRow = 10;
        [Category("Camera Calibration")]
        [PropertyOrder(1)]
        [DisplayName(@"Corners Per Row")]
        [Description(@"Inner corners (chessboard) or circles (circles grid) per row.")]
        public int CornersPerRow
        {
            get { return _cornersPerRow; }
            set { Set(ref _cornersPerRow, value.Clamp(2, 100)); }
        }

        private int _cornersPerCol = 6;
        [Category("Camera Calibration")]
        [PropertyOrder(2)]
        [DisplayName(@"Corners Per Col")]
        [Description(@"Inner corners (chessboard) or circles (circles grid) per column.")]
        public int CornersPerCol
        {
            get { return _cornersPerCol; }
            set { Set(ref _cornersPerCol, value.Clamp(2, 100)); }
        }

        private CalibCgType _circlesGridType = CalibCgType.SymmetricGrid;
        [Category("Camera Calibration")]
        [PropertyOrder(3)]
        [DisplayName(@"Circles Grid Type")]
        [Description(@"Type of circles grid calibration.")]
        public CalibCgType CirclesGridType
        {
            get { return _circlesGridType; }
            set { Set(ref _circlesGridType, value); }
        }

        private bool _find;
        [Category("Camera Calibration")]
        [PropertyOrder(4)]
        [DisplayName(@"Find")]
        [Description(@"Find corners/circles and display on image.")]
        public bool Find
        {
            get { return _find; }
            set { Set(ref _find, value); }
        }

        private float _objWidth = 10;
        [Category("Camera Calibration")]
        [PropertyOrder(5)]
        [DisplayName(@"Object Width")]
        [Description(@"Horizontal distance between corners/circles in world units.")]
        public float ObjWidth
        {
            get { return _objWidth; }
            set { Set(ref _objWidth, value); }
        }

        private float _objHeight = 10;
        [Category("Camera Calibration")]
        [PropertyOrder(6)]
        [DisplayName(@"Object Height")]
        [Description(@"Vertical distance between corners/circles in world units.")]
        public float ObjHeight
        {
            get { return _objHeight; }
            set { Set(ref _objHeight, value); }
        }

        private CalibType _calibType = CalibType.RationalModel;
        [Category("Camera Calibration")]
        [PropertyOrder(7)]
        [DisplayName(@"Calibration Type")]
        public CalibType CalibType
        {
            get { return _calibType; }
            set { Set(ref _calibType, value); }
        }

        private int _numberOfImages = 1;
        [Category("Camera Calibration")]
        [PropertyOrder(8)]
        [DisplayName(@"Number of Images")]
        [Description(@"Number of images to calibrate from.")]
        public int NumberOfImages
        {
            get { return _numberOfImages; }
            set { Set(ref _numberOfImages, value.Clamp(1, 100)); }
        }

        private int _imageIndex;
        [Category("Camera Calibration")]
        [PropertyOrder(9)]
        [DisplayName(@"Image Index")]
        [Description(@"Index of calibration images.")]
        [ReadOnly(true)]
        public int ImageIndex
        {
            get { return _imageIndex; }
            set { Set(ref _imageIndex, value); }
        }

        private bool _snap;
        [Category("Camera Calibration")]
        [PropertyOrder(10)]
        [DisplayName(@"Snap")]
        [Description(@"Snap until Image Index = Number of Images; unchecks when snap is done.")]
        public bool Snap
        {
            get { return _snap; }
            set { Set(ref _snap, value); }
        }

        private double _projectionError;
        [Category("Camera Calibration")]
        [PropertyOrder(11)]
        [DisplayName(@"Projection Error")]
        [Description(@"The intrinsic calculation error from calibration.")]
        [ReadOnly(true)]
        public double ProjectionError
        {
            get { return _projectionError; }
            set { Set(ref _projectionError, value); }
        }

        private bool _undistort;
        [Category("Camera Calibration")]
        [PropertyOrder(12)]
        [DisplayName(@"Undistort")]
        [Description(@"Gets checked after calibration routine.")]
        public bool Undistort
        {
            get { return _undistort; }
            set { Set(ref _undistort, value); }
        }

        private bool _performCal;
        [Category("Camera Calibration")]
        [PropertyOrder(13)]
        [DisplayName(@"Perform Calibration")]
        [Description(@"Auto calibration routine with delay between snaps.")]
        public bool PerformCal
        {
            get { return _performCal; }
            set { Set(ref _performCal, value); }
        }

        private float _snapDelay = 3;
        [Category("Camera Calibration")]
        [PropertyOrder(14)]
        [DisplayName(@"Snap Delay")]
        [Description(@"Delay in seconds between snaps when Perform Calibration is checked.")]
        public float SnapDelay
        {
            get { return _snapDelay; }
            set { Set(ref _snapDelay, value); }
        }

        [Browsable(false)]
        public override Color AnnoColor { get; set; }

        [Browsable(false)]
        public override int LineThick { get; set; }
    }
}
