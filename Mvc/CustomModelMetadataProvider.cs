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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using BWakaBats.Attributes;

namespace BWakaBats.Mvc
{
    public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        private Func<string, string> CreateDisplayName;

        public CustomModelMetadataProvider(Func<string, string> createDisplayName)
        {
            CreateDisplayName = createDisplayName;
        }

        public CustomModelMetadataProvider(IDictionary<string, string> createDisplayNamePairs)
            : this(displayName =>
            {
                displayName = System.Text.RegularExpressions.Regex.Replace(displayName, "(?<!(^|[A-Z0-9]))(?=[A-Z0-9])|(?<!^)(?=[A-Z0-9][a-z])", " $1") + " ";
                foreach (var keyValue in createDisplayNamePairs)
                {
                    displayName = displayName.Replace(keyValue.Key, keyValue.Value);
                }
                displayName = displayName.Replace("  ", " ");
                displayName = displayName.Trim();
                return displayName;
            })
        {
        }

        public CustomModelMetadataProvider()
            : this(new Dictionary<string, string> {
                { " Id ", " " }, { " Date Time ", " " }, { " Date ", " " }, { " Time ", " " }, { " Html ", " " },
                { " 1 ", " One "}, { " 2 ", " Two "}, { " 3 ", " Three "}, { " 4 ", " Four "},{ " 5 ", " Five "},
                { " 6 ", " Six "}, { " 7 ", " Seven "}, { " 8 ", " Eight "}, { " 9 ", " Nine "},
            })
        {
        }


        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var data = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            var dataAnnotationAttributes = attributes
                .Where(a => a is IDataAnnotationAttribute)
                .Select(a => (IDataAnnotationAttribute)a);

            foreach (var dataAnnotationAttribute in dataAnnotationAttributes)
            {
                data.AdditionalValues.Add(dataAnnotationAttribute.Key, dataAnnotationAttribute);
            }

            if (propertyName != null)
            {
                string displayName = CreateDisplayName(propertyName);
                if (displayName != propertyName)
                {
                    if (!attributes.OfType<DisplayAttribute>().Any())
                    {
                        if (!attributes.OfType<DisplayNameAttribute>().Any())
                        {
                            data.DisplayName = displayName;
                            data.ShortDisplayName = displayName;
                        }
                    }
                }
            }

            return data;
        }

    }
}