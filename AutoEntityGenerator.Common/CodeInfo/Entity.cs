using System.Collections.Generic;

namespace AutoEntityGenerator.Common.CodeInfo
{
    public class Entity
    {
        public Project Project { get; set; }
        public Namespace Namespace { get; set; }
        public string Name { get; set; }
        public List<Constructor> Constructors { get; set; }
        public string SourceFilePath { get; set; }
        public List<Property> Properties { get; set; }
        public List<string> TypeParameters { get; set; }
        public List<string> GenericConstraints { get; set; }
    }
}
