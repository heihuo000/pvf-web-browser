using System;

namespace pvfUtility.Helper
{
    /// <summary>
    ///     pvf核心算法
    /// </summary>
    internal static class PvfAlgorithmHelper
    {
        /// <summary>
        ///     字典存储位置
        /// </summary>
        private static readonly uint[] _checksumDic;

        /// <summary>
        ///     生成字典
        /// </summary>
        static PvfAlgorithmHelper()
        {
            _checksumDic = new uint[256];
            uint num1 = 1;
            uint num2 = 128;
            while (num2 > 0U)
            {
                var num3 = ((int) num1 & 1) == 0 ? 0U : 3988292384U;
                num1 = (num1 >> 1) ^ num3;
                uint num4 = 0;
                var num5 = num2;
                var num6 = num2 * 2U;
                do
                {
                    var num7 = _checksumDic[num4] ^ num1;
                    _checksumDic[num5] = num7;
                    var num8 = num2 * 2U;
                    num5 += num8;
                    num4 += num6;
                } while (num4 < 256U);

                num2 /= 2U;
            }
        }

        /// <summary>
        ///     生成checksum
        /// </summary>
        /// <param name="sourceBytes"></param>
        /// <param name="trueLen"></param>
        /// <param name="fileNameBytesHash"></param>
        /// <returns></returns>
        public static uint CreateBuffKey(byte[] sourceBytes, int trueLen, uint fileNameBytesHash)
        {
            var num1 = ~fileNameBytesHash;
            var index = 0;
            while (index < trueLen)
            {
                var num2 = (uint) ((sourceBytes[index] ^ (int) num1) & byte.MaxValue);
                var num3 = (num1 >> 8) ^ _checksumDic[num2];
                var num4 = (uint) (((int) num3 ^ sourceBytes[index + 1]) & byte.MaxValue);
                var num5 = (num3 >> 8) ^ _checksumDic[num4];
                var num6 = (uint) (((int) num5 ^ sourceBytes[index + 2]) & byte.MaxValue);
                var num7 = (num5 >> 8) ^ _checksumDic[num6];
                var num8 = (uint) (((int) num7 ^ sourceBytes[index + 3]) & byte.MaxValue);
                num1 = (num7 >> 8) ^ _checksumDic[num8];
                index += 4;
            }

            return ~num1;
        }

        /// <summary>
        ///     解密字节集
        /// </summary>
        /// <param name="sourceBytes"></param>
        /// <param name="len"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        public static byte[] DecryptionPvf(byte[] sourceBytes, int len, uint checksum)
        {
            const uint key = 2175242257;
            var decryptedBytes = new byte[len];
            for (var i = 0; i < len; i += 4)
                Buffer.BlockCopy(
                    BitConverter.GetBytes(RotateRight(BitConverter.ToUInt32(sourceBytes, i) ^ key ^ checksum, 6)), 0,
                    decryptedBytes, i, 4);

            return decryptedBytes;
        }

        private static uint RotateRight(uint uint0, int len)
        {
            return (uint0 >> len) | (uint0 << (32 - len));
        }

        /// <summary>
        ///     加密字节集
        /// </summary>
        /// <param name="sourceBytes"></param>
        /// <param name="len"></param>
        /// <param name="checksum"></param>
        /// <returns></returns>
        public static byte[] EncryptionPvf(byte[] sourceBytes, int len, uint checksum)
        {
            const uint key = 2175242257;
            var truelen = (len + 3) & -4;
            var encryptedBytes = new byte[truelen];
            for (var i = 0; i < truelen; i += 4)
                Buffer.BlockCopy(
                    BitConverter.GetBytes(RotateLeft(BitConverter.ToUInt32(sourceBytes, i), 6) ^ checksum ^ key), 0,
                    encryptedBytes, i, 4);
            return encryptedBytes;
        }

        private static uint RotateLeft(uint uint0, int len)
        {
            return (uint0 << len) | (uint0 >> (32 - len));
        }
    }
}