using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AutoEntityGenerator.UI.ViewModels
{
    internal class EntityConfigurationViewModelValidator : AbstractValidator<EntityConfigurationViewModel>
    {

        private readonly static HashSet<string> _keywords = new HashSet<string>()
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

        private readonly static HashSet<string> _primitiveTypes = new HashSet<string>()
        {
            "bool", "byte", "sbyte", "char", "decimal", "double", "float", "int", "uint",
            "long", "ulong", "short", "ushort", "string", "object", "void"
        };

        public EntityConfigurationViewModelValidator(string projectFolder)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.DtoName)
                .Must(v => !string.IsNullOrWhiteSpace(v)).WithMessage("Dto name is required.")
                .Must(v => char.IsLetter(v[0]) || v[0] == '_').WithMessage("Dto name must start with a letter or underscore.")
                .Must(v => v.All(c => char.IsLetterOrDigit(c) || c == '_')).WithMessage("Dto name can only contain letters, digits or underscores.")
                .Must(v => !_keywords.Contains(v)).WithMessage("Dto name can't be a reserved keyword.")
                .Must(v => !_primitiveTypes.Contains(v)).WithMessage("Dto name can't be the name of a predefined primitive type.");

            RuleFor(vm => vm.GeneratedFileName)
                .Must(v => !string.IsNullOrEmpty(v)).WithMessage("Generated file name is required.")
                .Must(v => !v.Any(c => Path.GetInvalidPathChars().Contains(c))).WithMessage("Invalid generated file name.")
                .Must(v => v.EndsWith(EntityConfigurationViewModel.Extension)).WithMessage("Invalid generated file extension.");

            RuleFor(vm => vm.DestinationFolder)
                 .Must(v => !v.Any(c => Path.GetInvalidPathChars().Contains(c))).WithMessage("Invalid Destination folder.")
                 .Must(v => string.IsNullOrWhiteSpace(v) || !Path.IsPathRooted(v.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))).WithMessage("Destination folder must contain a relative path.");

            RuleFor(vm => vm.DestinationPath)
                .Must(v => string.IsNullOrWhiteSpace(v) || v.StartsWith(projectFolder)).WithMessage("Destination folder must be a sub folder of the project folder.");

            RuleFor(vm => vm.Properties)
                .Must(v => v.Any(p => p.IsSelected)).WithMessage("You must check at least one property.");
        }
    }
}
