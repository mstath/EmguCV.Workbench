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
        private int _cpuUsage;
        private readonly Stopwatch _sw;
        private readonly object _lock = new object();

        private Image<Bgr, byte> _image;
        public Image<Bgr, byte> Image => _image;
        private Image<Bgr, byte> _annotatedImage;

        public RelayCommand SelectFileCommand { get; set; }
        public RelayCommand SnapImageCommand { get; set; }

        public EngineViewModel()
        {
            // do not execute if in design mode
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                HasCameras = IsCameraSource = true;
                return;
            }

            // instantiate command bindings
            SelectFileCommand = new RelayCommand(DoSelectFile);
            SnapImageCommand = new RelayCommand(DoSnapImage);

            // get list of usb cameras
            var devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (devs.Length>0)
            {
                // populate collection of usb cameras
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

            // set file source opposite of camera source
            IsFileSource = !IsCameraSource;

            // instantiate frame stopwatch
            _sw = Stopwatch.StartNew();
        }

        /// <summary>
        /// Initializes the engine view model.
        /// </summary>
        public void Initialize(ProcessorViewModel processorVm, AlgorithmViewModel algorithmVm, ImageViewModel frameVm)
        {
            // set references to the other view models
            _processorVm = processorVm;
            _algorithmVm = algorithmVm;
            _imageVm = frameVm;

            // start the engine
            StartEngine();
        }

        /// <summary>
        /// Starts the engine.
        /// </summary>
        private void StartEngine()
        {
            // start the asynchronous cpu counter
            StartCpuCounter();

            // asynchronous task for
            // grabbing and processing frames
            Task.Run(() =>
            {
                // repeat forever
                while (true)
                {
                    // if freeze frame sleep delay (ease cpu) and return
                    if (_isFreezeFrame)
                    {
                        Thread.Sleep(SleepTime);
                        continue;
                    }

                    List<object> data = null;
                    try
                    {
                        // grab the image from source
                        // (camera or file)
                        _image = GetImage();

                        // restart stopwatch
                        _sw.Restart();

                        // pass image through processors
                        _processorVm.Process(ref _image);

                        // pass image through selected algorithm
                        lock (_lock)
                            _algorithmVm.SelectedAlgorithm.Process(_image, out _annotatedImage, out data);
                    }
                    catch (Exception ex)
                    {
                        // present image with exception text
                        lock (_lock)
                            _annotatedImage = GetExceptionImage(ex);
                    }
                    finally
                    {
                        // stop stopwatch
                        _sw.Stop();
                        // present annotated image
                        _imageVm.SetImage(_annotatedImage);
                        // present data from algorithms
                        _imageVm.Data = data;
                        // set frame status info
                        _imageVm.FrameSizeStatus = $"{_annotatedImage.Width} x {_annotatedImage.Height}";
                        _imageVm.FrameTimeStatus = $"{_sw.ElapsedMilliseconds} ms, CPU {_cpuUsage}%";
                    }
                }
            });
        }

        /// <summary>
        /// Starts the cpu counter.
        /// </summary>
        private void StartCpuCounter()
        {
            // instantiate performance counter
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", Environment.MachineName);

            // asynchronous task for
            // measuring cpu performance
            Task.Run(() =>
            {
                // repeat forever
                while (true)
                {
                    // get cpu performance and sleep
                    // (otherwise performance always 0%)
                    _cpuUsage = (int) cpuCounter.NextValue();
                    Thread.Sleep(1000);
                }
            });
        }

        /// <summary>
        /// Gets the image from selected source.
        /// </summary>
        /// <returns>New BGR image.</returns>
        private Image<Bgr, byte> GetImage()
        {
            // if camera source, grab image from selected camera
            if (_isCameraSource)
                return _cameras[_cameraIndex].QueryFrame().ToImage<Bgr, byte>();

            // if not camera sleep to ease cpu
            Thread.Sleep(SleepTime);

            // if file exists, return new BGR image from file
            // otherwise return 'blank' frame
            if (!string.IsNullOrEmpty(_imageFile) && File.Exists(_imageFile))
                return new Image<Bgr, byte>(_imageFile);

            return new Image<Bgr, byte>(640, 480);
        }

        /// <summary>
        /// Gets the exception image.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>A black image with the text of the exception.</returns>
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

        /// <summary>
        /// The command-bound method for selecting a file.
        /// </summary>
        private void DoSelectFile()
        {
            // present dialog for selecting image-type files
            var dlg = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.gif, *.png) | *.jpg; *.jpeg; *.gif; *.png"
            };
            var result = dlg.ShowDialog();

            // if selected set properties for file path
            // and name (presentation)
            if (result == true)
            {
                _imageFile = dlg.FileName;
                SelectedFileName = Path.GetFileName(_imageFile);
            }
        }

        /// <summary>
        /// The command-bound method for saving image to desktop.
        /// </summary>
        private void DoSnapImage()
        {
            lock (_lock)
            {
                // get desktop path
                var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                // set file name to current timestamp
                var file = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".png";
                // save the image
                _annotatedImage?.Save(Path.Combine(dir, file));
            }
        }
    }
}
