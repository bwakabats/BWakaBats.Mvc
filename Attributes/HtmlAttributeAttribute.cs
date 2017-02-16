// *****************************************************
//                     MVC EXPANSION
//                  by  Shane Whitehead
//                  bwakabats@gmail.com
// *****************************************************
//      The software is released under the GNU GPL:
//          http://www.gnu.org/licenses/gpl.txt
//
// Feel free to use, modify and distribute this software
// I only ask you to keep this comment intact.
// Please contact me with bugs, ideas, modification etc.
// *****************************************************
using System;
using System.Diagnostics.CodeAnalysis;

namespace BWakaBats.Attributes
{
    public abstract class HtmlAttributeAttribute : Attribute, IDataAnnotationAttribute
    {
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected HtmlAttributeAttribute(string value)
        {
            Key = GetType().Name;
            Key = Key.Substring(0, Key.Length - 9).ToString().ToLowerInvariant();
            Value = value;
        }

        public string Key { get; protected set; }

        public string Value { get; protected set; }

    }
}
