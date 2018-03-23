using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Util;
using Microsoft.Win32;

namespace EmguCV.Workbench.ViewModels
{
    public class EngineViewModel : ViewModelBase
    {
        private readonly VideoCapture[] _cameras;
        private ProcessorViewModel _processorVm;
        private AlgorithmViewModel _algorithmVm;
        private ImageViewModel _imageVm;
        private string _imageFile;
        private const int SleepTime = 100;
        private readonly Stopwatch _sw;
        private readonly object _lock = new object();

        private Image<Bgr, byte> _image;
        public Image<Bgr, byte> Image => _image;
        private Image<Bgr, byte> _annotatedImage;

        public RelayCommand SelectFileCommand { get; set; }
        public RelayCommand SnapImageCommand { get; set; }

        public EngineViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                HasCameras = IsCameraSource = true;
                return;
            }

            SelectFileCommand = new RelayCommand(DoSelectFile);
            SnapImageCommand = new RelayCommand(DoSnapImage);

            var devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (devs.Length>0)
            {
                _cameras = new VideoCapture[devs.Length];
                var names = new List<string>(devs.Length);
                for (var i = 0; i < devs.Length; i++)
                {
                    _cameras[i] = new VideoCapture(i);
                    names.Add(devs[i].Name);
                }
                CameraNames = names;
                CameraIndex = 0;
                HasCameras = IsCameraSource = true;
            }
            else
                HasCameras = IsCameraSource = false;

            IsFileSource = !IsCameraSource;

            _sw = Stopwatch.StartNew();
        }

        public void Initialize(ProcessorViewModel processorVm, AlgorithmViewModel algorithmVm, ImageViewModel frameVm)
        {
            _processorVm = processorVm;
            _algorithmVm = algorithmVm;
            _imageVm = frameVm;

            StartEngine();
        }

        private void StartEngine()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (_isFreezeFrame)
                    {
                        Thread.Sleep(SleepTime);
                        continue;
                    }

                    List<object> data = null;
                    try
                    {
                        _image = GetImage();

                        _sw.Restart();

                        _processorVm.Process(ref _image);

                        lock (_lock)
                            _algorithmVm.SelectedAlgorithm.Process(_image, out _annotatedImage, out data);
                    }
                    catch (Exception ex)
                    {
                        lock (_lock)
                            _annotatedImage = GetExceptionImage(ex);
                    }
                    finally
                    {
                        _sw.Stop();
                        _imageVm.SetImage(_annotatedImage);
                        _imageVm.Data = data;
                        _imageVm.FrameSizeStatus = $"{_annotatedImage.Width} x {_annotatedImage.Height}";
                        _imageVm.FrameTimeStatus = $"{_sw.ElapsedMilliseconds} ms";
                    }
                }
            });
        }

        private Image<Bgr, byte> GetImage()
        {
            if (_isCameraSource)
                return _cameras[_cameraIndex].QueryFrame().ToImage<Bgr, byte>();

            Thread.Sleep(SleepTime);

            if (!string.IsNullOrEmpty(_imageFile) && File.Exists(_imageFile))
                return new Image<Bgr, byte>(_imageFile);

            return new Image<Bgr, byte>(640, 480);
        }

        private Image<Bgr, byte> GetExceptionImage(Exception ex)
        {
            using (var bm = new Bitmap(640, 480))
            using (var gr = Graphics.FromImage(bm))
            {
                gr.FillRectangle(Brushes.Black, 0, 0, bm.Width, bm.Height);
                var rectf = new RectangleF(10, 10, 620, 460);
                gr.DrawString(ex.ToString(), new Font("Tahoma", 10), Brushes.White, rectf);
                return new Image<Bgr, byte>(bm);
            }
        }

        private bool _hasCameras;
        public bool HasCameras
        {
            get { return _hasCameras; }
            set { Set(ref _hasCameras, value); }
        }

        private bool _isCameraSource;
        public bool IsCameraSource
        {
            get { return _isCameraSource; }
            set { Set(ref _isCameraSource, value); }
        }

        private bool _isFileSource;
        public bool IsFileSource
        {
            get { return _isFileSource; }
            set { Set(ref _isFileSource, value); }
        }

        private bool _isFreezeFrame;
        public bool IsFreezeFrame
        {
            get { return _isFreezeFrame; }
            set { Set(ref _isFreezeFrame, value); }
        }

        private int _cameraIndex;
        public int CameraIndex
        {
            get { return _cameraIndex; }
            set { Set(ref _cameraIndex, value); }
        }

        private List<string> _cameraNames;
        public List<string> CameraNames
        {
            get { return _cameraNames; }
            set { Set(ref _cameraNames, value); }
        }

        private string _selectedFileName;
        public string SelectedFileName
        {
            get { return _selectedFileName; }
            set { Set(ref _selectedFileName, value); }
        }

        private void DoSelectFile()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.gif, *.png) | *.jpg; *.jpeg; *.gif; *.png"
            };

            var result = dlg.ShowDialog();
            if (result == true)
            {
                _imageFile = dlg.FileName;
                SelectedFileName = Path.GetFileName(_imageFile);
            }
        }

        private void DoSnapImage()
        {
            lock (_lock)
            {
                var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var file = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".png";
                _annotatedImage?.Save(Path.Combine(dir, file));
            }
        }
    }
}
