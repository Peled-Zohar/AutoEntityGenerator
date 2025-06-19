using Microsoft.Extensions.Logging;

namespace AutoEntityGenerator.Common.Interfaces;

public interface IAppSettings
{
    string DestinationFolder { get; set; }
    LogLevel MinimumLogLevel { get; set; }
    string RequestSuffix { get; set; }
    string ResponseSuffix { get; set; }
    bool OpenGeneratedFiles { get; set; }
}
