namespace StripController.ViewInterfaces
{
    public interface ISaveFileDialog : IDialog
    {
        string FileName { get; }
        string Filter { get; set; }
    }
}