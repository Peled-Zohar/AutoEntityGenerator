using AutoEntityGenerator.Common.CodeInfo;
using System.Text;

namespace AutoEntityGenerator.CodeGenerator
{
    public interface IMappingsClassGenerator
    {
        string GenerateMappingClassCode(Entity from, Entity to);
    }

    internal class MappingsClassGenerator : CodeGenerator, IMappingsClassGenerator
    {
        public string GenerateMappingClassCode(Entity from, Entity to)
        {
            var properties = GenerateMappingProperties(from);

            return GenerateCode(from, to, properties);
        }

        private string GenerateMappingProperties(Entity from)
        {
            const string propertyFormat = "{0} = source.{0},";

            var indentationLevel = from.Namespace.IsFileScoped ? 3 : 4;
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

        private string GenerateCode(Entity from, Entity to, string properties)
        {

            var typeParameters = GenerateTypeParameters(to);
            var genericConstraints = GenerateGenericConstraints(to);

            var toFullName = string.IsNullOrEmpty(to.Namespace.Name)
                ? to.Name
                : to.Namespace.Name + "." + to.Name;

            return from.Namespace.IsFileScoped
                ?
$@"{Comments}namespace {from.Namespace.Name};

public static partial class {from.Name}MappingExtensions
{{
    public static {toFullName}{typeParameters} To{to.Name}{typeParameters}(this {from.Name}{typeParameters} source){genericConstraints}
    {{
        return new {toFullName}{typeParameters}
        {{
{properties}
        }};
    }}
}}"
                :
$@"{Comments}namespace {from.Namespace.Name}
{{
    public static partial class {from.Name}MappingExtension
    {{
        public static {toFullName}{typeParameters} To{to.Name}{typeParameters}(this {from.Name}{typeParameters} source){genericConstraints}
        {{
            return new {toFullName}{typeParameters}
            {{
{properties}
            }};
        }}  
    }}
}}";
        }
    }
}
