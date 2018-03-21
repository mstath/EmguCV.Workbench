using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EmguCV.Workbench.Algorithms;
using EmguCV.Workbench.ViewModels;


namespace EmguCV.Workbench.Views
{
    public partial class ImageView
    {
        private readonly ImageViewModel _viewModel;
        private bool _isDragging;
        private Point _startPoint;
        private Point _scale;
        private Point _offset;
        private Rectangle _box;

        public ImageView()
        {
            InitializeComponent();

            _viewModel = DataContext as ImageViewModel;
        }

        private void ImageCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (!(_viewModel?.AlgorithmVm?.SelectedAlgorithm is IImageTemplateAlgorithm))
                return;

            _startPoint = e.MouseDevice.GetPosition(ImageCanvas);
            _scale.X = _viewModel.Image.Width / Image.RenderSize.Width;
            _scale.Y = _viewModel.Image.Height / Image.RenderSize.Height;
            _offset.X = (Image.RenderSize.Width - ImageCanvas.Width)/2;
            _offset.Y = (Image.RenderSize.Height - ImageCanvas.Height)/2;
            _isDragging = _viewModel.EngineVm.IsFreezeFrame = true;
        }

        private void ImageCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging)
                return;

            var pos = e.MouseDevice.GetPosition(ImageCanvas);
            var x = Math.Min(pos.X, _startPoint.X);
            var y = Math.Min(pos.Y, _startPoint.Y);
            var w = Math.Abs(pos.X - _startPoint.X);
            var h = Math.Abs(pos.Y - _startPoint.Y);

            ImageCanvas.Children.Remove(_box);

            _box = new Rectangle
            {
                Width = w,
                Height = h,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection(new List<double> { 4, 4 })
            };

            Canvas.SetLeft(_box, x);
            Canvas.SetTop(_box, y);

            ImageCanvas.Children.Add(_box);

            if (e.LeftButton == MouseButtonState.Released)
            {
                _viewModel.GrabTemplate(
                    new System.Drawing.Rectangle(
                        (int) (_scale.X*(x + _offset.X)),
                        (int) (_scale.Y*(y + _offset.Y)),
                        (int) (_scale.X*w),
                        (int) (_scale.Y*h)));

                ImageCanvas.Children.Remove(_box);

                _isDragging = _viewModel.EngineVm.IsFreezeFrame = false;
            }
        }
    }
}
