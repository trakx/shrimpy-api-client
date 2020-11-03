using System;

namespace Trakx.Shrimpy.ApiClient.Utils
{
    public static class StringExtensions
    {
        public static string ToHexString(this byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "").ToLower();
        }
    }
}
