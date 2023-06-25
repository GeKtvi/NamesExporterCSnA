using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Serialization;

namespace NamesExporterCSnA.Services.Settings
{
    public class SettingsSaveLoadManager
    {
        private IPreferencesSettings _settings;
        private string _saveFileName;

        public SettingsSaveLoadManager(IPreferencesSettings settings) 
        {
            _settings = settings;
            _saveFileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                            '\\' + Assembly.GetExecutingAssembly().GetName().Name +
                            '\\' + "PreferencesSettings.config";

            Application.Current.Exit += AppExit;
            Load();
        }

        private void Load()
        {
            if (File.Exists(_saveFileName) == false)
                return;
            using (var fs = new FileStream(_saveFileName, FileMode.Open))
            {
                PropertyHolder<PreferencesSettings>.SetPropertiesValue(
                    (PreferencesSettings)_settings, 
                    (PreferencesSettings)new XmlSerializer(typeof(PreferencesSettings)).Deserialize(fs)
                    ); 
            }
        }

        private void AppExit(object sender, ExitEventArgs e)
        {
            using (var fs = new FileStream(_saveFileName, FileMode.OpenOrCreate))
            {
                new XmlSerializer(typeof(PreferencesSettings)).Serialize(fs, _settings);
            }
        }
    }
}
