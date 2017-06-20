using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Welcome.Commons
{
    public interface IProvideStorage
    {
        Task SaveFileAsync(string filename, string text, bool append);
        Task<string> GetFileAsync(string filename);
    }
}
