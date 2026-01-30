using System.Collections.Generic;
using pvfUtility.Service;

namespace pvfUtility.Model.PvfOperation.Praser
{
    internal class ScriptItemGroup : ScriptItemBase
    {
        public List<ScriptItemBase> Children = new List<ScriptItemBase>();
        public List<string> Description;
        public string GroupName;
        public List<string> Name;

        public override IEnumerable<ScriptItemData> GetData()
        {
            var lst = new List<ScriptItemData>();
            foreach (var item in Children) lst.AddRange(item.GetData());
            return lst;
        }

        public override string ToString()
        {
            return GroupName;
        }
    }

    internal class ScriptSection : ScriptItemBase
    {
        public List<ScriptItemBase> Children = new List<ScriptItemBase>();
        public string Description;
        public int GroupLength;
        public bool HasEndTag;
        public string Name { get; set; }

        public override IEnumerable<ScriptItemData> GetData()
        {
            var lst = new List<ScriptItemData>();
            foreach (var item in Children) lst.AddRange(item.GetData());
            return lst;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj != null && Name == obj.ToString();
        }
    }

    internal class ScriptItemBase
    {
        public ScriptItemData Item;

        public ScriptItemBase()
        {
        }

        public ScriptItemBase(ScriptItemData item)
        {
            Item = item;
        }

        public virtual IEnumerable<ScriptItemData> GetData()
        {
            return new List<ScriptItemData> {Item};
        }

        public override string ToString()
        {
            if (Item.ScriptType == ScriptType.Section || Item.ScriptType == ScriptType.String)
                return PackService.CurrentPack.Strtable.GetStringItem(Item.Data);
            return Item.Data.ToString();
        }
    }

    internal struct ScriptItemData
    {
        public ScriptType ScriptType;
        public int Data;
    }

    internal enum ScriptType
    {
        Int = 2,
        IntEx,
        Float,
        Section,
        Command,
        String,
        CommandSeparator,
        StringLinkIndex,
        StringLink
    }
}