﻿namespace AutoEntityGenerator.Common.CodeInfo;

public sealed class Project
{
    public string FilePath { get; set; }
    public string DefaultNamespace { get; set; }
    public CSharpVersion CSharpVersion { get; set; }
}
