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
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class FilePicker<TControl> : PreAppend<TControl, string>
        where TControl : FilePicker<TControl>
    {
        protected FilePicker(FilePickerContext context, string name)
            : base(context, name, true)
        {
            Context = context;
        }

        public new FilePickerContext Context { get; private set; }

        #region Control Properties

        public TControl FileType(string newValue)
        {
            Context.FileType = newValue;
            return (TControl)this;
        }

        public TControl FileName(string newValue)
        {
            Context.FileName = newValue;
            return (TControl)this;
        }

        public TControl Button(string newValue)
        {
            Context.Button = HtmlHelper.BootstrapButton(AwesomeIcon.Photo).Header(newValue).Behavior(Extensions.ButtonBehavior.Button);
            return (TControl)this;
        }

        public TControl Button(Button newValue)
        {
            Context.Button = newValue;
            return (TControl)this;
        }

        public TControl ActionName(string newValue)
        {
            Context.ActionName = newValue;
            return (TControl)this;
        }

        public TControl ControllerName(string newValue)
        {
            Context.ControllerName = newValue;
            return (TControl)this;
        }

        public TControl CanRotate(bool newValue = true)
        {
            Context.CanRotate = newValue;
            return (TControl)this;
        }

        public TControl CanCrop(bool newValue = true)
        {
            Context.CanCrop = newValue;
            return (TControl)this;
        }

        public TControl CanDelete(bool newValue = true)
        {
            Context.CanDelete = newValue;
            return (TControl)this;
        }

        public TControl CanCapture(bool newValue = true)
        {
            Context.CanCapture = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            var value = Context.Value;

            tag.MergeAttribute("type", "hidden");
            if (!string.IsNullOrWhiteSpace(value))
            {
                tag.MergeAttribute("value", value);
            }
            return base.UpdateTag(tag);
        }

        protected override string WrapTag(TagBuilder tag)
        {
            string imageId = Context.Id + "_Image";
            string imageName = FullHtmlFieldName + "_Image";
            var img = new HtmlTagBuilder("img");
            img.AddCssClass("img-preview");
            img.Attributes.Add("id", imageId);
            img.Attributes.Add("name", imageName);

            string selected;
            if (!string.IsNullOrEmpty(Context.FileName))
            {
                img.Attributes.Add("src", Context.FileName);
                selected = " selected";
            }
            else
            {
                selected = "";
            }
            string output = tag.ToString()
                 + "<div class='filepicker'>"
                 + img.ToString()
                 + "<div class='filepicker-buttons" + selected + "'>";

            bool havePreAppend = Context.Append.Count > 0 || Context.Prepend.Count > 0;
            bool moreButtons = Context.CanRotate || Context.CanCrop || havePreAppend;

            if (!moreButtons && (Context.CanDelete || Context.CanCapture))
            {
                output += "<div class='btn-group'>";
            }

            if (Context.Button == null)
            {
                Button(Context.FileType == FilePickerContext.ImageFileType ? "Add Image" : "Add File");
            }
            var button = Context.Button.Id(Context.Id + "_Button");
            var htmlAttributes = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(button.Context.HtmlAttributes);
            AdditionalButtonAttributes(htmlAttributes);
            button.HtmlAttributes(htmlAttributes);

            output += button.ToString();

            if (moreButtons)
            {
                output += "    <br /><div class='btn-group'>";
            }

            output += GetPreAppendString(Context.Prepend).ToString();

            if (Context.CanCapture)
            {
                output += "    <span class='input-group-btn'>"
                        + "      <button"
                        + "        type='button' class='btn btn-info'"
                        + "        data-file-capture-for='" + Context.Id + "'"
                        + "        data-file-type='" + Context.FileType + "'"
                        + "        title='Webcam Capture'>"
                        + "<span class='fa fa-camera'></span>"
                        + "      </button>"
                        + "    </span>";
            }
            if (Context.CanRotate)
            {
                output += "    <span class='input-group-btn'>"
                        + "      <button"
                        + "        type='button' class='btn btn-success'"
                        + "        data-file-rotate-for='" + Context.Id + "'"
                        + "        data-file-url='" + UrlHelper.Action("RotateLeft", Context.ControllerName, new { id = "_ID_" }) + "'"
                        + "        title='Rotate image anti-clockwise'>"
                        + "<span class='fa fa-rotate-left'></span>"
                        + "      </button>"
                        + "    </span>"
                        + "    <span class='input-group-btn'>"
                        + "      <button"
                        + "        type='button' class='btn btn-success'"
                        + "        data-file-rotate-for='" + Context.Id + "'"
                        + "        data-file-url='" + UrlHelper.Action("RotateRight", Context.ControllerName, new { id = "_ID_" }) + "'"
                        + "        title='Rotate image clockwise'>"
                        + "<span class='fa fa-rotate-right'></span>"
                        + "      </button>"
                        + "    </span>";
            }
            if (Context.CanCrop)
            {
                output += "    <span class='input-group-btn'>"
                        + "      <button"
                        + "        type='button' class='btn btn-success'"
                        + "        data-file-crop-for='" + Context.Id + "'"
                        + "        data-file-url='" + UrlHelper.Action("Crop", Context.ControllerName) + "'"
                        + "        title='Crop image'>"
                        + "<span class='fa fa-crop'></span>"
                        + "      </button>"
                        + "    </span>"
                        + "<input id='" + Context.Id + "_Crop' name='" + Context.Id + "_Crop' type='hidden' />";
            }
            output += GetPreAppendString(Context.Append).ToString();

            if (Context.CanDelete)
            {
                output += "    <span class='input-group-btn'>"
                        + "      <button"
                        + "        type='button' class='btn btn-danger'"
                        + "        data-file-cancel-for='" + Context.Id + "'"
                        + "        data-file-type='" + Context.FileType + "'"
                        + "        title='Delete'>"
                        + "<span class='fa fa-trash-o'></span>"
                        + "      </button>"
                        + "    </span>";
            }
            if (moreButtons || Context.CanDelete || Context.CanCapture)
            {
                output += "    </div>"; // btn-group
            }
            output += "  </div>" // filepicker-buttons
                    + "  <span id='" + Context.Id + "_FileName'>"
                    + GeneratePlaceholder()
                    + "</span>"
                    + "  <div id='" + Context.Id + "_ProgressContainer'>"
                    + "    <div id='" + Context.Id + "_ProgressPercent'>100%</div>"
                    + "    <div class='progress progress-striped active'>"
                    + "      <div id='" + Context.Id + "_Progress' class='progress-bar' role='progressbar'></div>"
                    + "    </div>" // progress
                    + "  </div>" // ProgressContainer
                    + "</div>"; // filepicker

            return output;
        }

        protected virtual void AdditionalButtonAttributes(IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes.Add("data-file-for", Context.Id);
            string url = UrlHelper.Action(Context.ActionName, Context.ControllerName);
            htmlAttributes.Add("data-file-url", url);
            htmlAttributes.Add("data-file-type", Context.FileType);
        }

        protected override string DefaultPlaceholder(string name)
        {
            return "";
        }
    }

    public class FilePickerContext : PreAppendContext<string>
    {
        public const string ImageFileType = "image";

        public string FileType { get; internal set; } = ImageFileType;
        public string FileName { get; internal set; }
        public string ActionName { get; internal set; } = "CreateFile";
        public string ControllerName { get; internal set; }
        public bool CanRotate { get; internal set; }
        public bool CanCrop { get; internal set; }
        public bool CanDelete { get; internal set; } = true;
        public bool CanCapture { get; internal set; } = true;
        public Button Button { get; internal set; }
    }

    public sealed class FilePicker : FilePicker<FilePicker>
    {
        internal FilePicker(string name = null) : base(new FilePickerContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static FilePicker BootstrapFilePicker(this HtmlHelper htmlHelper, string name)
        {
            var control = new FilePicker(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static FilePicker BootstrapFilePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new FilePicker();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
