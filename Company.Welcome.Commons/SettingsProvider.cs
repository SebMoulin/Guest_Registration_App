using System;
using Windows.Storage;

namespace Company.Welcome.Commons
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly object _locker = new object();

        public string GetSetting(string key, string defaultValue = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            lock (_locker)
            {
                var localSettings = ApplicationData.Current.LocalSettings.Values;
                if (localSettings.ContainsKey(key))
                {
                    var valueForKey = localSettings[key].ToString();
                    return !string.IsNullOrWhiteSpace(valueForKey) ? valueForKey : defaultValue;
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        public void SaveSetting(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            lock (_locker)
            {
                var localSettings = ApplicationData.Current.LocalSettings.Values;
                localSettings[key] = value;
            }
        }

        public void DeleteSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            lock (_locker)
            {
                var localSettings = ApplicationData.Current.LocalSettings.Values;
                if (localSettings.ContainsKey(key))
                {
                    localSettings.Remove(key);
                }
            }
        }
    }
}