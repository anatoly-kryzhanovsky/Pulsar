using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using StripController.Configuration.ConfigurationSections;
using StripController.Configuration.Interfaces;

namespace StripController.Services
{
    class Settings
    {
        private const string EmptyConfiguration = "<configuration></configuration>";

        private readonly StripperConfigSection _stripperConfig;
        private readonly CaptureModeConfigSection _captureModeConfig;
        private readonly CustomColorModeConfigSection _customColorModeConfig;
        private readonly ProgramModeConfigurationSection _programModeConfig;
        private readonly System.Configuration.Configuration _config;

        public IStripperSettings StripperSettings => _stripperConfig;
        public IAudioCaptureModeSettings CaptrureModeSettings => _captureModeConfig;
        public ICustomColorModeSettings CustomColorModeSettings => _customColorModeConfig;
        public IProgramModeSettings ProgramModeSettings => _programModeConfig;

        public Settings()
        {
            var configMap = new ExeConfigurationFileMap();

            var name = Assembly.GetExecutingAssembly().GetName().Name;

            configMap.LocalUserConfigFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{name}.config");
            configMap.RoamingUserConfigFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{name}.config");
            configMap.ExeConfigFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{name}.config");

            if(!File.Exists(configMap.LocalUserConfigFilename))
                File.WriteAllText(configMap.LocalUserConfigFilename, EmptyConfiguration);

            if (!File.Exists(configMap.RoamingUserConfigFilename))
                File.WriteAllText(configMap.RoamingUserConfigFilename, EmptyConfiguration);

            if (!File.Exists(configMap.ExeConfigFilename))
                File.WriteAllText(configMap.ExeConfigFilename, EmptyConfiguration);

            var defaultConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.PerUserRoamingAndLocal);

            _stripperConfig = (StripperConfigSection)defaultConfig.GetSection("stripper");

            _captureModeConfig = LoadSection<CaptureModeConfigSection>("capturemode", defaultConfig);
            _customColorModeConfig = LoadSection<CustomColorModeConfigSection>("customcolor", defaultConfig);
            _programModeConfig = LoadSection<ProgramModeConfigurationSection>("program", defaultConfig);
        }

        public void Save()
        {
            _config.Save(ConfigurationSaveMode.Full);
        }

        private T LoadSection<T>(string sectionName, System.Configuration.Configuration defaultConfig)
            where T: ConfigurationSection, IConfigSection
        {
            var section = (T)_config.GetSection(sectionName);
            if (section == null)
            {
                section = (T)((IConfigSection)defaultConfig.GetSection(sectionName)).Clone();
                section.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
                _config.Sections.Add(sectionName, section);
            }

            return section;
        }
    }
}
