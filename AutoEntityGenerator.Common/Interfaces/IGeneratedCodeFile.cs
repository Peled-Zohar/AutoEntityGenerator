namespace AutoEntityGenerator.Common.Interfaces
{
    public interface IGeneratedCodeFile
    {
        string FileName { get; }
        string Content { get; }
    }
}
