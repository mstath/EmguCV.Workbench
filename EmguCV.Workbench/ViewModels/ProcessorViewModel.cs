using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Windows;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Processors;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.ViewModels
{
    public class ProcessorViewModel : ViewModelBase
    {
        public RelayCommand AddProcessorCommand { get; set; }
        public RelayCommand MoveProcessorUpCommand { get; set; }
        public RelayCommand MoveProcessorDownCommand { get; set; }
        public RelayCommand RemoveProcessorCommand { get; set; }

        public ProcessorViewModel()
        {
            // do not execute if in design mode
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            // instantiate command bindings
            AddProcessorCommand = new RelayCommand(DoAddProcessor);
            MoveProcessorUpCommand = new RelayCommand(DoMoveProcessorUp);
            MoveProcessorDownCommand = new RelayCommand(DoMoveProcessorDown);
            RemoveProcessorCommand = new RelayCommand(DoRemoveProcessor);

            // create a sorted list of processor types and
            // set selected processor type to first
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            ProcessorTypes = Helper.GetExportedTypes<IImageProcessor>(catalog).ToList();
            ProcessorTypes.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            SelectedProcessorType = ProcessorTypes.First();

            // instantiate the processor collection
            Processors = new ObservableCollection<IImageProcessor>();
        }

        /// <summary>
        /// Run source image through each image processor in collection.
        /// </summary>
        /// <param name="image">The source image.</param>
        public void Process(ref Image<Bgr, byte> image)
        {
            foreach (var processor in Processors)
                processor.Process(ref image);
        }

        /// <summary>
        /// The command-bound method for adding a processor to the collection.
        /// </summary>
        private void DoAddProcessor()
        {
            // create instance of selected type
            var instance = Activator.CreateInstance(_selectedProcessorType);
            // add instance to collection
            Processors.Add(instance as IImageProcessor);
        }

        /// <summary>
        /// The command-bound method for moving a processor up in the collection.
        /// </summary>
        private void DoMoveProcessorUp()
        {
            if (_selectedProcessorIndex - 1 >= 0)
                Processors.Move(_selectedProcessorIndex, _selectedProcessorIndex - 1);
        }

        /// <summary>
        /// The command-bound method for moving a processor down in the collection.
        /// </summary>
        private void DoMoveProcessorDown()
        {
            if (_selectedProcessorIndex + 1 < Processors.Count)
                Processors.Move(_selectedProcessorIndex, _selectedProcessorIndex + 1);
        }

        /// <summary>
        /// The command-bound method for removing a processor from the collection.
        /// </summary>
        private void DoRemoveProcessor()
        {
            if (_selectedProcessorIndex >= 0 && Processors.Count > 0)
                Processors.RemoveAt(_selectedProcessorIndex);
        }

        public List<Type> ProcessorTypes { get; }

        private Type _selectedProcessorType;
        public Type SelectedProcessorType
        {
            get { return _selectedProcessorType; }
            set { Set(ref _selectedProcessorType, value); }
        }

        public ObservableCollection<IImageProcessor> Processors { get; }

        private int _selectedProcessorIndex;
        public int SelectedProcessorIndex
        {
            get { return _selectedProcessorIndex; }
            set { Set(ref _selectedProcessorIndex, value); }
        }
    }
}
