using EmguCV.Workbench.ViewModels;

namespace EmguCV.Workbench
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // init Engine VM, pass other module refs, start engine
            (EngineView.DataContext as EngineViewModel)?.Initialize(
                ProcessorView.DataContext as ProcessorViewModel,
                AlgorithmView.DataContext as AlgorithmViewModel, 
                ImageView.DataContext as ImageViewModel);

            // init Image VM, pass other module refs
            (ImageView.DataContext as ImageViewModel)?.Initialize(
                EngineView.DataContext as EngineViewModel,
                AlgorithmView.DataContext as AlgorithmViewModel);
        }
    }
}
