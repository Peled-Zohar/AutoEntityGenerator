using AutoEntityGenerator.Common.Interfaces;
using AutoEntityGenerator.UI.ViewModels;
using FluentValidation;
using System;
using System.IO;
using System.Linq;

namespace AutoEntityGenerator.UI.Validators
{
    internal class AppSettingslValidator : AbstractValidator<IAppSettings>
    {
        public AppSettingslValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(v => v.MinimumLogLevel)
               .IsInEnum()
               .WithMessage("Invalid log level.");

            RuleFor(v => v.DestinationFolder)
               .Must(v => !v.Any(c => Path.GetInvalidPathChars().Contains(c))).WithMessage("Invalid Destination folder.")
               .Must(v => string.IsNullOrWhiteSpace(v) || !Path.IsPathRooted(v.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))).WithMessage("Destination folder must be a relative path.");

            RuleFor(v => v.RequestSuffix)
                .Must(v => string.IsNullOrWhiteSpace(v) || v.All(c => char.IsLetterOrDigit(c) || c == '_')).WithMessage("Request suffix can only contain letters, digits or underscores.");

            RuleFor(v => v.ResponseSuffix)
                .Must(v => string.IsNullOrWhiteSpace(v) || v.All(c => char.IsLetterOrDigit(c) || c == '_')).WithMessage("Response suffix can only contain letters, digits or underscores.");
        }
    }
}
