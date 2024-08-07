using FluentValidation;
using System;
using System.IO;
using System.Linq;

namespace AutoEntityGenerator.UI.ViewModels
{
    internal class EntityConfigurationViewModelValidator : AbstractValidator<EntityConfigurationViewModel>
    {
        private readonly string _projectDirectory;
        public EntityConfigurationViewModelValidator(string projectDirectory)
        {
            _projectDirectory = projectDirectory;

            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.DtoName)
                .Must(v => !string.IsNullOrEmpty(v)).WithMessage("Dto name is required")
                .Must(v => IsValidIdentifier(v)).WithMessage("Invalid Dto name.");

            RuleFor(vm => vm.DestinationFolder)
                .Must(folder => !string.IsNullOrWhiteSpace(folder)).WithMessage("Destination folder is required")
                .Must(folder => !folder.Any(c => Path.GetInvalidPathChars().Contains(c))).WithMessage("Invalid Destination folder path.")
                .Must(folder => folder.StartsWith(_projectDirectory, StringComparison.OrdinalIgnoreCase)).WithMessage("Destination folder must be a subfolder of the project folder.");

            RuleFor(vm => vm.Properties)
                .Must(v => v.Any(p => p.IsSelected)).WithMessage("You must check at least one property.");

            // TODO: Add missing validation rules
        }

        

        private bool IsValidIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return false;
            }

            if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
            {
                return false;
            }

            if (identifier.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
            {
                return false;
            }

            if (IsReservedKeyword(identifier))
            {
                return false;
            }

            if (IsPrimitiveType(identifier))
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
