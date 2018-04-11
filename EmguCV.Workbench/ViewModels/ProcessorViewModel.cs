using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCV.Workbench.Processors;
using EmguCV.Workbench.Util;

namespace EmguCV.Workbench.ViewModels
{
    public class ProcessorViewModel : ViewModelBase
    {
        private readonly SortedList<string, Type> _processorTypes;

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

            // create sorted list of names and types of image processors
            var list = new SortedList<string, Type>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IImageProcessor).IsAssignableFrom(t) && !t.IsAbstract))
                list.Add(Regex.Replace(type.Name, @"(\B[A-Z])", " $1"), type);
            _processorTypes = list;

            // set collection of processor names,
            // slected (first) name, and instatiate blank
            // collection of processors
            ProcessorNames = new List<string>(_processorTypes.Keys);
            SelectedProcessorName = ProcessorNames.First();
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
            // get selected processor type
            var type = _processorTypes[_selecteProcessorName];
            // create instance of type
            var instance = Activator.CreateInstance(type);
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

        private IEnumerable<string> _processorNames;
        public IEnumerable<string> ProcessorNames
        {
            get { return _processorNames; }
            set { Set(ref _processorNames, value); }
        }

        private string _selecteProcessorName;
        public string SelectedProcessorName
        {
            get { return _selecteProcessorName; }
            set { Set(ref _selecteProcessorName, value); }
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
