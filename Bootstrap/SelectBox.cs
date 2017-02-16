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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BWakaBats.Bootstrap
{
    public abstract class SelectBox<TControl, TValue> : BoundControl<TControl, TValue>
        where TControl : SelectBox<TControl, TValue>
    {
        private static MethodInfo _getSelectData;
        private static MethodInfo _getModelStateValue;
        private static MethodInfo _getSelectListWithDefaultValue;
        private static MethodInfo _buildItems;

        internal SelectBox(SelectBoxContext<TValue> context, string name)
            : base(context, name, true)
        {
            Context = context;
        }

        public new SelectBoxContext<TValue> Context { get; private set; }

        #region Control Properties

        public TControl OptionalLabel(string newValue)
        {
            Context.OptionalLabel = newValue;
            return (TControl)this;
        }

        public TControl Items(IEnumerable<SelectListItem> newValue)
        {
            Context.Items = newValue;
            return (TControl)this;
        }

        #endregion

        protected override string TagType
        {
            get { return "select"; }
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            string name = Context.Name;
            bool allowMultiple = AllowMultipleOverride;
            if (allowMultiple)
            {
                tag.MergeAttribute("multiple", "multiple");
            }
            var selectList = Context.Items;
            bool flag = (selectList == null);
            if (flag)
            {
                selectList = GetSelectData(HtmlHelper, name);
            }

            object defaultValue;
            TValue value = Context.Value;
            if (value != null)
            {
                defaultValue = value;
            }
            else if (!string.IsNullOrWhiteSpace(FullHtmlFieldName))
            {
                var destinationType = allowMultiple ? typeof(string[]) : typeof(string);
                defaultValue = GetModelStateValue(HtmlHelper, FullHtmlFieldName, destinationType);
            }
            else
            {
                defaultValue = null;
            }
            if (!flag && defaultValue == null && !string.IsNullOrWhiteSpace(name))
            {
                defaultValue = HtmlHelper.ViewData.Eval(name);
            }
            if (defaultValue != null)
            {
                selectList = GetSelectListWithDefaultValue(selectList, defaultValue, allowMultiple);
            }

            tag.InnerHtml = BuildItems(Context.OptionalLabel, selectList).ToString();
            return base.UpdateTag(tag);
        }

        protected override string DefaultCssClass
        {
            get { return "form-control"; }
        }

        protected virtual bool AllowMultipleOverride
        {
            get { return false; }
        }

        #region Static <option> Helper Functions

        private static IEnumerable<SelectListItem> GetSelectData(HtmlHelper htmlHelper, string name)
        {
            if (_getSelectData == null)
            {
                var type = typeof(SelectExtensions);
                _getSelectData = type.GetMethod("GetSelectData", BindingFlags.Static | BindingFlags.NonPublic);
            }
            return (IEnumerable<SelectListItem>)_getSelectData.Invoke(null, new object[] { htmlHelper, name });
        }

        private static object GetModelStateValue(HtmlHelper htmlHelper, string key, Type destinationType)
        {
            if (_getModelStateValue == null)
            {
                var type = typeof(HtmlHelper);
                _getModelStateValue = type.GetMethod("GetModelStateValue", BindingFlags.NonPublic);
            }
            return _getModelStateValue.Invoke(htmlHelper, new object[] { key, destinationType });
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IEnumerable", Justification = "This is a message for developers")]
        private static IEnumerable<SelectListItem> GetSelectListWithDefaultValue(IEnumerable<SelectListItem> selectList, object defaultValue, bool allowMultiple)
        {
            if (_getSelectListWithDefaultValue == null)
            {
                var type = typeof(SelectExtensions);
                _getSelectListWithDefaultValue = type.GetMethod("GetSelectListWithDefaultValue", BindingFlags.Static | BindingFlags.NonPublic);
            }

            if (allowMultiple)
            {
                var enumerable = defaultValue as IEnumerable;
                if ((enumerable == null) || (enumerable is string))
                {
                    int bits = (int)defaultValue;
                    var list = new List<int>();
                    int bit = 1;
                    while (bits != 0)
                    {
                        if ((bits & bit) != 0)
                        {
                            list.Add(bit);
                            bits -= bit;
                        }
                        bit *= 2;
                    }
                    defaultValue = list;
                }
            }

            return (IEnumerable<SelectListItem>)_getSelectListWithDefaultValue.Invoke(null, new object[] { selectList, defaultValue, allowMultiple });
        }

        private static StringBuilder BuildItems(string optionLabel, IEnumerable<SelectListItem> selectList)
        {
            if (_buildItems == null)
            {
                var type = typeof(SelectExtensions);
                _buildItems = type.GetMethod("BuildItems", BindingFlags.Static | BindingFlags.NonPublic);
            }
            return (StringBuilder)_buildItems.Invoke(null, new object[] { optionLabel, selectList });
        }

        #endregion
    }

    public class SelectBoxContext<TValue> : BoundControlContext<TValue>
    {
        public string OptionalLabel { get; internal set; }
        public IEnumerable<SelectListItem> Items { get; internal set; }
    }

    public sealed class SelectBox<TValue> : SelectBox<SelectBox<TValue>, TValue>
    {
        internal SelectBox(string name = null) : base(new SelectBoxContext<TValue>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static SelectBox<object> BootstrapSelectBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new SelectBox<object>(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static SelectBox<TProperty> BootstrapSelectBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new SelectBox<TProperty>();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
