using AutoEntityGenerator.Common.Interfaces;

namespace AutoEntityGenerator.CodeGenerator
{
    internal class GeneratedCodeFile : IGeneratedCodeFile
    {
        public GeneratedCodeFile(string content, string fileName)
        {
            Content = content;
            FileName = fileName;
        }

        public string FileName { get; }

        public string Content { get; }

    }
}
