using System;

namespace StripController.PresentationEntities
{
    public class ProgramItemPe : PresentationEntity
    {
        private TimeSpan _timeoffset;
        private byte _r;
        private byte _g;
        private byte _b;
        private byte _startPixel;
        private byte _endPixel;
        private EProgramItemType _type;
        private byte _brightness;
        private bool _current;

        public bool Current
        {
            get { return _current; }
            set
            {
                if (_current != value)
                {
                    _current = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public TimeSpan Timeoffset
        {
            get { return _timeoffset; }
            set
            {
                if (_timeoffset != value)
                {
                    _timeoffset = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public EProgramItemType Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public byte StartPixel
        {
            get { return _startPixel; }
            set
            {
                if (_startPixel != value)
                {
                    _startPixel = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public byte EndPixel
        {
            get { return _endPixel; }
            set
            {
                if (_endPixel != value)
                {
                    _endPixel = value;
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
    }
}