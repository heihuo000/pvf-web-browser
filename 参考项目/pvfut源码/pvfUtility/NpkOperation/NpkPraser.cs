using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using pvfUtility.Properties;

namespace pvfUtility.NpkOperation
{
    internal struct ImgFile
    {
        public int Offset;
        public int Size;
        public string ImgName;
    }
    internal static class NpkPraser
    {
        
        public static List<ImgFile> PraseNpkFile(string fileName)
        {
            var lst = new List<ImgFile>();
            using (var npkFile = new FileStream(fileName, FileMode.Open))
            {
                var data = new byte[npkFile.Length];
                npkFile.Read(data, 0, data.Length);
                var fileCount = BitConverter.ToInt32(data, 16);
                var pos = 20;
                for (var i = 0; i < fileCount; i++)
                {
                    ImgFile img;
                    img.Offset = BitConverter.ToInt32(data, pos);
                    img.Size = BitConverter.ToInt32(data, pos + 4);
                    var tmp = new byte[256];
                    Buffer.BlockCopy(data, pos + 8, tmp, 0, 256);
                    img.ImgName = Encoding.ASCII.GetString(Decrypt(tmp)).TrimEnd('/').TrimEnd('\0');
                    tmp = null;
                    pos += 264;
                    lst.Add(img);
                    //                    if (hash.Contains(img.ImgName))
                    //                        Console.WriteLine(img.ImgName);
                    //                    else
                    //                        hash.Add(img.ImgName);
                }
                npkFile.Close();
            }

            return lst;
        }

        private static byte[] Decrypt(IReadOnlyList<byte> source)
        {
            var data = new byte[256];
            var password = npk.npk_decrypt_dic;
            for (var i = 0; i < 256; i++)
                data[i] = (byte)(source[i] ^ password[i]);
            return data;
        }
    }
}
