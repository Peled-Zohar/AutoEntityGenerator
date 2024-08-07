using AutoEntityGenerator.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
