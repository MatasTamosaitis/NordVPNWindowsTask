using System;
using System.Diagnostics.CodeAnalysis;

namespace NordVPNModels.Models
{
    [ExcludeFromCodeCoverage]
    public class LogModel
    {
        public string Action { get; set; }
        public DateTime Time { get; set; }
    }
}
