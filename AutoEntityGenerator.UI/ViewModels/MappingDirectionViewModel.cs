using AutoEntityGenerator.Common.Interfaces;

namespace AutoEntityGenerator.UI.ViewModels
{
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
