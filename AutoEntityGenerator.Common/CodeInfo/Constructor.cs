using System.Collections.Generic;

namespace AutoEntityGenerator.Common.CodeInfo
{
    public class Constructor
    {
        public Constructor(IEnumerable<Parameter> parameters)
        {
            Parameters = new List<Parameter>(parameters);
        }

        public List<Parameter> Parameters { get; }
    }
}
