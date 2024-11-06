namespace EntityClassLib
{
    /// <summary>
    /// 基于时间戳的36进制编码
    /// </summary>
    public static class TimeEncode
    {
        private static readonly char[] Base36Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static string EncodeNow() => EncodeTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        // 将时间戳编码为36进制字符串
        public static string EncodeTimestamp(long timestamp)
        {
            if (timestamp == 0) return "0";

            var result = new System.Text.StringBuilder();
            while (timestamp > 0)
            {
                result.Insert(0, Base36Characters[timestamp % 36]);
                timestamp /= 36;
            }
            return result.ToString();
        }

        // 通过编码后的字符串反推时间戳
        // 转换输入字符串为大写以支持不分区大小写
        public static long DecodeTimestamp(string encoded)
        {
            // 转换为大写以确保不区分大小写
            encoded = encoded.ToUpper();
            return encoded.Aggregate<char, long>(0, (current, c) => current * 36 + Array.IndexOf(Base36Characters, c));
        }
    }
}
