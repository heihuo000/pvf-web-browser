using System;
using pvfUtility.Model.PvfOperation;

namespace pvfUtility.Interface.View
{
    public interface IEditor
    {
        /// <summary>
        ///     文件名
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        ///     是否当前pvf内部文件
        /// </summary>
        bool IsExternalFile { get; set; }

        /// <summary>
        ///     外部目录使用
        /// </summary>
        Guid ExternalContentId { get; set; }

        /// <summary>
        ///     编辑器编码格式
        /// </summary>
        EncodingType EncodingType { get; set; }

        /// <summary>
        ///     是否被修改
        /// </summary>
        bool IsEdited { get; set; }

        /// <summary>
        ///     内容
        /// </summary>
        string Content { get; set; }

        /// <summary>
        ///     保存文件
        /// </summary>
        void SaveFile();

        /// <summary>
        ///     转到
        /// </summary>
        bool GoTo(int line);

        /// <summary>
        ///     设置文本
        /// </summary>
        /// <param name="text"></param>
        void SetText(string text);
    }
}