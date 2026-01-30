using System;

namespace pvfUtility.Model
{
    internal class ErrorListItem
    {
        public Guid Guid { get; set; }
        public bool IsExternal { get; set; }
        public string FullText { get; set; }
        public string Action { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public int Line { get; set; }
    }
}