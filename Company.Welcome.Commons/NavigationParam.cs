using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Welcome.Commons
{
    public interface INavigationParams
    {
        void AddParam(string key, object navogationParam);
        TRetrivedObject GetNavigationParam<TRetrivedObject>(string key, TRetrivedObject defaultValue);
    }

    public class NavigationParams : INavigationParams
    {
        private readonly Dictionary<string, object> _navigationParameters;

        public NavigationParams()
        {
            _navigationParameters = new Dictionary<string, object>();
        }

        public void AddParam(string key, object navogationParam)
        {
            _navigationParameters.Add(key, navogationParam);
        }

        public TRetrivedObject GetNavigationParam<TRetrivedObject>(string key, TRetrivedObject defaultValue = default(TRetrivedObject))
        {
            if (!_navigationParameters.ContainsKey(key))
                return defaultValue;
            var value =(TRetrivedObject)_navigationParameters[key];
            return value;
        }
    }


}
