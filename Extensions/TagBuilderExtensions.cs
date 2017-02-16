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
using System.Collections.Generic;
using System.Web.Mvc;

namespace BWakaBats.Extensions
{
    public static class TagBuilderExtensions
    {
        public static void AppendAttribute(this TagBuilder source, string name, object value, string separator = ";")
        {
            var attributes = source.Attributes;
            string existingAttribute;
            if (attributes.TryGetValue(name, out existingAttribute))
            {
                attributes.Remove(name);
                attributes.Add(name, existingAttribute + separator + value.ToString());
            }
            else
            {
                attributes.Add(name, value.ToString());
            }
        }

        public static void MergeAttributes(this TagBuilder source, object htmlAttributes)
        {
            if (htmlAttributes == null)
                return;

            var attributeDictionary = htmlAttributes as IDictionary<string, object>;
            if (attributeDictionary == null)
            {
                attributeDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            }
            AppendAttribute(source.Attributes, attributeDictionary, "style", ";");
            AppendAttribute(source.Attributes, attributeDictionary, "class", " ");
            source.MergeAttributes(attributeDictionary);
        }


        public static bool MergeNotNullAttribute(this TagBuilder tag, string name, object value)
        {
            if (value == null)
                return false;

            tag.MergeIfAttribute(name, value, value != null);
            return true;
        }

        public static bool MergeIfAttribute(this TagBuilder tag, string name, object value, bool addIt)
        {
            if (addIt)
            {
                tag.MergeAttribute(name, value.ToString());
            }
            return addIt;
        }

        private static void AppendAttribute(IDictionary<string, string> attributes, IDictionary<string, object> attributeDictionary, string key, string separator)
        {
            string existingAttribute;
            if (attributes.TryGetValue(key, out existingAttribute))
            {
                object newAttribute;
                if (attributeDictionary.TryGetValue(key, out newAttribute))
                {
                    attributeDictionary.Remove(key);
                    attributes.Remove(key);
                    attributes.Add(key, existingAttribute + separator + newAttribute.ToString());
                }
            }
        }
    }
}