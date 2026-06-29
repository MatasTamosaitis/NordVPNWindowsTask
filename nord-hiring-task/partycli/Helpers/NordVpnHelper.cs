using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partycli.Helpers
{
    public static class NordVpnHelper
    {
        public static string ProccessName(string name)
        {
            name = name.Replace("-", string.Empty);
            return name;
        }
    }
}
