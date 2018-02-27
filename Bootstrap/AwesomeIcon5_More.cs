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
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public enum AwesomeIcon5Size
    {
        Normal,
        ExtraSmall,
        Small,
        Large,
        Automatic,
        Times2,
        Times3,
        Times5,
        Times7,
        Times10,
        Other,
    }

    public enum AwesomeIcon5Flip
    {
        None,
        FlipHorizontal,
        FlipVertical,
    }

    public enum AwesomeIcon5Pull
    {
        None,
        Left,
        Right,
    }

    public partial class AwesomeIcon5 : IHtmlString, IPreAppender, IAwesomeIcon5Layer
    {
        private bool _isStatic = true;
        private string _output;

        internal AwesomeIcon5(string name, string className, string keywords)
        {
            Context.Name = name;
            Context.ClassName = className;
            Context.Keywords = keywords;
        }

        private AwesomeIcon5(AwesomeIcon5 original)
            : this(original.Context.Name, original.Context.ClassName, original.Context.Keywords)
        {
            _isStatic = false;
        }

        public AwesomeIcon5Context Context { get; } = new AwesomeIcon5Context();

        #region Properties

        public AwesomeIcon5 HtmlAttributes(object newValue)
        {
            var that = NonStatic(this);
            that.Context.HtmlAttributes = newValue;
            return that;
        }

        public AwesomeIcon5 Size(double newValue)
        {
            if (Context.SizeOther == newValue)
                return this;

            AwesomeIcon5Size size;
            switch (newValue)
            {
                case .75:
                    size = AwesomeIcon5Size.ExtraSmall;
                    break;
                case .875:
                    size = AwesomeIcon5Size.Small;
                    break;
                case 1:
                    size = AwesomeIcon5Size.Normal;
                    break;
                case 1.33:
                    size = AwesomeIcon5Size.Large;
                    break;
                case 2:
                    size = AwesomeIcon5Size.Times2;
                    break;
                case 3:
                    size = AwesomeIcon5Size.Times3;
                    break;
                case 5:
                    size = AwesomeIcon5Size.Times5;
                    break;
                case 7:
                    size = AwesomeIcon5Size.Times7;
                    break;
                case 10:
                    size = AwesomeIcon5Size.Times10;
                    break;
                default:
                    size = AwesomeIcon5Size.Other;
                    break;
            }
            var that = NonStatic(this);
            that.Context.Size = size;
            that.Context.SizeOther = newValue;
            return that;
        }

        public AwesomeIcon5 Size(AwesomeIcon5Size newValue)
        {
            if (Context.Size == newValue)
                return this;

            double sizeOther;
            switch (newValue)
            {
                case AwesomeIcon5Size.ExtraSmall:
                    sizeOther = .75;
                    break;
                case AwesomeIcon5Size.Small:
                    sizeOther = .875;
                    break;
                case AwesomeIcon5Size.Normal:
                    sizeOther = 1;
                    break;
                case AwesomeIcon5Size.Large:
                    sizeOther = 1.33;
                    break;
                case AwesomeIcon5Size.Times2:
                    sizeOther = 2;
                    break;
                case AwesomeIcon5Size.Times3:
                    sizeOther = 3;
                    break;
                case AwesomeIcon5Size.Times5:
                    sizeOther = 5;
                    break;
                case AwesomeIcon5Size.Times7:
                    sizeOther = 7;
                    break;
                case AwesomeIcon5Size.Times10:
                    sizeOther = 10;
                    break;
                default:
                    sizeOther = 0;
                    break;
            }
            var that = NonStatic(this);
            that.Context.Size = newValue;
            that.Context.SizeOther = sizeOther;
            return that;
        }

        public AwesomeIcon5 Shrink(double newValue)
        {
            if (Context.Shrink == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Shrink = newValue;
            return that;
        }

        public AwesomeIcon5 Grow(double newValue)
        {
            if (Context.Grow == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Grow = newValue;
            return that;
        }

        public AwesomeIcon5 Up(double newValue)
        {
            if (Context.Up == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Up = newValue;
            return that;
        }

        public AwesomeIcon5 Down(double newValue)
        {
            if (Context.Down == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Down = newValue;
            return that;
        }

        public AwesomeIcon5 Left(double newValue)
        {
            if (Context.Left == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Left = newValue;
            return that;
        }

        public AwesomeIcon5 Right(double newValue)
        {
            if (Context.Right == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Right = newValue;
            return that;
        }

        public AwesomeIcon5 Rotate(double newValue)
        {
            if (Context.Rotate == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Rotate = newValue;
            return that;
        }

        public AwesomeIcon5 Flip(AwesomeIcon5Flip newValue)
        {
            if (Context.Flip == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Flip = newValue;
            return that;
        }

        public AwesomeIcon5 FixedWidth(bool newValue = true)
        {
            if (Context.FixedWidth == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.FixedWidth = newValue;
            return that;
        }

        public AwesomeIcon5 UnorderedList(bool newValue = true)
        {
            if (Context.UnorderedList == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.UnorderedList = newValue;
            return that;
        }

        public AwesomeIcon5 Bordered(bool newValue = true)
        {
            if (Context.Bordered == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Bordered = newValue;
            return that;
        }

        public AwesomeIcon5 Pull(AwesomeIcon5Pull newValue)
        {
            if (Context.Pull == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Pull = newValue;
            return that;
        }

        public AwesomeIcon5 Spin(bool newValue = true)
        {
            if (Context.Spin == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Spin = newValue;
            return that;
        }

        public AwesomeIcon5 UseInverseColor(bool newValue = true)
        {
            if (Context.UseInverseColor == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.UseInverseColor = newValue;
            return that;
        }

        public AwesomeIcon5 Mask(AwesomeIcon5 newValue)
        {
            if (Context.Mask == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Mask = newValue;
            return that;
        }

        public AwesomeIcon5 AddLayer(AwesomeIcon5 newValue)
        {
            var that = NonStatic(this);
            that.Context.Layers.Add(newValue);
            return that;
        }

        public AwesomeIcon5 AddLayer(string newValue, object htmlAttributes = null)
        {
            var that = NonStatic(this);
            that.Context.Layers.Add(new AwesomeIcon5LayerText(newValue, htmlAttributes));
            return that;
        }

        public AwesomeIcon5 AddLayer(int newValue, object htmlAttributes = null)
        {
            var that = NonStatic(this);
            that.Context.Layers.Add(new AwesomeIcon5LayerCounter(newValue, htmlAttributes));
            return that;
        }

        #endregion

        public override int GetHashCode()
        {
            return Context.ClassName.GetHashCode();
        }

        public static AwesomeIcon5 FromString(string name)
        {
            return _lookup[name];
        }

        public string ToHtmlString()
        {
            return ToString("i");
        }

        public IHtmlString ToHtmlString(string tagName)
        {
            return new HtmlString(ToString(tagName));
        }

        public override string ToString()
        {
            return ToString("i");
        }

        public virtual string ToString(string tagName)
        {
            if (_output == null)
            {
                if (Context.Size != AwesomeIcon5Size.Automatic)
                {
                    _output = GenerateHtml(tagName, Context.Size);
                }
                else
                {
                    _output = "<span class='visible-xs'>" + GenerateHtml(tagName, AwesomeIcon5Size.ExtraSmall) + "</span>"
                            + "<span class='visible-sm'>" + GenerateHtml(tagName, AwesomeIcon5Size.Small) + "</span>"
                            + "<span class='visible-md'>" + GenerateHtml(tagName, AwesomeIcon5Size.Normal) + "</span>"
                            + "<span class='visible-lg'>" + GenerateHtml(tagName, AwesomeIcon5Size.Large) + "</span>";
                }
            }
            return _output;
        }

        #region Generate

        private string GenerateHtml(string tagName, AwesomeIcon5Size size)
        {
            string classNames = GenerateClassNames();
            string transform = GenerateTransform();
            string mask = GenerateMask();

            string tagText;
            if (Context.HtmlAttributes == null)
            {
                tagText = "<" + tagName
                        + " class='" + classNames + "'"
                        + (string.IsNullOrEmpty(transform) ? "" : (" data-fa-transform='" + transform + "'"))
                        + (string.IsNullOrEmpty(mask) ? "" : (" data-fa-mask='" + mask + "'"))
                        + "></" + tagName + ">";
            }
            else
            {
                var tag = new TagBuilder(tagName);
                if (!string.IsNullOrEmpty(transform))
                {
                    tag.Attributes.Add("data-fa-transform", transform);
                }
                if (!string.IsNullOrEmpty(mask))
                {
                    tag.Attributes.Add("data-fa-mask", mask);
                }
                tag.MergeAttributes(Context.HtmlAttributes);
                tag.AddCssClass(classNames);
                tagText = tag.ToString();
            }

            if (Context.Layers.Count == 0)
                return tagText;

            string output = "<span class='fa-layers fa-fw'>" + tagText;
            foreach (var layer in Context.Layers)
            {
                output += layer.ToAwesomeIcon5LayerString();
            }
            return output + "</span>";

        }

        private string GenerateClassNames()
        {
            string classNames = Context.ClassName;

            switch (Context.Size)
            {
                case AwesomeIcon5Size.Normal:
                    break;
                case AwesomeIcon5Size.ExtraSmall:
                    classNames += " fa-xs";
                    break;
                case AwesomeIcon5Size.Small:
                    classNames += " fa-sm";
                    break;
                case AwesomeIcon5Size.Large:
                    classNames += " fa-lg";
                    break;
                default:
                    classNames += " fa-" + Context.SizeOther.ToString(CultureInfo.InvariantCulture).Replace(".", "-") + "x";
                    break;
            }

            if (Context.FixedWidth)
            {
                classNames += " fa-fw";
            }

            if (Context.UnorderedList)
            {
                classNames += " fa-li";
            }

            if (Context.Bordered)
            {
                classNames += " fa-border";
            }

            switch (Context.Pull)
            {
                case AwesomeIcon5Pull.Left:
                    classNames += " fa-pull-left";
                    break;
                case AwesomeIcon5Pull.Right:
                    classNames += " fa-pull-right";
                    break;
            }

            if (Context.Spin)
            {
                classNames += " fa-spin";
            }

            if (Context.UseInverseColor)
            {
                classNames += " fa-inverse";
            }

            return classNames;
        }

        private string GenerateTransform()
        {
            string transform = "";

            if (Context.Shrink != 0)
            {
                transform = " shrink-" + Context.Shrink;
            }
            if (Context.Grow != 0)
            {
                transform = " grow-" + Context.Grow;
            }
            if (Context.Up != 0)
            {
                transform = " up-" + Context.Up;
            }
            if (Context.Down != 0)
            {
                transform = " down-" + Context.Down;
            }
            if (Context.Left != 0)
            {
                transform = " left-" + Context.Left;
            }
            if (Context.Right != 0)
            {
                transform = " right-" + Context.Right;
            }
            if (Context.Rotate != 0)
            {
                transform = " rotate-" + Context.Rotate;
            }

            switch (Context.Flip)
            {
                case AwesomeIcon5Flip.FlipHorizontal:
                    transform += " fa-flip-h";
                    break;
                case AwesomeIcon5Flip.FlipVertical:
                    transform += " fa-flip-v";
                    break;
            }

            if (transform.Length == 0)
                return transform;

            return transform.TrimStart();
        }

        private string GenerateMask()
        {
            if (Context.Mask == null)
                return null;
            return Context.Mask.ToString();
        }

        #endregion

        private AwesomeIcon5 NonStatic(AwesomeIcon5 original)
        {
            return original._isStatic ? new AwesomeIcon5(original) : original;
        }

        public string ToPreAppenderString()
        {
            return "<span class='input-group-addon'>" + ToString() + "</span>";
        }

        public string ToAwesomeIcon5LayerString()
        {
            return ToString();
        }
    }

    public class AwesomeIcon5Context
    {
        public string Name { get; internal set; }
        public string ClassName { get; internal set; }
        public string Keywords { get; internal set; }
        public AwesomeIcon5Size Size { get; internal set; }
        public double SizeOther { get; internal set; }
        public double Shrink { get; internal set; }
        public double Grow { get; internal set; }
        public double Up { get; internal set; }
        public double Down { get; internal set; }
        public double Left { get; internal set; }
        public double Right { get; internal set; }
        public double Rotate { get; internal set; }
        public AwesomeIcon5Flip Flip { get; internal set; }
        public bool FixedWidth { get; internal set; }
        public bool UnorderedList { get; internal set; }
        public bool Bordered { get; internal set; }
        public AwesomeIcon5Pull Pull { get; internal set; }
        public bool Spin { get; internal set; }
        public bool UseInverseColor { get; internal set; }
        public AwesomeIcon5 Mask { get; internal set; }
        public ICollection<IAwesomeIcon5Layer> Layers { get; } = new List<IAwesomeIcon5Layer>();
        public object HtmlAttributes { get; internal set; }
    }
}
