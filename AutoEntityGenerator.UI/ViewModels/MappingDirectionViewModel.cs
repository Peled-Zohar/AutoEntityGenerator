using AutoEntityGenerator.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace AutoEntityGenerator.UI.ViewModels
{
    [ExcludeFromCodeCoverage] // There's no logic to test here...
    public class MappingDirectionViewModel
    {
        public MappingDirectionViewModel(string name, MappingDirection value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public MappingDirection Value { get; }
    }
}
