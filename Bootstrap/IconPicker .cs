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
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class IconPicker<TControl> : PreAppend<TControl, string>
        where TControl : IconPicker<TControl>
    {
        private static string _popup;

        #region Favourites

        private static List<AwesomeIcon> _favourites = new List<AwesomeIcon>
        {
            AwesomeIcon.StarOutline,
            AwesomeIcon.NewspaperOutline,
            AwesomeIcon.Calendar,
            AwesomeIcon.Database,
            AwesomeIcon.PlusSquare,
            AwesomeIcon.MinusSquare,
            AwesomeIcon.TimesCircle,
            AwesomeIcon.Angellist,
            AwesomeIcon.Glass,
            AwesomeIcon.Music,
            AwesomeIcon.EnvelopeOutline,
            AwesomeIcon.Heart,
            AwesomeIcon.User,
            AwesomeIcon.Film,
            AwesomeIcon.Check,
            AwesomeIcon.Remove,
            AwesomeIcon.Signal,
            AwesomeIcon.Gear,
            AwesomeIcon.Home,
            AwesomeIcon.ClockOutline,
            AwesomeIcon.Download,
            AwesomeIcon.Inbox,
            AwesomeIcon.PlayCircleOutline,
            AwesomeIcon.Lock,
            AwesomeIcon.Headphones,
            AwesomeIcon.VolumeUp,
            AwesomeIcon.Book,
            AwesomeIcon.Bookmark,
            AwesomeIcon.Print,
            AwesomeIcon.Camera,
            AwesomeIcon.VideoCamera,
            AwesomeIcon.Photo,
            AwesomeIcon.Pencil,
            AwesomeIcon.MapMarker,
            AwesomeIcon.Crosshairs,
            AwesomeIcon.Gift,
            AwesomeIcon.Eye,
            AwesomeIcon.Warning,
            AwesomeIcon.Plane,
            AwesomeIcon.Comment,
            AwesomeIcon.ShoppingCart,
            AwesomeIcon.FolderOpen,
            AwesomeIcon.CameraRetro,
            AwesomeIcon.Key,
            AwesomeIcon.Gears,
            AwesomeIcon.Comments,
            AwesomeIcon.ThumbsOutlineUp,
            AwesomeIcon.ThumbsOutlineDown,
            AwesomeIcon.Trophy,
            AwesomeIcon.Phone,
            AwesomeIcon.Bullhorn,
            AwesomeIcon.BellOutline,
            AwesomeIcon.Certificate,
            AwesomeIcon.Globe,
            AwesomeIcon.Wrench,
            AwesomeIcon.ArrowsAlternative,
            AwesomeIcon.Group,
            AwesomeIcon.Cloud,
            AwesomeIcon.Flask,
            AwesomeIcon.Cut,
            AwesomeIcon.Copy,
            AwesomeIcon.Paperclip,
            AwesomeIcon.Save,
            AwesomeIcon.Table,
            AwesomeIcon.Magic,
            AwesomeIcon.Truck,
            AwesomeIcon.Dashboard,
            AwesomeIcon.Umbrella,
            AwesomeIcon.LightbulbOutline,
            AwesomeIcon.Suitcase,
            AwesomeIcon.Bell,
            AwesomeIcon.Coffee,
            AwesomeIcon.Cutlery,
            AwesomeIcon.Beer,
            AwesomeIcon.MobilePhone,
            AwesomeIcon.QuoteRight,
            AwesomeIcon.SmileOutline,
            AwesomeIcon.FrownOutline,
            AwesomeIcon.MehOutline,
            AwesomeIcon.PuzzlePiece,
            AwesomeIcon.Microphone,
            AwesomeIcon.Shield,
            AwesomeIcon.Rocket,
            AwesomeIcon.PlayCircle,
            AwesomeIcon.Ticket,
            AwesomeIcon.Female,
            AwesomeIcon.Male,
            AwesomeIcon.SunOutline,
            AwesomeIcon.MoonOutline,
            AwesomeIcon.Archive,
            AwesomeIcon.Bug,
            AwesomeIcon.Institution,
            AwesomeIcon.MortarBoard,
            AwesomeIcon.Child,
            AwesomeIcon.Paw,
            AwesomeIcon.Spoon,
            AwesomeIcon.Cube,
            AwesomeIcon.Cab,
            AwesomeIcon.Tree,
            AwesomeIcon.Bomb,
        };

        #endregion

        protected IconPicker(IconPickerContext context, string name)
            : base(context, name, true)
        {
            Context = context;
        }

        public new IconPickerContext Context { get; private set; }

        #region Control Properties

        public TControl Style(ButtonStyle newValue)
        {
            Context.Style = newValue;
            return (TControl)this;
        }

        public TControl OptionalLabel(string newValue)
        {
            Context.OptionalLabel = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "hidden");
            tag.MergeNotNullAttribute("value", Context.Value);
            return base.UpdateTag(tag);
        }

        protected override string WrapTag(TagBuilder tag)
        {
            var value = new AwesomeIcon(Context.Value);
            string id = Context.Id;
            string buttonStyle = "btn-" + (Context.Style == ButtonStyle.Information ? "info" : Context.Style.ToString().ToLowerInvariant());

            var group = new TagBuilder("div");
            if (Context.Append.Count + Context.Prepend.Count > 0)
            {
                group.AddCssClass("input-group-btn");
            }
            else
            {
                group.AddCssClass("btn-group");
            }

            group.InnerHtml = "<button type='button' class='btn " + buttonStyle + " btn-iconpicker dropdown-toggle' data-toggle='dropdown'>"
                + "<span id='" + id + "_ip' class='fa-fw " + value.ToString() + "'></span>"
                + " <span class='caret'></span>"
                + "</button>"
                + GeneratePopup(id, (Context.IsRequired ? null : (Context.OptionalLabel ?? "(none)")), Context.OnClick)
                + "<span id='" + Context.Id + "_IconName'>"
                + GeneratePlaceholder()
                + "</span>";

            return tag + base.WrapTag(group);
        }

        protected override string WrapGroupStyle
        {
            get { return "width:0;display:block;"; }
        }

        private static string GeneratePopup(string id, string optionalLabel, string onClick)
        {
            if (_popup == null)
            {
                var cellsList = _favourites.Select(c => GeneratePopupCell(c, false))
                                           .Concat(
                                                      AwesomeIcon.All.Select(c => GeneratePopupCell(c, true))
                                                  );

                var cells = string.Join("", cellsList)
                          + "<div class='icon-more' onclick='javascript: $(this).hide();$(this).parent().removeClass(\"icon-large\").addClass(\"icon-small\");event.stopPropagation(); event.preventDefault(); return true;'>more...</div>";

                _popup = "<div class='iconpicker-popup icon-large'>_OPTIONAL_" + cells + "</div>";
            }

            string popup;
            if (optionalLabel == null)
            {
                popup = _popup.Replace("_OPTIONAL_", "");
            }
            else
            {
                popup = _popup.Replace("_OPTIONAL_", "<div class='icon-optional' onclick='javascript: $(\"#_ID_\").val(null);$(\"#_ID__ip\").removeClass().addClass(\"fa fa-fw fa-times fa-none\");window.isDirty=true;" + onClick + "'>" + optionalLabel + "</div>");
            }
            popup = popup.Replace("_ONCLICK_", onClick);
            return popup.Replace("_ID_", id);
        }

        private static object GeneratePopupCell(AwesomeIcon icon, bool ignoreFavourites)
        {
            bool isFavourite = _favourites.Contains(icon);
            if (ignoreFavourites && isFavourite)
                return "";

            return "<div "
                 + (isFavourite ? "class='icon-fav'" : "")
                 + "onclick='javascript: $(\"#_ID_\").val(\"" + icon.Context.ClassName + "\");$(\"#_ID__ip\").removeClass().addClass(\"fa-fw " + icon.ToString() + "\");$(\"#_ID__IconName\").html(\"" + icon.Context.Name + "\");window.isDirty=true;_ONCLICK_'>"
                 + icon.FixedWidth(true).FixedWidth(true).ToTag()
                 + "</div>";
        }
    }

    public class IconPickerContext : PreAppendContext<string>
    {
        public string OptionalLabel { get; internal set; }
        public ButtonStyle Style { get; internal set; }
    }

    public sealed class IconPicker : IconPicker<IconPicker>
    {
        internal IconPicker(string name = null) : base(new IconPickerContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static IconPicker BootstrapIconPicker(this HtmlHelper htmlHelper, string name)
        {
            var control = new IconPicker(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static IconPicker BootstrapIconPickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new IconPicker();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
