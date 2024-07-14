using AutoEntityGenerator.Common.CodeInfo;
using System.Collections.Generic;
using System.Text;

namespace AutoEntityGenerator.CodeGenerator
{
    public interface IEntityGenerator
    {
        string GenerateEntityCode(Entity entity);
    }

    internal class EntityGenerator : CodeGenerator, IEntityGenerator
    {
        public string GenerateEntityCode(Entity entity)
        {

            var entityType = entity.Project.CSharpVersion == CSharpVersion.Default
               || (int)entity.Project.CSharpVersion >= (int)CSharpVersion.CSharp9
               ? "record"
               : "class";

            var indentationLevel = entity.Namespace.IsFileScoped ? 1 : 2;

            var properties = GenerateProperties(entity.Properties, indentationLevel);

            return GenerateCode(entity, entityType, properties);
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

        private string GenerateCode(Entity entity, string typeName, string properties)
        {
            var typeParameters = GenerateTypeParameters(entity);
            var genericConstraints = GenerateGenericConstraints(entity);

            return entity.Namespace.IsFileScoped
                ?
$@"{Comments}namespace {entity.Namespace.Name};

public partial {typeName} {entity.Name}{typeParameters}{genericConstraints}
{{
{properties}
}}
"
                :
$@"{Comments}namespace {entity.Namespace.Name}
{{
    public partial {typeName} {entity.Name}{typeParameters}{genericConstraints}
    {{
{properties}
    }}
}}
";
        }

    }
}

