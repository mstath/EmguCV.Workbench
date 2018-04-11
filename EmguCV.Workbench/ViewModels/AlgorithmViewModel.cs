using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            // populate collection of instantiated image algorithms
            // and set the initial selected algorithm to first (None)
            Algorithms =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => typeof(IImageAlgorithm).IsAssignableFrom(t) && !t.IsAbstract)
                    .Select(Activator.CreateInstance)
                    .Select(i => i as IImageAlgorithm)
                    .ToList()
                    .OrderBy(a => a.GetType() == typeof(None) ? 0 : 1)
                    .ThenBy(a => a.Name);
            SelectedAlgorithm = Algorithms.First();
        }

        private IEnumerable<IImageAlgorithm> _algorithms;
        public IEnumerable<IImageAlgorithm> Algorithms
        {
            get { return _algorithms; }
            set { Set(ref _algorithms, value); }
        }

        private IImageAlgorithm _selectedAlgorithm;
        public IImageAlgorithm SelectedAlgorithm
        {
            get { return _selectedAlgorithm; }
            set { Set(ref _selectedAlgorithm, value); }
        }
    }
}
