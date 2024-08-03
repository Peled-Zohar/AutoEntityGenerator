using System.Linq;

namespace AutoEntityGenerator.UI
{
    internal class IdentifierValidator
    {
        public static bool IsValidClassName(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return false;
            }

            if (!char.IsLetter(className[0]) && className[0] != '_')
            {
                return false;
            }

            if (className.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
            {
                return false;
            }

            if (IsReservedKeyword(className))
            {
                return false;
            }

            if (IsPrimitiveType(className))
            {
                return false;
            }

            return true;
        }

        private static bool IsReservedKeyword(string name)
        {
            string[] keywords = new string[]
            {
                "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char",
                "checked", "class", "const", "continue", "decimal", "default", "delegate", "do",
                "double", "else", "enum", "event", "explicit", "extern", "false", "finally",
                "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int",
                "interface", "internal", "is", "lock", "long", "namespace", "new", "null",
                "object", "operator", "out", "override", "params", "private", "protected", "public",
                "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc",
                "static", "string", "struct", "switch", "this", "throw", "true", "try",
                "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual",
                "void", "volatile", "while"
            };

            return keywords.Contains(name);
        }

        private static bool IsPrimitiveType(string name)
        {
            string[] primitiveTypes = new string[]
            {
                "bool", "byte", "sbyte", "char", "decimal", "double", "float", "int", "uint",
                "long", "ulong", "short", "ushort", "string", "object", "void"
            };

            return primitiveTypes.Contains(name);
        }
    }
}
