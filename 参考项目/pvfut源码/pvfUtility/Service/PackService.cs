using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;

namespace pvfUtility.Service
{
    internal static class PackService
    {
        public static bool IsProcessing = false;

        /// <summary>
        ///     current editing pvf
        /// </summary>
        public static PvfPack CurrentPack { get; set; }

        /// <summary>
        ///     当前pvf的路径
        /// </summary>
        public static string CurrentPackPathFolder
        {
            get
            {
                if (Config.Instance.ExtractDefaultPath != "")
                    return Config.Instance.ExtractDefaultPath;
                return CurrentPack != null
                    ? CurrentPack.PvfPackFilePath.Remove(CurrentPack.PvfPackFilePath.Length - 4, 4)
                    : "";
            }
        }

        /// <summary>
        ///     overall encoding type
        /// </summary>
        public static EncodingType CurrentEncodingType
        {
            get;
            set;
            /*                if (CurrentPack == null)
                {
                    return;
                }

                lock (CurrentPack)
                {
                    CurrentPack.OverAllEncodingType = value;
                    if (CurrentPack.Strtable.IsStringTableUpdated) //修复切换编码bug
                    {
                        CurrentPack.GetFile("stringtable.bin")?.WriteFileData(CurrentPack.Strtable.CreateStringTable());
                    }
                    CurrentPack.Strtable.Loadstringtable(CurrentPack.GetFile("stringtable.bin").Data, CurrentPack.OverAllEncodingType);
                    CurrentPack.Strview.InitStringData(CurrentPack.GetFile("n_string.lst"), CurrentPack, CurrentPack.OverAllEncodingType);
                }*/
        }
    }
}