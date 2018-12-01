using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CallApiInvoice.Utils
{
    public static class StringHelpers
    {

        public static string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string GetRtfUnicodeEscapedString(string s)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (c <= 0x7f)
                    sb.Append(c);
                else
                    sb.Append("\\u" + Convert.ToUInt32(c) + "?");
            }
            return sb.ToString();
        }
        public static string beutifer(String jsonString)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(jsonString);
            var beautified = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
           
            return beautified;
        }

        public static string minifier(string jsonString)
        {
            JToken parsedJson = JToken.Parse(jsonString);
            var minified = parsedJson.ToString(Formatting.None);
            return minified;
        }
    }
}
