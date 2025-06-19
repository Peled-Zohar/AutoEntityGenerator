namespace AutoEntityGenerator.Common.CodeInfo;

public enum CSharpVersion
{
    Unknown = -1,
    Default = 0,
    Latest = int.MaxValue,
    LatestMajor = int.MaxValue - 2,
    PreviewLatestMinor = int.MaxValue - 1,
    CSharp1 = 1,
    CSharp2 = 2,
    CSharp3 = 3,
    CSharp4 = 4,
    CSharp5 = 5,
    CSharp6 = 6,
    CSharp7 = 7,
    CSharp7_1 = 701,
    CSharp7_2 = 702,
    CSharp7_3 = 703,
    CSharp8 = 800,
    CSharp9 = 900,
    CSharp10 = 1000,
    CSharp11 = 1100,
    CSharp12 = 1200,
}
