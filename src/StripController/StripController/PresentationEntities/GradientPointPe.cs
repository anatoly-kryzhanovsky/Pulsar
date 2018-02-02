using System.Windows.Media;

namespace StripController.PresentationEntities
{
    public class GradientPointPe: PresentationEntity
    {
        private Color _color;
        private double _value;

        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}