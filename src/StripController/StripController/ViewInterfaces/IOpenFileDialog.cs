namespace StripController.ViewInterfaces
{
    public interface IOpenFileDialog : IDialog
    {
        string FileName { get; }
        string Filter { get; set; }
    }
}