using EmguCV.Workbench.ViewModels;

namespace EmguCV.Workbench
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            (EngineView.DataContext as EngineViewModel)?.Initialize(
                ProcessorView.DataContext as ProcessorViewModel,
                null,
                ImageView.DataContext as ImageViewModel);
        }
    }
}
