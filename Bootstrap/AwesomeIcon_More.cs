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
using System.Web;
using System.Web.Mvc;
using System;

namespace BWakaBats.Bootstrap
{
    public enum AwesomeIconRotateOrFlip
    {
        None,
        Rotate90,
        Rotate180,
        Rotate270,
        FlipHorizontal,
        FlipVertical,
    }

    public enum AwesomeIconPull
    {
        None,
        Left,
        Right,
    }

    public partial class AwesomeIcon : IHtmlString, IPreAppender
    {
        private bool _isStatic = true;

        public AwesomeIcon(string className)
            : this(className, className.ToWords(), className)
        {
        }

        private AwesomeIcon(string id, string name, string className)
        {
            Context.Id = id;
            Context.Name = name;
            Context.ClassName = className;
        }

        private AwesomeIcon(AwesomeIcon original)
            : this(original.Context.Id, original.Context.Name, original.Context.ClassName)
        {
            _isStatic = false;
        }

        public AwesomeIconContext Context { get; } = new AwesomeIconContext();

        #region Properties

        public AwesomeIcon HtmlAttributes(object newValue)
        {
            var that = NonStatic(this);
            that.Context.HtmlAttributes = newValue;
            return that;
        }

        public AwesomeIcon Size(int newValue)
        {
            if (Context.Size == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Size = newValue;
            return that;
        }

        public AwesomeIcon RotateOrFlip(AwesomeIconRotateOrFlip newValue)
        {
            if (Context.RotateOrFlip == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.RotateOrFlip = newValue;
            return that;
        }

        public AwesomeIcon FixedWidth(bool newValue = true)
        {
            if (Context.FixedWidth == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.FixedWidth = newValue;
            return that;
        }

        public AwesomeIcon UnorderedList(bool newValue = true)
        {
            if (Context.UnorderedList == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.UnorderedList = newValue;
            return that;
        }

        public AwesomeIcon Bordered(bool newValue = true)
        {
            if (Context.Bordered == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Bordered = newValue;
            return that;
        }

        public AwesomeIcon Pull(AwesomeIconPull newValue)
        {
            if (Context.Pull == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Pull = newValue;
            return that;
        }

        public AwesomeIcon Spin(bool newValue = true)
        {
            if (Context.Spin == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.Spin = newValue;
            return that;
        }

        public AwesomeIcon StackSize(int newValue)
        {
            if (Context.StackSize == newValue)
                return this;

            var that = NonStatic(this);
            that.Context.StackSize = newValue;
            return that;
        }

        #endregion

        public override int GetHashCode()
        {
            return Context.ClassName.GetHashCode();
        }

        public IHtmlString ToTag(string tagName = "span")
        {
            string classNames = ToHtmlString();
            if (Context.HtmlAttributes == null)
                return new HtmlString("<" + tagName + " class='" + classNames + "'></" + tagName + ">");

            var tag = new TagBuilder(tagName);
            tag.MergeAttributes(Context.HtmlAttributes);
            tag.AddCssClass(classNames);
            return new HtmlString(tag.ToString());

        }

        public override string ToString()
        {
            return ToHtmlString();
        }

        public static AwesomeIcon FromString(string name)
        {
            return _lookup[name];
        }

        public virtual string ToHtmlString()
        {
            string classes = "fa " + Context.ClassName;
            switch (Context.Size)
            {
                case 0:
                    break;
                case 1:
                    classes += " fa-lg";
                    break;
                default:
                    classes += " fa-" + Context.Size + "x";
                    break;
            }
            switch (Context.RotateOrFlip)
            {
                case AwesomeIconRotateOrFlip.Rotate90:
                    classes += " fa-rotate-90";
                    break;
                case AwesomeIconRotateOrFlip.Rotate180:
                    classes += " fa-rotate-180";
                    break;
                case AwesomeIconRotateOrFlip.Rotate270:
                    classes += " fa-rotate-270";
                    break;
                case AwesomeIconRotateOrFlip.FlipHorizontal:
                    classes += " fa-flip-horizontal";
                    break;
                case AwesomeIconRotateOrFlip.FlipVertical:
                    classes += " fa-flip-vertical";
                    break;
            }
            if (Context.FixedWidth)
            {
                classes += " fa-fw";
            }
            if (Context.UnorderedList)
            {
                classes += " fa-li";
            }
            if (Context.Bordered)
            {
                classes += " fa-border";
            }
            switch (Context.Pull)
            {
                case AwesomeIconPull.Left:
                    classes += " fa-pull-left";
                    break;
                case AwesomeIconPull.Right:
                    classes += " fa-pull-right";
                    break;
            }
            if (Context.Spin)
            {
                classes += " fa-spin";
            }
            if (Context.StackSize > 0)
            {
                classes += " fa-stack-" + Context.StackSize + "x";
            }
            return classes;
        }

        private AwesomeIcon NonStatic(AwesomeIcon original)
        {
            return original._isStatic ? new AwesomeIcon(original) : original;
        }

        public string ToPreAppenderString()
        {
            return "<span class='input-group-addon'>" + ToTag() + "</span>";
        }
    }

    public class AwesomeIconContext
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string ClassName { get; internal set; }
        public int Size { get; internal set; }
        public AwesomeIconRotateOrFlip RotateOrFlip { get; internal set; }
        public bool FixedWidth { get; internal set; }
        public bool UnorderedList { get; internal set; }
        public bool Bordered { get; internal set; }
        public AwesomeIconPull Pull { get; internal set; }
        public bool Spin { get; internal set; }
        public int StackSize { get; internal set; }
        public object HtmlAttributes { get; internal set; }
    }
}
