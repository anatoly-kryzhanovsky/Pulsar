using StripController.ViewInterfaces;

namespace StripController.Views
{
    class SaveFileDialog: ISaveFileDialog
    {
        public string FileName { get; private set; }
        public string Filter { get; set; }

        public bool ShowDialog()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog {Filter = Filter};
            var result = dialog.ShowDialog();
            
            if (result.HasValue && result.Value)
            {
                FileName = dialog.FileName;
                return true;
            }

            return false;
        }
    }
}
