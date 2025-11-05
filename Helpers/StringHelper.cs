using System;
using System.Text;

namespace BTL.Helpers
{
    public static class StringHelper
    {
        // Mã hóa Base64
        public static string EncodeBase64(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

        // Giải mã Base64
        public static string DecodeBase64(string encodedText)
        {
            if (string.IsNullOrEmpty(encodedText))
                return encodedText;
            var base64Bytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(base64Bytes);
        }
    }
}
