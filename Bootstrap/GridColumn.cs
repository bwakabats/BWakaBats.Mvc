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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public enum GridColumnStyle
    {
        Text,
        Boolean,
        Date,
        DateTime,
        Number,
        Currency,
        Image,
        Buttons,
        Control,
        PercentageBar,
    }

    public enum GridColumnTotal
    {
        None,
        Title, // Do not place anything between None and Total without reviewing "Skip total titles"
        Sum,
        Average,
        Min,
        Max,
        Count,
    }

    public enum GridColumnSubtotal
    {
        None,
        Prefix,
        Suffix,
    }

    public enum GridBooleanPlus
    {
        False,
        True,
        Automatic,
        Force,
    }

    public enum GridGroupsExpanded
    {
        None,
        First,
        All,
    }

    public abstract class GridColumn<TControl, TRow>
        where TControl : GridColumn<TControl, TRow>
    {
        protected GridColumn(GridColumnContext<TRow> context)
        {
            Context = context;
        }

        public GridColumnContext<TRow> Context { get; internal set; }

        #region Control Properties

        public TControl Header(string newValue)
        {
            Context.Header = newValue;
            return (TControl)this;
        }

        public TControl WrapHeader(bool newValue = true)
        {
            Context.WrapHeader = newValue;
            return (TControl)this;
        }

        public TControl Tooltip(string newValue)
        {
            Context.Tooltip = newValue;
            return (TControl)this;
        }

        public TControl Format(Func<TRow, object> newValue)
        {
            Context.Format = (row, rowIndex) => newValue(row);
            return (TControl)this;
        }

        public TControl Format(Func<TRow, int, object> newValue)
        {
            Context.Format = newValue;
            return (TControl)this;
        }

        public TControl Style(GridColumnStyle newValue)
        {
            Context.Style = newValue;
            return (TControl)this;
        }

        public TControl Total(GridColumnTotal newValue = GridColumnTotal.Sum)
        {
            Context.Total = newValue;
            return (TControl)this;
        }

        public TControl Subtotal(GridColumnSubtotal newValue)
        {
            Context.Subtotal = newValue;
            return (TControl)this;
        }

        public TControl IsSortable(GridBooleanPlus newValue = GridBooleanPlus.True)
        {
            Context.IsSortable = newValue;
            return (TControl)this;
        }

        public TControl IsFilterable(GridBooleanPlus newValue = GridBooleanPlus.True)
        {
            Context.IsFilterable = newValue;
            return (TControl)this;
        }

        public TControl IsGroupable(GridBooleanPlus newValue = GridBooleanPlus.True)
        {
            Context.IsGroupable = newValue;
            return (TControl)this;
        }

        public TControl GroupsExpanded(GridGroupsExpanded newValue)
        {
            Context.GroupsExpanded = newValue;
            return (TControl)this;
        }

        public TControl Prefix(Func<TRow, int, string> newValue)
        {
            Context.Prefix = newValue;
            return (TControl)this;
        }

        public TControl Prefix(Func<TRow, string> newValue)
        {
            Context.Prefix = (row, rowIndex) => newValue(row);
            return (TControl)this;
        }

        public TControl Suffix(Func<TRow, int, string> newValue)
        {
            Context.Suffix = newValue;
            return (TControl)this;
        }

        public TControl Suffix(Func<TRow, string> newValue)
        {
            Context.Suffix = (row, rowIndex) => newValue(row);
            return (TControl)this;
        }

        public TControl Value(Func<TRow, int, string> newValue)
        {
            Context.Value = newValue;
            return (TControl)this;
        }

        public TControl Value(Func<TRow, string> newValue)
        {
            Context.Value = (row, rowIndex) => newValue(row);
            return (TControl)this;
        }

        public TControl HtmlAttributes(Func<TRow, int, object> newValue)
        {
            Context.HtmlAttributes = newValue;
            return (TControl)this;
        }

        public TControl HtmlAttributes(Func<TRow, object> newValue)
        {
            Context.HtmlAttributes = (row, rowIndex) => newValue(row);
            return (TControl)this;
        }

        public TControl HtmlAttributes(object newValue)
        {
            Context.HtmlAttributes = (row, index) => { return newValue; };
            return (TControl)this;
        }

        public GridColumnPosition Position { get; } = new GridColumnPosition();

        #endregion

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public virtual bool CreateCell(StringBuilder cells, object cellData, string columnClassPrefix, bool isHeader, bool isTotal, GridColumnContext context, object htmlAttributes)
        {
            var cell = new HtmlTagBuilder("div");

            int width = AddColClass(cell, columnClassPrefix, "xs", Position.Width(0), -1);
            width = AddColClass(cell, columnClassPrefix, "sm", Position.Width(1), width);
            width = AddColClass(cell, columnClassPrefix, "md", Position.Width(2), width);
            width = AddColClass(cell, columnClassPrefix, "lg", Position.Width(3), width);

            int offset = AddOffsetClass(cell, columnClassPrefix, "xs", Position.Offset(0), 0);
            offset = AddOffsetClass(cell, columnClassPrefix, "sm", Position.Offset(1), offset);
            offset = AddOffsetClass(cell, columnClassPrefix, "md", Position.Offset(2), offset);
            offset = AddOffsetClass(cell, columnClassPrefix, "lg", Position.Offset(3), offset);

            string style = Context.Style.ToString().ToLowerInvariant();
            if (Context.Style != GridColumnStyle.Text)
            {
                cell.AddCssClass(columnClassPrefix + "-" + style);
            }
            if (isHeader)
            {
                if (context.WrapHeader)
                {
                    cell.AddCssClass("wrap");
                }
                string valueId = context.Index.ToString(CultureInfo.InvariantCulture);
                var sortable = context.IsSortable;
                if (sortable != GridBooleanPlus.False)
                {
                    cell.MergeAttribute("data-sortable", valueId);
                    if (sortable != GridBooleanPlus.True)
                    {
                        cell.MergeAttribute("data-sortable-type", sortable.ToString().ToLowerInvariant());
                    }
                }
                var filterable = context.IsFilterable;
                if (filterable != GridBooleanPlus.False)
                {
                    cell.MergeAttribute("data-filterable", valueId);
                    if (filterable != GridBooleanPlus.True)
                    {
                        cell.MergeAttribute("data-filterable-type", filterable.ToString().ToLowerInvariant());
                    }
                }
                var groupable = context.IsGroupable;
                if (groupable != GridBooleanPlus.False)
                {
                    cell.MergeAttribute("data-groupable", valueId);
                    if (groupable != GridBooleanPlus.True)
                    {
                        cell.MergeAttribute("data-groupable-type", groupable.ToString().ToLowerInvariant());
                    }
                    var groupsExpanded = context.GroupsExpanded;
                    if (groupsExpanded != GridGroupsExpanded.First)
                    {
                        cell.MergeAttribute("data-groupable-expanded", groupsExpanded.ToString().ToLowerInvariant());
                    }
                }
            }
            else if (isTotal && context.Total > GridColumnTotal.Title) // Skip total titles
            {
                cell.MergeAttribute("data-total", context.Index.ToString(CultureInfo.InvariantCulture));
                cell.MergeAttribute("data-total-type", context.Total.ToString().ToLowerInvariant());
                if (context.Subtotal != GridColumnSubtotal.None)
                {
                    cell.MergeAttribute("data-total-sub", "true");
                }
                cell.MergeAttribute("data-total-style", context.Style.ToString().ToLowerInvariant());
            }
            bool hasData;
            if (cellData == null)
            {
                hasData = false;
            }
            else
            {
                string cellString = cellData.ToString();
                if (string.IsNullOrWhiteSpace(cellString))
                {
                    hasData = false;
                }
                else
                {
                    cell.InnerHtml = cellString;
                    hasData = true;
                }
            }

            cell.MergeAttributes(htmlAttributes);
            AddCell(cells, cell);

            AddClearFix(cells, "visible-xs", Position.EndOfRow(0));
            AddClearFix(cells, "visible-sm", Position.EndOfRow(1));
            AddClearFix(cells, "visible-md", Position.EndOfRow(2));
            AddClearFix(cells, "visible-lg", Position.EndOfRow(3));

            return hasData;
        }

        protected virtual void AddCell(StringBuilder cells, TagBuilder cell)
        {
            cells.Append(cell.ToString());
        }

        protected virtual void AddClearFix(StringBuilder cells, string className, bool endOfRow)
        {
            if (endOfRow)
            {
                var cell = new HtmlTagBuilder("div");
                cell.AddCssClass("clearfix");
                cell.AddCssClass(className);
                cells.Append(cell.ToString());
            }
        }

        private static int AddColClass(TagBuilder cell, string columnClassPrefix, string className, int width, int previousWidth)
        {
            if (width == 0)
            {
                cell.AddCssClass("hidden-" + className);
            }
            else if (previousWidth != width)
            {
                cell.AddCssClass(columnClassPrefix + "-" + className + "-" + width);
            }
            return width;
        }

        private static int AddOffsetClass(TagBuilder cell, string columnClassPrefix, string className, int offset, int previousOffset)
        {
            if (previousOffset != offset)
            {
                cell.AddCssClass(columnClassPrefix + "-" + className + "-offset-" + offset);
            }
            return offset;
        }
    }

    public class GridColumnContext
    {
        public int Index { get; internal set; }
        public string Header { get; internal set; }
        public bool WrapHeader { get; internal set; }
        public string Tooltip { get; internal set; }
        public GridColumnStyle Style { get; internal set; }
        public GridColumnTotal Total { get; internal set; }
        public GridColumnSubtotal Subtotal { get; internal set; } = GridColumnSubtotal.Prefix;
        public GridBooleanPlus IsSortable { get; internal set; }
        public GridBooleanPlus IsFilterable { get; internal set; }
        public GridBooleanPlus IsGroupable { get; internal set; }
        public GridGroupsExpanded GroupsExpanded { get; internal set; } = GridGroupsExpanded.First;
    }

    public class GridColumnContext<TRow> : GridColumnContext
    {
        public Func<TRow, int, object> Format { get; internal set; }
        public Func<TRow, int, string> Prefix { get; internal set; }
        public Func<TRow, int, string> Suffix { get; internal set; }
        public Func<TRow, int, string> Value { get; internal set; }
        public Func<TRow, int, object> HtmlAttributes { get; internal set; }
    }

    public class GridColumn<TRow> : GridColumn<GridColumn<TRow>, TRow>
    {
        public GridColumn() : base(new GridColumnContext<TRow>()) { }
    }

}
