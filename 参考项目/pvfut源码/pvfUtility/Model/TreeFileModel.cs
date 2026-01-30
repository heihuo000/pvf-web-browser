using System.Collections.Generic;

namespace pvfUtility.Model
{
    /// <summary>
    ///     树形文件结构
    /// </summary>
    internal struct TreeFileModel
    {
        public Dictionary<string, TreeFileModel> List;
    }
}