namespace AutoEntityGenerator.CodeGenerator
{
    internal class MappingsClassGenerator : CodeGenerator
    {
        public string ToNamespace { get; set; }
        public string FromNamespace { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public string GenerateMappingClassCode(bool isFileScopedNamespace)
        {
            return isFileScopedNamespace
                ?
$@"{Comments}namespace {FromNamespace};

public static partial class {From}MappingExtensions
{{
    public static {ToNamespace}.{To}{TypeParameters} To{To}{TypeParameters}(this {From}{TypeParameters} source){GenericConstraints}
    {{
        return new {ToNamespace}.{To}{TypeParameters}
        {{
{Properties}
        }};
    }}
}}"
                :
$@"{Comments}namespace {FromNamespace}
{{
    public static partial class {From}MappingExtension
    {{
        public static {ToNamespace}.{To}{TypeParameters} To{To}{TypeParameters}(this {From}{TypeParameters} source){GenericConstraints}
        {{
            return new {ToNamespace}.{To}{TypeParameters}
            {{
{Properties}
            }};
        }}  
    }}
}}";

        }
    }
}
