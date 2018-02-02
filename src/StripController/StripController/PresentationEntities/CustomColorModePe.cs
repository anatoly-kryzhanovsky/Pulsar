namespace StripController.PresentationEntities
{
    public class CustomColorModePe : PresentationEntity
    {
        private byte _r;
        private byte _g;
        private byte _b;
        private bool _autoApply;
        private byte _brightness;

        public byte Brightness
        {
            get { return _brightness; }
            set
            {
                if (_brightness != value)
                {
                    _brightness = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool AutoApply
        {
            get { return _autoApply; }
            set
            {
                if (_autoApply != value)
                {
                    _autoApply = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public byte R
        {
            get { return _r; }
            set
            {
                if (_r != value)
                {
                    _r = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public byte G
        {
            get { return _g; }
            set
            {
                if (_g != value)
                {
                    _g = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public byte B
        {
            get { return _b; }
            set
            {
                if (_b != value)
                {
                    _b = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
