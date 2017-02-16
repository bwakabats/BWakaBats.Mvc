// *****************************************************
//               MVC EXPANSION - BOOTSTRAP
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
using BWakaBats.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Device.Location;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class GeographicMapMap<TControl> : Map<TControl, object>
        where TControl : GeographicMapMap<TControl>
    {
        private static Dictionary<Type, Tuple<PropertyInfo, PropertyInfo>> _types = new Dictionary<Type, Tuple<PropertyInfo, PropertyInfo>>();

        internal GeographicMapMap(MapContext<object> context, string name)
            : base(context, name)
        {
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            GenerateLocation(Context); // Set Zoom
            tag.MergeNotNullAttribute("data-address-part", "map");
            return base.UpdateTag(tag);
        }

        protected override string WrapTag(TagBuilder tag)
        {
            var location = GenerateLocation(Context);

            return base.WrapTag(tag)
                 + "<input type='hidden' id='" + Context.Id + "_Latitude' name='" + Context.Name + ".Latitude' value='" + location.Latitude + "' />"
                 + "<input type='hidden' id='" + Context.Id + "_Longitude' name='" + Context.Name + ".Longitude' value='" + location.Longitude + "' />";
        }

        protected override string DefaultCssClass
        {
            get { return "map geographicmap"; }
        }

        #region GenerateLocation

        private static ILocation GenerateLocation(MapContext<object> context)
        {
            object value = context.Value;
            if (value == null)
                return new Location();

            var location = value as ILocation;
            if (location != null)
                return location;

            var geoCoordinate = value as GeoCoordinate;
            if (geoCoordinate != null)
            {
                if (geoCoordinate.IsUnknown)
                    return new Location();
                return new Location(geoCoordinate.Latitude, geoCoordinate.Longitude);
            }
            var dbGeography = value as DbGeography;
            if (dbGeography != null)
                return new Location(dbGeography.Latitude, dbGeography.Longitude);

            var tupleNullable = value as Tuple<double?, double?>;
            if (tupleNullable != null)
                return new Location(tupleNullable.Item1, tupleNullable.Item2);

            var tuple = value as Tuple<double, double>;
            if (tuple != null)
                return new Location(tuple.Item1, tuple.Item2);

            var type = value.GetType();
            Tuple<PropertyInfo, PropertyInfo> tupleProperties;
            if (!_types.TryGetValue(type, out tupleProperties))
            {
                lock (_types)
                {
                    if (!_types.TryGetValue(type, out tupleProperties))
                    {
                        PropertyInfo latitudePropertyInfo = type.GetProperty("Latitude", BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        PropertyInfo longitudePropertyInfo = type.GetProperty("Longitude", BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        tupleProperties = Tuple.Create<PropertyInfo, PropertyInfo>(latitudePropertyInfo, longitudePropertyInfo);
                        _types.Add(type, tupleProperties);
                    }
                }
            }

            return new Location(GetLocationValue(tupleProperties.Item1, value), GetLocationValue(tupleProperties.Item2, value));
        }

        private static double? GetLocationValue(PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo == null)
                return null;
            var propertyValue = propertyInfo.GetValue(value);
            if (propertyValue == null)
                return null;

            try
            {
                return Convert.ToDouble(propertyValue, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }

    public sealed class GeographicMap : GeographicMapMap<GeographicMap>
    {
        internal GeographicMap(string name = null) : base(new MapContext<object>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static GeographicMap BootstrapGeographicMap(this HtmlHelper htmlHelper, string name)
        {
            var control = new GeographicMap(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static GeographicMap BootstrapGeographicMapFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new GeographicMap();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
