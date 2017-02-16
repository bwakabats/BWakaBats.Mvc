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
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class Map<TControl, TValue> : BoundControl<TControl, TValue>
        where TControl : Map<TControl, TValue>
    {
        internal Map(MapContext<TValue> context, string name)
            : base(context, name, true)
        {
            Context = context;
        }

        public new MapContext<TValue> Context { get; private set; }

        #region Control Properties

        public TControl HeightPercentage(double newValue)
        {
            Context.HeightPercentage = newValue;
            return (TControl)this;
        }


        public TControl Zoom(double newValue)
        {
            Context.Zoom = newValue;
            return (TControl)this;
        }

        public TControl MinZoom(double newValue)
        {
            Context.MinZoom = newValue;
            return (TControl)this;
        }

        public TControl MaxZoom(double newValue)
        {
            Context.MaxZoom = newValue;
            return (TControl)this;
        }

        public TControl CreateLayerFunction(string newValue)
        {
            Context.CreateLayerFunction = newValue;
            return (TControl)this;
        }

        public TControl OnLoadFunction(string newValue)
        {
            Context.OnLoadFunction = newValue;
            return (TControl)this;
        }

        #endregion

        protected override string TagType
        {
            get { return "div"; }
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("style", "padding-bottom: " + Context.HeightPercentage + "%");
            tag.MergeNotNullAttribute("data-map-zoom", Context.Zoom);
            tag.MergeNotNullAttribute("data-map-minzoom", Context.MinZoom);
            tag.MergeNotNullAttribute("data-map-maxzoom", Context.MaxZoom);
            tag.MergeNotNullAttribute("data-map-createlayer", Context.CreateLayerFunction);
            tag.MergeNotNullAttribute("data-map-onload", Context.OnLoadFunction);
            return base.UpdateTag(tag);
        }
    }

    public class MapContext<TValue> : BoundControlContext<TValue>
    {
        public double HeightPercentage { get; internal set; } = 100;
        public double Zoom { get; internal set; } = 19;
        public double MinZoom { get; internal set; } = 5;
        public double MaxZoom { get; internal set; } = 20;
        public string CreateLayerFunction { get; internal set; }
        public string OnLoadFunction { get; internal set; }
    }
}
