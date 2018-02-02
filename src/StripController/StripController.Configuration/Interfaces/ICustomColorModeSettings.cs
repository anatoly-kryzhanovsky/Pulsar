namespace StripController.Configuration.Interfaces
{
    public interface ICustomColorModeSettings: IConfigSection
    {
        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }

        bool AutoApply { get; set; }
    }
}