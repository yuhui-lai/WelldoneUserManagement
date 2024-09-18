using System.Text.Encodings.Web;

namespace WUM.Lib.Utilities
{
    public static class StringUtil
    {
        public static string Purify(this string str, HtmlEncoder encoder)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return encoder.Encode(str.Trim());
        }

        public static string SafeTrim(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return str.Trim();
        }
    }
}
