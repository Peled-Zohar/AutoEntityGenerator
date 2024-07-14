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
            var indentationLevel = from.Namespace.IsFileScoped ? 3 : 4;

            var properties = GenerateMappingProperties(from, indentationLevel);

            return GenerateCode(from, to, properties);
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

        private string GenerateCode(Entity from, Entity to, string properties)
        {

            var typeParameters = GenerateTypeParameters(to);
            var genericConstraints = GenerateGenericConstraints(to);

            return from.Namespace.IsFileScoped
                ?
$@"{Comments}namespace {from.Namespace.Name};

public static partial class {from.Name}MappingExtensions
{{
    public static {to.Namespace.Name}.{to.Name}{typeParameters} To{to.Name}{typeParameters}(this {from.Name}{typeParameters} source){genericConstraints}
    {{
        return new {to.Namespace.Name}.{to.Name}{typeParameters}
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
        public static {to.Namespace.Name}.{to.Name}{typeParameters} To{to.Name}{typeParameters}(this {from.Name}{typeParameters} source){genericConstraints}
        {{
            return new {to.Namespace.Name}.{to.Name}{typeParameters}
            {{
{properties}
            }};
        }}  
    }}
}}";
        }
    }
}
