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
        private readonly Dictionary<string, Type> _processorTypes;

        public RelayCommand AddProcessorCommand { get; set; }
        public RelayCommand MoveProcessorUpCommand { get; set; }
        public RelayCommand MoveProcessorDownCommand { get; set; }
        public RelayCommand RemoveProcessorCommand { get; set; }

        public ProcessorViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            AddProcessorCommand = new RelayCommand(DoAddProcessor);
            MoveProcessorUpCommand = new RelayCommand(DoMoveProcessorUp);
            MoveProcessorDownCommand = new RelayCommand(DoMoveProcessorDown);
            RemoveProcessorCommand = new RelayCommand(DoRemoveProcessor);

            _processorTypes =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.BaseType == typeof(ImageProcessor))
                    .ToDictionary(type => Regex.Replace(type.Name, @"(\B[A-Z])", " $1"));
            ProcessorNames = new List<string>(_processorTypes.Keys);
            SelectedProcessorName = ProcessorNames.First();
            Processors = new ObservableCollection<IImageProcessor>();
        }

        public void Process(ref Image<Gray, byte> image)
        {
            foreach (var processor in Processors)
                processor.Process(ref image);
        }

        private void DoAddProcessor()
        {
            var type = _processorTypes[_selecteProcessorName];
            var instance = Activator.CreateInstance(type);
            Processors.Add(instance as IImageProcessor);
        }

        private void DoMoveProcessorUp()
        {
            if (_selectedProcessorIndex >= 0 && _selectedProcessorIndex - 1 >= 0)
                Processors.Move(_selectedProcessorIndex, _selectedProcessorIndex - 1);
        }

        private void DoMoveProcessorDown()
        {
            if (_selectedProcessorIndex >= 0 && _selectedProcessorIndex + 1 < Processors.Count)
                Processors.Move(_selectedProcessorIndex, _selectedProcessorIndex + 1);
        }

        private void DoRemoveProcessor()
        {
            if (_selectedProcessorIndex >= 0 && Processors.Count > 0)
                Processors.RemoveAt(_selectedProcessorIndex);
        }

        private List<string> _processorNames;
        public List<string> ProcessorNames
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
