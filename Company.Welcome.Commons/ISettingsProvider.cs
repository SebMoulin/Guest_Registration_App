using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Welcome.Commons
{
    public interface ISettingsProvider
    {
        string GetSetting(string key, string defaultValue = null);
        void SaveSetting(string key, string value);
        void DeleteSetting(string key);
    }
}
