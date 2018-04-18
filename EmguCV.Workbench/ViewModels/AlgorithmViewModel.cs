using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Windows;
using EmguCV.Workbench.Algorithms;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.ViewModels
{
    public class AlgorithmViewModel : ViewModelBase
    {
        public AlgorithmViewModel()
        {
            // do not execute if in design mode
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            // import algorithms
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);

            // reorder the list and set selected algorithm to first (None)
            Algorithms = Algorithms.OrderBy(a => a.GetType() == typeof(None) ? 0 : 1).ThenBy(a => a.ToString()).ToList();
            SelectedAlgorithm = Algorithms.First();
        }

        [ImportMany]
        public List<IImageAlgorithm> Algorithms { get; set; }

        private IImageAlgorithm _selectedAlgorithm;
        public IImageAlgorithm SelectedAlgorithm
        {
            get { return _selectedAlgorithm; }
            set { Set(ref _selectedAlgorithm, value); }
        }
    }
}
