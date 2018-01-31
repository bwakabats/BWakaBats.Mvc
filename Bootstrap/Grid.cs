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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public static class Grid
    {
        public const int NotPaginated = 0;
        public const string ClassName = "grid";
    }

    [Flags]
    public enum GridMoveableExtraButtons
    {
        None,
        Top = 1,
        Bottom = 2,
        TopBottom = Top + Bottom,
        Up = 4,
        Down = 8,
        UpDown = Up + Down,
        All = TopBottom + UpDown,
    }

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class Grid<TControl, TColumn, TRow> : Element<TControl>, IHtmlString
        where TControl : Grid<TControl, TColumn, TRow>
        where TColumn : GridColumn<TColumn, TRow>, new()
    {
        protected Grid(GridContext<TColumn, TRow> context, IEnumerable<TRow> rows)
            : base(context, null)
        {
            Context = context;
            Context.Rows = rows;
        }

        public new GridContext<TColumn, TRow> Context { get; private set; }

        #region Control Properties

        public TControl IfEmpty(string newValue)
        {
            Context.IfEmpty = newValue;
            return (TControl)this;
        }

        public TControl ColumnClassPrefix(string newValue)
        {
            Context.ColumnClassPrefix = newValue;
            return (TControl)this;
        }

        public TControl TotalColumnWidth(int newValue)
        {
            Context.TotalColumnWidth = newValue;
            return (TControl)this;
        }

        public TControl RowHtmlAttributes(Func<TRow, int, object> newValue)
        {
            Context.RowHtmlAttributes = newValue;
            return (TControl)this;
        }

        public TControl RowHtmlAttributes(Func<TRow, object> newValue)
        {
            Context.RowHtmlAttributes = (row, rowIndex) => newValue(row);
            return (TControl)this;
        }

        public TControl RowHtmlAttributes(object newValue)
        {
            Context.RowHtmlAttributes = (row, index) => { return newValue; };
            return (TControl)this;
        }

        public TControl Rows(IEnumerable<TRow> newValue)
        {
            Context.Rows = newValue;
            return (TControl)this;
        }

        public TControl Columns(GridColumnCollection<TColumn, TRow> newValue)
        {
            Context.Columns = newValue;
            return (TControl)this;
        }

        public TControl Layout(int[][] newValue)
        {
            Context.Layout = newValue;
            return (TControl)this;
        }

        public TControl PageSize(int newValue)
        {
            Context.PageSize = newValue;
            return (TControl)this;
        }

        public TControl Total(string newValue)
        {
            Context.Total = newValue;
            return (TControl)this;
        }

        public TControl MoveableRowId(Func<TRow, int, string> newValue)
        {
            Context.MoveableRowId = newValue;
            return (TControl)this;
        }

        public TControl MoveableRowId(Func<TRow, string> newValue)
        {
            Context.MoveableRowId = (row, rowIndex) => newValue(row);
            return (TControl)this;
        }

        public TControl MoveableUrl(string newValue)
        {
            Context.MoveableUrl = newValue;
            return (TControl)this;
        }

        public TControl MoveableExtraButtons(GridMoveableExtraButtons newValue)
        {
            Context.MoveableExtraButtons = newValue;
            return (TControl)this;
        }

        #endregion

        public override string ToString()
        {
            return ToHtmlString();
        }

        public string ToHtmlString()
        {
            var tag = CreateTag();

            string name = Context.Name;

            if (!string.IsNullOrWhiteSpace(name))
            {
                tag.Attributes.Add("data-container-for", name);
            }
            return tag.ToString();
        }

        protected override string TagType
        {
            get { return "div"; }
        }

        protected override string DefaultCssClass
        {
            get { return Grid.ClassName; }
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            var layout = Context.Layout;
            if (layout == null)
                throw new NullReferenceException("Layout has not been set");

            if (Context.MoveableRowId != null)
            {
                tag.AddCssClass(DefaultCssClass + "-moveable");
                tag.MergeAttribute("data-move-url", Context.MoveableUrl);
            }

            tag.MergeAttribute("data-move-buttons", Context.MoveableExtraButtons.ToString().ToLowerInvariant());
            tag.MergeAttribute("data-grid-id", GenerateId(Context.Columns).ToString(CultureInfo.InvariantCulture));
            tag.MergeAttribute("data-grid-pagesize", Context.PageSize.ToString(CultureInfo.InvariantCulture));

            bool hasColumnHeadings = false;
            int totalTitleIndex = 0;
            int index = 0;
            foreach (var column in Context.Columns)
            {
                index++;
                column.Context.Index = index;
                if (column.Context.Header != null)
                {
                    hasColumnHeadings = true;
                }
                if (column.Context.Total == GridColumnTotal.Title)
                {
                    totalTitleIndex = index;
                }
                else if (totalTitleIndex == 0 && column.Context.Total != GridColumnTotal.None)
                {
                    totalTitleIndex = 1;
                }
            }

            if (layout.Length != GridColumnPosition.Sizes)
                throw new ArgumentException("Layout must have " + GridColumnPosition.Sizes + " rows");

            var columnsArray = Context.Columns.ToArray();
            ArrangeLayout(columnsArray, layout, Context.TotalColumnWidth, c => c.Position);

            tag.InnerHtml = GetGridInnerHtml(hasColumnHeadings, totalTitleIndex, Context.Rows, columnsArray);

            return base.UpdateTag(tag);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected virtual string GetGridInnerHtml(bool hasColumnHeadings, int totalTitleIndex, IEnumerable<TRow> rows, TColumn[] columns)
        {
            string header = Context.Header;
            string columnClassPrefix = Context.ColumnClassPrefix;
            int totalColumnWidth = Context.TotalColumnWidth;
            TagBuilder rowTag;

            var result = new StringBuilder();
            if (hasColumnHeadings || header != null)
            {
                rowTag = CreateRow(true, false, header, columns, Grid.ClassName + "-header", columnClassPrefix, totalColumnWidth, Grid.NotPaginated, null, default(TRow), 0, null, c => (c.Context.Tooltip != null ? "<span data-tip='" + c.Context.Tooltip + "'>" : "<span>") + c.Context.Header + "</span>");
                if (rowTag != null)
                {
                    result.Append(rowTag.ToString());
                }
            }

            var valueColumns = columns.Where(c => c.Context.IsSortable != GridBooleanPlus.False
                                               || c.Context.IsFilterable != GridBooleanPlus.False
                                               || c.Context.IsGroupable != GridBooleanPlus.False
                                               || c.Context.Total > GridColumnTotal.Title) // Skip total titles
                                      .ToArray();

            int rowCount = 0;
            foreach (var row in rows)
            {
                int page = Context.PageSize == Grid.NotPaginated ? 1 : (rowCount / Context.PageSize) + 1;
                string rowName = (rowCount % 2 == 0) ? Grid.ClassName + "-row" : Grid.ClassName + "-row-alt";
                var rowHtmlAttributes = Context.RowHtmlAttributes == null ? null : Context.RowHtmlAttributes(row, rowCount);
                string rowId = Context.MoveableRowId == null ? null : Context.MoveableRowId(row, rowCount);

                rowTag = CreateRow(false, false, "", columns, rowName, columnClassPrefix, totalColumnWidth, page, rowHtmlAttributes, row, rowCount, rowId, c =>
                {
                    string prefix = GetCellValue(c.Context.Prefix, row, rowCount, c.Context.Style == GridColumnStyle.Currency ? "&#163;" /*£*/ : "");
                    object cellValue = GetCellValue(c.Context.Format, row, rowCount, null);
                    string suffix = GetCellValue(c.Context.Suffix, row, rowCount, "");

                    if (cellValue == null)
                        return prefix + suffix;

                    switch (c.Context.Style)
                    {
                        case GridColumnStyle.Date:
                            return prefix + Convert.ToDateTime(cellValue, CultureInfo.InvariantCulture).ToString("dd/MM/yy", CultureInfo.InvariantCulture) + suffix;
                        case GridColumnStyle.DateTime:
                            return prefix + Convert.ToDateTime(cellValue, CultureInfo.InvariantCulture).ToString("dd/MM/yy HH:mm", CultureInfo.InvariantCulture) + suffix;
                        case GridColumnStyle.Currency:
                            return prefix + Convert.ToDecimal(cellValue, CultureInfo.InvariantCulture).ToString("#,0.00", CultureInfo.InvariantCulture) + suffix;
                        case GridColumnStyle.Boolean:
                            return prefix + (Convert.ToBoolean(cellValue, CultureInfo.InvariantCulture) ? AwesomeIcon.Check.Size(2).ToTag() : null) + suffix;
                        case GridColumnStyle.PercentageBar:
                            double percent = Convert.ToDouble(cellValue, CultureInfo.InvariantCulture);
                            return prefix + "<div class='bar' style='width: " + percent + "%;'>&nbsp;</div>" + suffix;
                    }
                    return prefix + cellValue + suffix;
                });

                if (rowTag != null)
                {
                    AddValueData(rowTag, valueColumns, row, rowCount);
                    result.Append(rowTag.ToString());
                }

                rowCount++;
            }

            if (rowCount == 0)
            {
                if (Context.IfEmpty != null)
                {
                    rowTag = CreateIfEmpty(columnClassPrefix, totalColumnWidth, Context.IfEmpty);
                    if (rowTag != null)
                    {
                        result.Append(rowTag.ToString());
                    }
                }
            }
            else if (totalTitleIndex > 0)
            {
                rowTag = CreateRow(false, true, "", columns, Grid.ClassName + "-total", columnClassPrefix, totalColumnWidth, Grid.NotPaginated, null, default(TRow), 0, null, c => { return (c.Context.Index == totalTitleIndex) ? Context.Total : null; });
                if (rowTag != null)
                {
                    result.Append(rowTag.ToString());
                }
            }

            rowTag = CreateFooter(columnClassPrefix, totalColumnWidth, rowCount, Context.PageSize);
            if (rowTag != null)
            {
                result.Append(rowTag.ToString());
            }

            return result.ToString();
        }

        private static T GetCellValue<T>(Func<TRow, int, T> func, TRow row, int rowIndex, T defaultValue)
        {
            return func == null ? defaultValue : func(row, rowIndex);
        }

        #region Static Methods

        private static int GenerateId(IEnumerable<TColumn> columns)
        {
            string code = "";
            foreach (var column in columns)
            {
                code += (int)column.Context.IsFilterable
                      + (int)column.Context.IsSortable
                      + (int)column.Context.IsGroupable
                      + (int)column.Context.Total;
            }
            return code.GetHashCode();
        }

        protected static void ArrangeLayout(TColumn[] columns, int[][] layout, int totalColumnWidth, Func<TColumn, GridColumnPosition> positionSelector)
        {
            int columnLength = columns.Length;
            int size = 0;
            foreach (var layer in layout)
            {
                int index = 0;
                int offset = 0;
                int width = 0;
                foreach (var cell in layer)
                {
                    if (cell >= 0)
                    {
                        if (index == columnLength)
                            throw new ArgumentException("Too many columns in layout[" + size + "]", "layout");

                        var position = positionSelector(columns[index]);
                        position.Width(size, cell);
                        position.Offset(size, offset);
                        offset = 0;
                        index++;
                        width += cell;
                    }
                    else
                    {
                        offset += -cell;
                        width += offset;
                    }
                    if (width == totalColumnWidth)
                    {
                        var position = positionSelector(columns[index - 1]);
                        position.EndOfRow(size, true);
                        width = 0;
                        offset = 0;
                    }
                    else if (width > totalColumnWidth)
                    {
                        throw new ArgumentException("Row too wide layout[" + size + "]: " + width + "/" + totalColumnWidth, "layout");
                    }
                }
                if (index != columnLength)
                    throw new ArgumentException("Not enough columns in layout[" + size + "]: " + index + "/" + columnLength, "layout");
                if (width != 0)
                    throw new ArgumentException("Width not divisible by " + totalColumnWidth + " in layout[" + size + "]: " + width, "layout");

                size++;
            }
        }

        private static TagBuilder CreateRow(bool isHeader, bool isTotal, string header, TColumn[] columns, string className, string columnClassPrefix, int totalColumnWidth, int page, object rowHtmlAttributes, TRow row, int rowIndex, string rowId, Func<TColumn, object> dataSelector)
        {
            var innerHtml = new StringBuilder();

            bool hasData = !string.IsNullOrWhiteSpace(header);
            if (hasData)
            {
                innerHtml.Append("<div class='" + columnClassPrefix + "-xs-" + totalColumnWidth + "'><h2>");
                innerHtml.Append(header);
                innerHtml.AppendLine("</h2></div>");
            }

            foreach (var column in columns)
            {
                var context = column.Context;
                var columnHtmlAttributes = context.HtmlAttributes;
                var cellData = dataSelector(column);
                if (column.CreateCell(innerHtml, cellData, columnClassPrefix, isHeader, isTotal, context, isHeader || columnHtmlAttributes == null ? null : columnHtmlAttributes(row, rowIndex)))
                {
                    hasData = true;
                }
            }

            if (!hasData)
                return null;

            return CreateRowTag(className, page, rowId, innerHtml.ToString(), rowHtmlAttributes);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private static void AddValueData(TagBuilder rowTag, TColumn[] columns, TRow row, int rowCount)
        {
            string cellString;
            foreach (var column in columns)
            {
                var context = column.Context;
                string sortValue;
                string filterValue;

                if (context.Value != null)
                {
                    var originalValue = context.Value(row, rowCount);
                    sortValue = originalValue == null ? "" : originalValue.ToUpperInvariant();
                    filterValue = sortValue;
                }
                else
                {
                    var format = context.Format;
                    if (format == null)
                    {
                        sortValue = "";
                        filterValue = sortValue;
                    }
                    else
                    {
                        var cellValue = format(row, rowCount);
                        if (cellValue == null)
                        {
                            sortValue = "";
                            filterValue = sortValue;
                        }
                        else
                        {
                            switch (context.Style)
                            {
                                case GridColumnStyle.Date:
                                    cellString = cellValue as string;
                                    if (cellString != null)
                                    {
                                        sortValue = cellValue.ToString().PadLeft(9);
                                        filterValue = sortValue;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            DateTime cellDate = (DateTime)cellValue;
                                            sortValue = cellDate.ToString("yyyyMMddH", CultureInfo.InvariantCulture);
                                            filterValue = cellDate.ToString("dd/MM/yy", CultureInfo.InvariantCulture); ;
                                        }
                                        catch
                                        {
                                            sortValue = cellValue.ToString();
                                            filterValue = sortValue;
                                        }
                                    }
                                    break;
                                case GridColumnStyle.DateTime:
                                    cellString = cellValue as string;
                                    if (cellString != null)
                                    {
                                        sortValue = cellValue.ToString().PadLeft(9);
                                        filterValue = sortValue;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            DateTime cellDateTime = (DateTime)cellValue;
                                            sortValue = cellDateTime.ToString("yyyyMMddHHmmssff", CultureInfo.InvariantCulture);
                                            filterValue = cellDateTime.ToString("dd/MM/yy HH:mm", CultureInfo.InvariantCulture); ;
                                        }
                                        catch
                                        {
                                            sortValue = cellValue.ToString();
                                            filterValue = sortValue;
                                        }
                                    }
                                    break;
                                case GridColumnStyle.Currency:
                                    cellString = cellValue as string;
                                    if (cellString != null)
                                    {
                                        sortValue = cellValue.ToString().PadLeft(9);
                                        filterValue = sortValue;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            decimal cellDecimal = (decimal)cellValue;
                                            sortValue = cellDecimal.ToString("000000.00", CultureInfo.InvariantCulture);
                                            filterValue = cellDecimal.ToString("0.00", CultureInfo.InvariantCulture); ;
                                        }
                                        catch
                                        {
                                            sortValue = cellValue.ToString().PadLeft(9);
                                            filterValue = sortValue;
                                        }
                                    }
                                    break;
                                case GridColumnStyle.Number:
                                    cellString = cellValue as string;
                                    if (cellString != null)
                                    {
                                        sortValue = cellValue.ToString().PadLeft(14);
                                        filterValue = sortValue;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            double cellDouble = (double)cellValue;
                                            sortValue = cellDouble.ToString("00000000.00000", CultureInfo.InvariantCulture);
                                            filterValue = cellDouble.ToString("#0.00", CultureInfo.InvariantCulture);
                                        }
                                        catch
                                        {
                                            sortValue = cellValue.ToString().PadLeft(14);
                                            filterValue = sortValue;
                                        }
                                    }
                                    break;
                                case GridColumnStyle.PercentageBar:
                                    cellString = cellValue as string;
                                    if (cellString != null)
                                    {
                                        sortValue = cellValue.ToString().PadLeft(14);
                                        filterValue = sortValue;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int cellPercent = (int)cellValue;
                                            sortValue = cellPercent.ToString("000.00000", CultureInfo.InvariantCulture);
                                            filterValue = cellPercent.ToString("#0", CultureInfo.InvariantCulture);
                                        }
                                        catch
                                        {
                                            sortValue = cellValue.ToString().PadLeft(14);
                                            filterValue = sortValue;
                                        }
                                    }
                                    break;
                                case GridColumnStyle.Boolean:
                                    sortValue = ((bool)cellValue) ? "Yes" : "No";
                                    filterValue = sortValue;
                                    break;
                                default:
                                    sortValue = cellValue.ToString().ToUpperInvariant();
                                    filterValue = sortValue;
                                    break;
                            }
                        }
                    }
                }

                rowTag.MergeAttribute("data-sortvalue-" + context.Index, sortValue);
                rowTag.MergeAttribute("data-filtervalue-" + context.Index, filterValue);
                switch (context.Subtotal)
                {
                    case GridColumnSubtotal.Prefix:
                        if (context.Prefix != null)
                        {
                            rowTag.MergeAttribute("data-subvalue-" + context.Index, context.Prefix(row, rowCount));
                        }
                        break;
                    case GridColumnSubtotal.Suffix:
                        if (context.Suffix != null)
                        {
                            rowTag.MergeAttribute("data-subvalue-" + context.Index, context.Suffix(row, rowCount));
                        }
                        break;
                }
            }
        }

        private static TagBuilder CreateFooter(string columnClassPrefix, int totalColumnWidth, int rowCount, int pageSize)
        {
            if (pageSize == Grid.NotPaginated || rowCount <= pageSize)
                return null;

            int pages = (rowCount - 1) / pageSize + 1;

            var unorderedList = new StringBuilder();
            unorderedList.Append("<ul class='pagination pull-right'><li><a onclick=\"javascript:event.preventDefault();return false;\">Change page</a></li>");
            for (int page = 1; page <= pages; page++)
            {
                unorderedList.Append("<li data-page-select='");
                unorderedList.Append(page);
                unorderedList.Append('\'');
                if (page == 1)
                {
                    unorderedList.Append(" class='active'");
                }
                unorderedList.Append("><a onclick='javascript:var t=$(this);var p=t.closest(\".");
                unorderedList.Append(Grid.ClassName);
                unorderedList.Append("\");p.data(\"pagecurrent\", ");
                unorderedList.Append(page);
                unorderedList.Append(");p.find(\".row[data-page=");
                unorderedList.Append(page);
                unorderedList.Append("]\").removeClass(\"hide\");p.find(\".row[data-page][data-page!=");
                unorderedList.Append(page);
                unorderedList.Append("]\").addClass(\"hide\");t.closest(\".pagination\").children().removeClass(\"active\");t.parent().addClass(\"active\");event.preventDefault();return false;'>");
                unorderedList.Append(page);
                unorderedList.Append("</a></li>");
            }
            if (pages > 1)
            {
                unorderedList.Append("<li><a onclick='javascript:var t=$(this);var p=t.closest(\".");
                unorderedList.Append(Grid.ClassName);
                unorderedList.Append("\");p.data(\"pagecurrent\", ");
                unorderedList.Append(0);
                unorderedList.Append(");p.find(\".row[data-page!=0]\").removeClass(\"hide\");t.closest(\".pagination\").children().removeClass(\"active\");t.parent().addClass(\"active\");event.preventDefault();return false;'>");
                unorderedList.Append("All");
                unorderedList.Append("</a></li>");
            }
            unorderedList.Append("</ul>");

            var div = new TagBuilder("div");
            div.AddCssClass(columnClassPrefix + "-xs-" + totalColumnWidth);
            div.InnerHtml = unorderedList.ToString();

            return CreateRowTag(Grid.ClassName + "-footer", Grid.NotPaginated, null, div.ToString(), null);
        }

        private static TagBuilder CreateIfEmpty(string columnClassPrefix, int totalColumnWidth, string ifEmpty)
        {
            var div = new TagBuilder("div");
            div.AddCssClass(columnClassPrefix + "-xs-" + totalColumnWidth);
            div.InnerHtml = ifEmpty;

            return CreateRowTag(Grid.ClassName + "-row", Grid.NotPaginated, null, div.ToString(), null);
        }

        private static TagBuilder CreateRowTag(string className, int page, string rowId, string innerHtml, object rowHtmlAttributes)
        {
            var row = new HtmlTagBuilder("div");
            row.AddCssClass("row");
            row.AddCssClass(className);
            if (page != Grid.NotPaginated)
            {
                row.MergeAttribute("data-page", page.ToString(CultureInfo.InvariantCulture));
                if (page != 1)
                {
                    row.AddCssClass("hide");
                }
            }
            if (!string.IsNullOrEmpty(rowId))
            {
                row.MergeAttribute("data-rowid", rowId);
            }
            row.MergeAttributes(rowHtmlAttributes);

            row.InnerHtml = innerHtml;
            return row;
        }

        #endregion
    }

    public abstract class GridContext<TColumn, TRow> : ElementContext
        where TColumn : GridColumn<TColumn, TRow>, new()
    {
        protected GridContext() : base() { }

        public string IfEmpty { get; internal set; } = "No " + typeof(TRow).Name.ToWords() + "s found.";
        public string ColumnClassPrefix { get; internal set; } = "col";
        public int TotalColumnWidth { get; internal set; } = 12;
        public IEnumerable<TRow> Rows { get; internal set; }
        public GridColumnCollection<TColumn, TRow> Columns { get; internal set; } = new GridColumnCollection<TColumn, TRow>();
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public int[][] Layout { get; internal set; }
        public Func<TRow, int, object> RowHtmlAttributes { get; internal set; }
        public int PageSize { get; internal set; } = 20;
        public string Total { get; internal set; } = "Total";
        public Func<TRow, int, string> MoveableRowId { get; internal set; }
        public string MoveableUrl { get; internal set; }
        public GridMoveableExtraButtons MoveableExtraButtons { get; internal set; } = GridMoveableExtraButtons.All;
    }

    public sealed class Grid<TRow> : Grid<Grid<TRow>, GridColumn<TRow>, TRow>
    {
        internal Grid(IEnumerable<TRow> rows) : base(new GridContext<TRow>(), rows) { }
    }

    public class GridContext<TRow> : GridContext<GridColumn<TRow>, TRow>
    {
    }

    public static partial class HtmlHelperExtensions
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static Grid<TRow> BootstrapGrid<TRow>(this HtmlHelper htmlHelper, IEnumerable<TRow> rows)
        {
            var control = new Grid<TRow>(rows);
            control.Initialize(htmlHelper);
            return control;
        }
    }
}