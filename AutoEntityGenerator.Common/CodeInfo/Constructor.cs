using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoEntityGenerator.Common.CodeInfo;

[ExcludeFromCodeCoverage] // There's no logic to test here...
public sealed class Constructor
{
    public Constructor(IEnumerable<Parameter> parameters)
    {
        Parameters = new List<Parameter>(parameters);
    }

    public List<Parameter> Parameters { get; }
}
