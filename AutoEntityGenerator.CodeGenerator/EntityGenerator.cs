using AutoEntityGenerator.Common.CodeInfo;

namespace AutoEntityGenerator.CodeGenerator;

public interface IEntityGenerator
{
    string GenerateEntityCode(Entity entity);
}

internal class EntityGenerator : CodeGeneratorBase, IEntityGenerator
{
    public string GenerateEntityCode(Entity entity)
    {

        var entityType = entity.Project.CSharpVersion == CSharpVersion.Default
           || (int)entity.Project.CSharpVersion >= (int)CSharpVersion.CSharp9
           ? "record"
           : "class";

        var indentationLevel = entity.Namespace.IsFileScoped ? 1 : 2;
        var indentation = new string('\t', indentationLevel);
        var properties = GenerateProperties(entity.Properties, p => $"{indentation}public {p.Type} {p.Name} {{get;set;}}");

        return GenerateCode(entity, entityType, properties);
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
}}"
            :
$@"{Comments}namespace {entity.Namespace.Name}
{{
    public partial {typeName} {entity.Name}{typeParameters}{genericConstraints}
    {{
{properties}
    }}
}}";
    }
}
