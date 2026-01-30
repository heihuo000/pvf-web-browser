using pvfUtility.Model;

namespace pvfUtility.Action.Batch
{
    internal class BatchCmd
    {
        public BatchMode BatchMode;
        public FileCollectionData FileCollection;
        public bool IsDelContent;

        public string TextData;

        public string TextDataReplace;
        public string TextDelContent;
        public string TextDelSection;
    }
}