using System.Drawing;

namespace StripController.PresentationEntities
{
    public class AudioCaptureColorModePe : PresentationEntity
    {
        private double _sensivity;
        private bool _isEnabled;
 
        public Bitmap Bitmap { get; set; }

        public double Sensivity
        {
            get { return _sensivity; }
            set
            {
                if (_sensivity != value)
                {
                    _sensivity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}