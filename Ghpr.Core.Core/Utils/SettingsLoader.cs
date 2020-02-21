using System;
using System.IO;
using Newtonsoft.Json;

namespace Ghpr.Core.Utils
{
    public static class SettingsLoader
    {
        public static T LoadSettingsAs<T>(this string fileName)
        {
            var settings = default(T);
            var settingsPath = "";
            Exception exception = null;
            try
            {
                var uri = new Uri(typeof(T).Assembly.CodeBase);
                settingsPath = Path.Combine(Path.GetDirectoryName(uri.LocalPath) ?? "", fileName);
                var fileContent = File.ReadAllText(settingsPath);
                settings = JsonConvert.DeserializeObject<T>(fileContent, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                });
            }
            catch (Exception e)
            {
                exception = e;
            }

            if (exception != null)
            {
                throw new ApplicationException($"Unable to read the settings from file '{fileName}'. " +
                                               $"Full path: '{settingsPath}'", exception);
            }
            return settings;
        }
    }
}