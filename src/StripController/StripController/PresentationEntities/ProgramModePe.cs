using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StripController.PresentationEntities
{
    public class ProgramModePe : PresentationEntity
    {
        private ObservableCollection<ProgramItemPe> _items;
        private bool _interpolate;
        private bool _canPlay;
        private bool _canStop;
        private bool _canPause;
        private TimeSpan _currentTime;
        
        public TimeSpan CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool CanPlay
        {
            get { return _canPlay; }
            set
            {
                if (_canPlay != value)
                {
                    _canPlay = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool CanStop
        {
            get { return _canStop; }
            set
            {
                if (_canStop != value)
                {
                    _canStop = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool CanPause
        {
            get { return _canPause; }
            set
            {
                if (_canPause != value)
                {
                    _canPause = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Interpolate
        {
            get { return _interpolate; }
            set
            {
                if (_interpolate != value)
                {
                    _interpolate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<ProgramItemPe> Items
        {
            get { return _items; }
            set
            {
                if(_items != null)
                    _items.CollectionChanged -= ItemsOnCollectionChanged;

                _items = value;

                if (_items != null)
                    _items.CollectionChanged += ItemsOnCollectionChanged;
            }
        }

        public ProgramModePe()
        {
            Items = new ObservableCollection<ProgramItemPe>();
        }

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            NotifyPropertyChanged(nameof(Items));
        }
    }
}