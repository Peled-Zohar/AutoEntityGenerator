using System.Diagnostics.CodeAnalysis;

namespace AutoEntityGenerator.Common.CodeInfo
{
    [ExcludeFromCodeCoverage] // There's no logic to test here...
    public sealed class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
