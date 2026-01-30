// ReSharper disable InconsistentNaming

using System.Reflection;

namespace pvfUtility.Model.PvfOperation
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public enum EncodingType
    {
        TW = 950,
        CN = 936,
        KR = 949,
        JP = 932,
        UTF8 = 65001,
        Unicode = 1200
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public enum FileOperation
    {
        OverWrite,
        Rename,
        Skip,
        Cancel
    }
}