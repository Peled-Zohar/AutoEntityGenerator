using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoEntityGenerator.CodeGenerator
{
    public class CodeFileGenerator : ICodeFileGenerator
    {
        public IGeneratedCodeFile GenerateEntityCodeFile(Entity entityInfo)
        {
            if (entityInfo == null)
            {
                return null;
            }

            var useRecords = entityInfo.Project.CSharpVersion == CSharpVersion.Default
                || (int)entityInfo.Project.CSharpVersion >= (int)CSharpVersion.CSharp9;

            var indentationLevel = entityInfo.Namespace.IsFileScoped ? 1 : 2;

            var typeParameters = entityInfo.TypeParameters.Count > 0
                ? $"<{string.Join(", ", entityInfo.TypeParameters)}>"
                : "";

            var genericConstraints = entityInfo.GenericConstraints.Count > 0
                ? " " + string.Join(" ", entityInfo.GenericConstraints)
                : "";

            var properties = GenerateProperties(entityInfo.Properties, indentationLevel);

            var generatedEntityInfo = new EntityGenerator
            {
                Namespace = entityInfo.Namespace.Name,
                EntityType = useRecords ? "record" : "class",
                EntityName = entityInfo.Name,
                TypeParameters = typeParameters,
                GenericConstraints = genericConstraints,
                Properties = properties
            };

            var code = generatedEntityInfo.GenerateEntityCode(entityInfo.Namespace.IsFileScoped);
            return new GeneratedCodeFile(code, Path.GetFileName(entityInfo.SourceFilePath));
        }

        public IGeneratedCodeFile GenerateMappingCodeFile(Entity from, Entity to)
        {
            if (from is null || to is null)
            {
                return null;
            }

            var indentationLevel = from.Namespace.IsFileScoped ? 3 : 4;

            var typeParameters = to.TypeParameters.Count > 0
               ? $"<{string.Join(", ", to.TypeParameters)}>"
               : "";


            var genericConstraints = to.GenericConstraints.Count > 0
                ? " " + string.Join(" ", to.GenericConstraints)
                : "";

            var mappingProperties = GenerateMappingProperties(from, indentationLevel);

            var generatedMappingsInfo = new MappingsClassGenerator
            {
                FromNamespace = from.Namespace.Name,
                From = from.Name,
                To = to.Name,
                ToNamespace = to.Namespace.Name,
                GenericConstraints = genericConstraints,
                Properties = mappingProperties,
                TypeParameters = typeParameters
            };

            var code = generatedMappingsInfo.GenerateMappingClassCode(from.Namespace.IsFileScoped);

            return new GeneratedCodeFile(code, Path.GetFileNameWithoutExtension(from.SourceFilePath) + "MappingExtensions.cs");
        }

        private string GenerateProperties(IEnumerable<Property> properties, int indentationLevel)
        {
            const string propertyFormat = "public {0} {1} {{get;set;}}";
            var propertiesBuilder = new StringBuilder();
            foreach (var property in properties)
            {
                GenerateIndentation(propertiesBuilder, indentationLevel);
                propertiesBuilder
                    .AppendFormat(propertyFormat, property.Type, property.Name)
                    .AppendLine();
            }
            return propertiesBuilder.ToString(0, propertiesBuilder.Length - 2);
        }

        private string GenerateMappingProperties(Entity from, int indentationLevel)
        {
            const string propertyFormat = "{0} = source.{0},";
            var propertiesBuilder = new StringBuilder();
            foreach (var property in from.Properties)
            {
                GenerateIndentation(propertiesBuilder, indentationLevel);
                propertiesBuilder
                    .AppendFormat(propertyFormat, property.Name)
                    .AppendLine();
            }
            return propertiesBuilder.ToString(0, propertiesBuilder.Length - 2);
        }

        private void GenerateIndentation(StringBuilder sb, int indentationLevel)
        {
            for (var i = 0; i < indentationLevel; i++)
            {
                sb.Append("\t");
            }
        }
    }
}
