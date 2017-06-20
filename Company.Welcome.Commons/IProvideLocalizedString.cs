using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Welcome.Commons
{
    public interface IProvideLocalizedString
    {
        string GetFor(string key);
    }
}
