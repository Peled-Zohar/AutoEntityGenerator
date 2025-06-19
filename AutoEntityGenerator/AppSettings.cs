using AutoEntityGenerator.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator
{
    internal class AppSettings : IAppSettings
    {
        public LogLevel MinimumLogLevel { get; set; }
        public string DestinationFolder { get; set; }
        public string RequestSuffix { get; set; }
        public string ResponseSuffix { get; set; }
        public bool OpenGeneratedFiles { get; set; }
    }
}
