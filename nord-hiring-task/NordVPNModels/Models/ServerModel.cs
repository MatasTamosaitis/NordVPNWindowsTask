
using System.Diagnostics.CodeAnalysis;

namespace NordVPNModels.Models
{
    [ExcludeFromCodeCoverage]
    public class ServerModel
    {
        public string Name { get; set; }
        public int Load { get; set; }
        public string Status { get; set; }
    }
}
