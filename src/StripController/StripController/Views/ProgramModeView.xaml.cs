using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using StripController.PresentationEntities;
using StripController.ViewInterfaces;

namespace StripController.Views
{
    public partial class ProgramModeView : IProgramModeView
    {
        public ProgramModePe DisplayObject
        {
            get { return (ProgramModePe)DataContext; }
            set
            {
                DataContext = value;
                UpdateSort();
            }
        }

        public event DeleteItemRequestedDelegate DeleteItemRequested;
        public event EventHandler CreateItemRequested;
        public event EventHandler PlayRequested;
        public event EventHandler StopRequested;
        public event EventHandler SaveProgramRequested;
        public event EventHandler LoadProgramRequested;
        public event EventHandler ResetProgramRequested;

        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public event EventHandler LoadStateRequested;
        public event EventHandler SaveStateRequested;

        public ProgramModeView()
        {
            InitializeComponent();
        }

        public void Activate()
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }

        public void Deactivate()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void LoadState()
        {
            RaiseLoadStateRequestedEvent();
        }

        public void SaveState()
        {
            RaiseSaveStateRequestedEvent();
        }
        
        private void OnItemDeleteClick(object sender, RoutedEventArgs e)
        {
            var button= sender as Button;
            var item = button?.Tag as ProgramItemPe;
            if (item == null)
                return;

            RaiseDeleteItemRequestedEvent(item);
        }

        private void OnCreateItemClick(object sender, RoutedEventArgs e)
        {
            RaiseCreateItemRequestedEvent();
        }

        private void OnPlayClick(object sender, RoutedEventArgs e)
        {
            RaisePlayRequestedEvent();
        }

        private void OnStopClick(object sender, RoutedEventArgs e)
        {
            RaiseStopRequestedEvent();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            RaiseSaveProgramRequestedEvent();
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            RaiseLoadProgramRequestedEvent();
        }

        private void OnResetClick(object sender, RoutedEventArgs e)
        {
            ResetResetProgramRequestedEvent();
        }

        private void RaisePlayRequestedEvent()
        {
            PlayRequested?.Invoke(this, EventArgs.Empty);
        }
        
        private void RaiseStopRequestedEvent()
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseDeleteItemRequestedEvent(ProgramItemPe item)
        {
            DeleteItemRequested?.Invoke(this, new DeleteItemRequestedAgrs(item));
        }

        private void RaiseCreateItemRequestedEvent()
        {
            CreateItemRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseSaveProgramRequestedEvent()
        {
            SaveProgramRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseLoadProgramRequestedEvent()
        {
            LoadProgramRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ResetResetProgramRequestedEvent()
        {
            ResetProgramRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseLoadStateRequestedEvent()
        {
            LoadStateRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseSaveStateRequestedEvent()
        {
            SaveStateRequested?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateSort()
        {
            var view = (CollectionView)CollectionViewSource.GetDefaultView(DisplayObject.Items);
            view.SortDescriptions.Add(new SortDescription(nameof(ProgramItemPe.Timeoffset), ListSortDirection.Ascending));
        }
    }
}
