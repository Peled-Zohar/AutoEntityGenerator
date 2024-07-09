namespace AutoEntityGenerator.CodeGenerator
{
    internal class EntityGenerator : CodeGenerator
    {
        public string Namespace { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }

        public string GenerateEntityCode(bool isFileScopedNamespace)
        {
            return isFileScopedNamespace
                ?
$@"{Comments}namespace {Namespace};

public partial {EntityType} {EntityName}{TypeParameters}{GenericConstraints}
{{
{Properties}
}}
"
                :
$@"{Comments}namespace {Namespace}
{{
    public partial {EntityType} {EntityName}{TypeParameters}{GenericConstraints}
    {{
{Properties}
    }}
}}
"
;
        }
    }
}
