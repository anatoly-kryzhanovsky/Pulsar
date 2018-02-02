using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;

namespace StripController.PresentationEntities
{
    public class GradientSelectorPe : PresentationEntity
    {
        private Brush _brush;
        private ObservableCollection<GradientPointPe> _points;

        public Brush Brush
        {
            get { return _brush; }
            set
            {
                if (_brush != value)
                {
                    _brush = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<GradientPointPe> Points
        {
            get { return _points; }
            set
            {
                if (_points != value)
                {
                    if (_points != null)
                        _points.CollectionChanged -= PointsOnCollectionChanged;

                    _points = value;

                    if (_points != null)
                        _points.CollectionChanged += PointsOnCollectionChanged;

                    NotifyPropertyChanged();
                }
            }
        }

        public GradientSelectorPe()
        {
            Points = new ObservableCollection<GradientPointPe>();
        }

        private void PointsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            NotifyPropertyChanged(nameof(Points));
        }
    }
}