using AutoEntityGenerator.Common.CodeInfo;

namespace AutoEntityGenerator.CodeGenerator;

public interface IMappingsClassGenerator
{
    string GenerateMappingClassCode(Entity from, Entity to);
}

internal class MappingsClassGenerator : CodeGeneratorBase, IMappingsClassGenerator
{
    public string GenerateMappingClassCode(Entity from, Entity to)
    {
        var indentationLevel = from.Namespace.IsFileScoped ? 3 : 4;
        var indentation = new string('\t', indentationLevel);
        var properties = GenerateProperties(from.Properties, p => $"{indentation}{p.Name} = source.{p.Name},");

        return GenerateCode(from, to, properties);
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
    public static partial class {from.Name}MappingExtensions
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
