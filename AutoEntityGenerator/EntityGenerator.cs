using AutoEntityGenerator.Common.CodeInfo;
using AutoEntityGenerator.Common.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoEntityGenerator
{
    internal interface IEntityGenerator
    {
        Task<Entity> GenerateFromDocumentAsync(Document document, TypeDeclarationSyntax typeDeclaration, CancellationToken cancellationToken);

        Entity GenerateFromUIResult(IUserInteractionResult userInteractionResult, Entity sourceEntity);
    }

    internal class EntityGenerator : IEntityGenerator
    {
        private readonly ILogger _logger;

        public EntityGenerator(ILogger logger)
        {
            _logger = logger;
        }

        public Entity GenerateFromUIResult(IUserInteractionResult userInteractionResult, Entity sourceEntity)
        {
            var newNamespace = new Namespace()
            {
                IsFileScoped = sourceEntity.Namespace.IsFileScoped,
                Name = sourceEntity.Project.DefaultNamespace + userInteractionResult.TargetDirectory
                    .Replace(Path.GetDirectoryName(sourceEntity.Project.FilePath), "")
                    .Replace(Path.DirectorySeparatorChar, '.')
                    .Replace(Path.AltDirectorySeparatorChar, '.')
            };

            _logger.Debug("Attempting to create entity based on source entity and user interaction result.");
            return new Entity
            {
                Constructors = Enumerable.Empty<Constructor>().ToList(),
                Name = userInteractionResult.EntityName,
                Namespace = newNamespace,
                Project = sourceEntity.Project,
                Properties = userInteractionResult.EntityProperties,
                SourceFilePath = Path.Combine(userInteractionResult.TargetDirectory, userInteractionResult.FileName),
                GenericConstraints = sourceEntity.GenericConstraints,
                TypeParameters = sourceEntity.TypeParameters,
            };
        }

        public async Task<Entity> GenerateFromDocumentAsync(Document document, TypeDeclarationSyntax typeDeclaration, CancellationToken cancellationToken)
        {
            _logger.Debug("Attempting to get semantic model.");
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
            var typeParameters = typeDeclaration.TypeParameterList is null
                ? Enumerable.Empty<string>()
                : typeDeclaration.TypeParameterList.Parameters.Select(p => p.Identifier.Text);
            var genericConstraints = typeDeclaration.ConstraintClauses.Select(c => c.ToString());

            _logger.Debug("Attempting to create entity from document info.");
            return new Entity
            {
                Constructors = GenerateConstructors(typeSymbol, cancellationToken),
                GenericConstraints = genericConstraints.ToList(),
                Name = typeSymbol.Name,
                Namespace = GenerateNamespace(typeDeclaration),
                Project = GenerateProject(document),
                Properties = GenerateProperties(typeSymbol, cancellationToken),
                SourceFilePath = document.FilePath,
                TypeParameters = typeParameters.ToList()
            };
        }

        private Namespace GenerateNamespace(TypeDeclarationSyntax typeDeclaration)
        {
            var namespaceDeclaration = typeDeclaration.FirstAncestorOrSelf<BaseNamespaceDeclarationSyntax>();

            return new Namespace()
            {
                Name = namespaceDeclaration?.Name.ToString() ?? "",
                IsFileScoped = namespaceDeclaration is FileScopedNamespaceDeclarationSyntax
            };
        }

        private Common.CodeInfo.Project GenerateProject(Document document)
        {
            var lanugageVersion = (int?)((document.Project.ParseOptions as CSharpParseOptions)?.LanguageVersion);
            return new Common.CodeInfo.Project
            {
                DefaultNamespace = document.Project.DefaultNamespace,
                FilePath = document.Project.FilePath,
                CSharpVersion = (CSharpVersion)(lanugageVersion ?? -1)
            };
        }

        private List<Constructor> GenerateConstructors(INamedTypeSymbol typeSymbol, CancellationToken cancellationToken)
        {
            var constructors = typeSymbol.Constructors;
            var constructorInfos = new List<Constructor>();

            foreach (var constructor in constructors)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var parameters = constructor.Parameters
                    .Select(parameter => new Parameter
                    {
                        Name = parameter.Name,
                        Type = parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                    });

                var constructorInfo = new Constructor(parameters);

                constructorInfos.Add(constructorInfo);
            }

            return constructorInfos;
        }

        private List<Property> GenerateProperties(INamedTypeSymbol typeSymbol, CancellationToken cancellationToken)
        {
            var properties = new List<Property>();

            while (typeSymbol != null && typeSymbol.Name != nameof(System.Object))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var classProperties = typeSymbol.GetMembers()
                    .OfType<IPropertySymbol>()
                    .Where(p => p.DeclaredAccessibility == Accessibility.Public)
                    .Select(p => new Property
                    {
                        Name = p.Name,
                        Type = p.Type.ToString(),
                        IsReadonly = p.SetMethod is null || p.SetMethod.DeclaredAccessibility != Accessibility.Public
                    });

                properties.AddRange(classProperties);
                typeSymbol = typeSymbol.BaseType;
            }

            return properties;
        }
    }
}
