using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NotebookSecond.Data
{
    public static class UnicodeExtensions
    {
        private static readonly Regex Regex = new Regex(@"\\[uU]([0-9A-Fa-f]{4})");

        public static string UnescapeUnicode(this string str)
        {
            return Regex.Replace(str,
                match => ((char)int.Parse(match.Value.Substring(2),
                    NumberStyles.HexNumber)).ToString());
        }
    }
}
