namespace Company.Welcome.Commons
{
    public class LocalizedStringProvider : IProvideLocalizedString
    {
        public string GetFor(string key)
        {
            var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var localizedValue = resourceLoader.GetString(key);
            return string.IsNullOrWhiteSpace(localizedValue) ? string.Empty : localizedValue;
        }
    }
}