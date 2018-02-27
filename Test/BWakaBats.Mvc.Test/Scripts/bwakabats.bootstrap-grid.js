var _bootstrapGridIndex = 0;
var BootstrapGrid = function ($element)
{
    _bootstrapGridIndex++;
    var self = this;

    var icons;
    if (utilities.checkFontVersion() == 4)
    {
        icons = {
            sortNone: "fa-sort",
            sortUp: "fa-sort-amount-asc",
            sortDown: "fa-sort-amount-desc",
            moveDrag: "fa-arrows-v",
            moveTop: "fa-angle-double-up",
            moveUp: "fa-angle-up",
            moveDown: "fa-angle-down",
            moveBottom: "fa-angle-double-down",
            groupNone: "fa-list",
            groupAscending: "fa-indent",
            groupDescending: "fa-outdent",
            groupOpen: "fa-chevron-circle-down",
            groupClosed: "fa-chevron-circle-right",
            filter: "fa-filter",
            close: "fa-times",
        }
    }
    else
    {
        icons = {
            sortNone: "fa-sort",
            sortUp: "fa-sort-amount-up",
            sortDown: "fa-sort-amount-down",
            moveDrag: "fa-arrows-v",
            moveTop: "fa-angle-double-up",
            moveUp: "fa-angle-up",
            moveDown: "fa-angle-down",
            moveBottom: "fa-angle-double-down",
            groupNone: "fa-list",
            groupAscending: "fa-indent",
            groupDescending: "fa-outdent",
            groupOpen: "fa-chevron-circle-down",
            groupClosed: "fa-chevron-circle-right",
            filter: "fa-filter",
            close: "fa-times",
        }
    }

    self._getId = function ($element)
    {
        var parentId = $element.parent().length == 0 ? "" : self._getId($element.parent());
        if ($element.attr("id"))
            return parentId + $element.attr("id");
        if ($element.attr("name"))
            return parentId + $element.attr("name");
        return parentId + $element[0].tagName;
    };

    self._save = function ()
    {
        $.cookie.json = true;
        $.cookie(self.cookieName, self.settings);
    };

    self.$element = $element;
    self.uniqueGroupId = Math.floor(Math.random() * 100000 + 1) * 10000;
    var _totals = self.$element.find(".grid-total");
    self.hasTotals = _totals.length > 0;
    var _subtotalsRowHtml = self.hasTotals ? $(_totals[0].outerHTML).removeClass("grid-total").addClass("grid-subtotal")[0].outerHTML : "";

    self.cookieName = "grid" + _bootstrapGridIndex;
    $.cookie.json = true;
    self.settings = $.cookie(self.cookieName);
    var _id = location.pathname.replace(/[^a-z]+/gi, "") + self.$element.data("grid-id") + self._getId($element);
    var _saved = (self.settings != undefined && self.settings.id == _id);
    if (!_saved)
    {
        self.settings = {
            id: _id,
            filterColumn: -1,
            filterValue: "",
            sortColumn: 0,
            sortOrder: 0,
            groupColumn: -1,
            groupOrder: 0
        };
    }

    self.sort = function ($clicked, index)
    {
        self.$element.find(".col-sort").html("<snap class='fa " + icons.sortNone + "'></span>");
        if (self.settings.sortColumn != index)
        {
            if ($clicked)
            {
                $clicked.html("<snap class='fa " + icons.sortUp + "'></span>");
            }
            self.settings.sortColumn = index;
            self.settings.sortOrder = 1;
            justReverse = false;
        }
        else
        {
            self.settings.sortOrder = -self.settings.sortOrder;
            if ($clicked)
            {
                if (self.settings.sortOrder == 1)
                {
                    $clicked.html("<snap class='fa " + icons.sortUp + "'></span>");
                }
                else
                {
                    $clicked.html("<snap class='fa " + icons.sortDown + "'></span>");
                }
            }
            justReverse = self.settings.groupOrder == 0;
        }
        self._groupAndSort(justReverse, false);
    };

    self.clearFilter = function ()
    {
        self.settings.filterColumn = -1;
        self.settings.filterValue = "";
        self._save();

        self.$element.find(".col-filter-input").addClass("hide");
        var $rows = self.$element.find(".grid-row,.grid-row-alt");
        $rows.data("isFiltered", false);
        self._paginate($rows);
    };

    self.filter = function ($clicked, index, value)
    {
        self.settings.filterColumn = index;
        self.settings.filterValue = value;
        self._save();

        var filterIndex = "data-filtervalue-" + index;
        var sortIndex = "data-sortvalue-" + index;
        value = value.toUpperCase();
        var $rows = self.$element.find(".grid-row,.grid-row-alt");
        $rows.each(function ()
        {
            var $row = $(this);
            var $filterValue = $row.attr(filterIndex);
            if ($filterValue == undefined)
            {
                $filterValue = $row.attr(sortIndex);
            }
            $row.data("isFiltered", $filterValue.indexOf(value) == -1);
        });
        self.$element.data("pagecurrent", 1);
        self.$element.find("[data-page-select]").removeClass("active");
        self.$element.find("[data-page-select='1']").addClass("active");
        self._paginate($rows);
    };

    self.attachMoveEvents = function ($moveableRows)
    {
        var url = self.$element.data("move-url");
        var buttons = self.$element.data("move-buttons");

        var isFunction = !/[^a-zA-Z0-9_]/.test(url) && eval("typeof " + url + " === 'function'");
        var dragcounter = 0;
        var draggingId = 0;
        var draggingGroupId = undefined;

        var htmlButtons = "<div class='grid-move-bar'><button type='button' class='btn btn-primary grid-move-btn grid-drag' title='Drag up or down'><span class='fa " + icons.moveDrag + "'></span></button>";
        if (buttons == "all" || buttons.indexOf("top") > -1)
        {
            htmlButtons += "<button type='button' class='btn btn-primary grid-move-btn grid-move-top' title='Move to top'><span class='fa " + icons.moveTop + "'></span></button>";
        }
        if (buttons == "all" || buttons.indexOf("up") > -1)
        {
            htmlButtons += "<button type='button' class='btn btn-primary grid-move-btn grid-move-up' title='Move up'><span class='fa " + icons.moveUp + "'></span></button>";
        }
        if (buttons == "all" || buttons.indexOf("down") > -1)
        {
            htmlButtons += "<button type='button' class='btn btn-primary grid-move-btn grid-move-down' title='Move down'><span class='fa " + icons.moveDown + "'></span></button>";
        }
        if (buttons == "all" || buttons.indexOf("bottom") > -1)
        {
            htmlButtons += "<button type='button' class='btn btn-primary grid-move-btn grid-move-bottom' title='Move to bottom'><span class='fa " + icons.moveBottom + "'></span></button></div>";
        }
        htmlButtons += "</div>";

        $moveableRows
            .prepend(htmlButtons)
            .on("dragenter", function (event)
            {
                var $this = $(this);
                event.preventDefault();
                event.stopPropagation();
                dragcounter++;
                self.$element.find(".drag-over").removeClass("drag-over");
                if (!$this.hasClass("dragging") && (draggingGroupId == null || $this.data("groupid") == draggingGroupId))
                {
                    $this.addClass("drag-over");
                }
            })
            .on("dragleave", function (event)
            {
                var $this = $(this);
                event.preventDefault();
                event.stopPropagation();
                dragcounter--;
                if (dragcounter === 0)
                {
                    $this.removeClass("drag-over");
                }
            })
            .on("dragover", function ()
            {
                var $this = $(this);
                return draggingId == undefined || $this.hasClass("dragging") || (draggingGroupId != undefined && $this.data("groupid") != draggingGroupId);
            })
            .on("drop", function (event)
            {
                event.preventDefault();
                event.stopPropagation();
                dragcounter = 0;
                self.$element.find(".drag-over").removeClass("drag-over");
                var rowId = $(this).closest(".row").data("rowid");
                if (isFunction)
                {
                    eval(url + "('" + encodeURIComponent(draggingId) + "','DragDrop','" + encodeURIComponent(rowId) + "')");
                }
                else
                {
                    location.href = url + "?id=" + encodeURIComponent(draggingId) + "&direction=DragDrop&dropId=" + encodeURIComponent(rowId);
                }
            });

        $moveableRows.find(".grid-drag")
            .prop("draggable", true)
            .on("dragstart", function ()
            {
                $(".tooltip").hide();
                var $row = $(this).closest(".row");
                draggingId = $row.data("rowid");
                draggingGroupId = (self.settings.groupColumn == -1 ? undefined : $row.data("groupid"));
                $row.addClass("dragging");
                self.$element.addClass('dragging');
            })
            .on("dragend", function ()
            {
                draggingId = undefined;
                $(this).closest(".row").removeClass("dragging");
                self.$element.removeClass('dragging');
            });

        $moveableRows.find(".grid-move-top").click(function ()
        {
            var rowId = $(this).closest(".row").data("rowid");
            if (isFunction)
            {
                eval(url + "('" + encodeURIComponent(draggingId) + "','Top')");
            }
            else
            {
                location.href = url + "?id=" + encodeURIComponent(rowId) + "&direction=Top";
            }
        });
        $moveableRows.find(".grid-move-bottom").click(function ()
        {
            var rowId = $(this).closest(".row").data("rowid");
            if (isFunction)
            {
                eval(url + "('" + encodeURIComponent(draggingId) + "','Bottom')");
            }
            else
            {
                location.href = url + "?id=" + encodeURIComponent(rowId) + "&direction=Bottom";
            }
        });
        $moveableRows.find(".grid-move-up").click(function ()
        {
            var rowId = $(this).closest(".row").data("rowid");
            if (isFunction)
            {
                eval(url + "('" + encodeURIComponent(draggingId) + "','Up')");
            }
            else
            {
                location.href = url + "?id=" + encodeURIComponent(rowId) + "&direction=Up";
            }
        });
        $moveableRows.find(".grid-move-down").click(function ()
        {
            var rowId = $(this).closest(".row").data("rowid");
            if (isFunction)
            {
                eval(url + "('" + encodeURIComponent(draggingId) + "','Down')");
            }
            else
            {
                location.href = url + "?id=" + encodeURIComponent(rowId) + "&direction=Down";
            }
        });
    };

    self.group = function ($clicked, index, expanded)
    {
        self.$element.find(".col-group").html("<snap class='fa " + icons.groupNone + "'></span>");
        if (self.settings.groupColumn != index || self.settings.groupOrder == 0)
        {
            self.settings.groupColumn = index;
            self.settings.groupOrder = 1;
            if ($clicked)
            {
                $clicked.html("<snap class='fa " + icons.groupAscending + "'></span>");
            }
        }
        else if (self.settings.groupOrder == 1)
        {
            self.settings.groupOrder = -1;
            if ($clicked)
            {
                $clicked.html("<snap class='fa " + icons.groupDescending + "'></span>");
            }
        }
        else
        {
            self.settings.groupColumn = -1;
            self.settings.groupOrder = 0;
            if ($clicked)
            {
                $clicked.html("<snap class='fa " + icons.groupNone + "'></span>");
            }
        }

        self._groupAndSort(false, expanded);
    };

    self._groupAndSort = function (justReverse, expanded)
    {
        self._save();

        self.$element.find(".grid-group").detach();
        self.$element.find(".grid-subtotal").detach();

        var sortIndex = "data-sortvalue-" + self.settings.sortColumn;
        var sortOrder = self.settings.sortOrder;
        var groupOrder = self.settings.groupOrder;

        var $rows = self.$element.find(".grid-row,.grid-row-alt");
        $rows.detach();
        var $footer = self.$element.find(".grid-footer");

        var $newRows;
        if (justReverse)
        {
            var newRows = $rows.get().reverse();
            self.$element.children(".grid-header:last").after(newRows);
            $newRows = $(newRows).removeClass("grid-group-hide");
        }
        else if (groupOrder == 0)
        {
            if (sortOrder == 0)
            {
                newRows = $rows;
            }
            else
            {
                newRows = $rows.get().sort(function (a, b)
                {
                    return a.attributes[sortIndex].value > b.attributes[sortIndex].value ? sortOrder : -sortOrder;
                });
            }
            self.$element.children(".grid-header:last").after(newRows);
            $newRows = $(newRows).removeClass("grid-group-hide");
            $footer.show();
        }
        else
        {
            var groupIndex = "data-filtervalue-" + self.settings.groupColumn;
            var sortedRows = $rows.get().sort(function (a, b)
            {
                var groupA = a.attributes[groupIndex].value;
                var groupB = b.attributes[groupIndex].value;
                if (groupA == groupB)
                    return sortOrder == 0 ? 0 : (a.attributes[sortIndex].value > b.attributes[sortIndex].value ? sortOrder : -sortOrder);

                return groupA > groupB ? groupOrder : -groupOrder;
            });
            var previousGroup;
            var $previousRow = self.$element.children(".grid-header:last");
            var groupExpand;
            var $subtotalsRow;
            for (index in sortedRows)
            {
                var row = sortedRows[index];
                var $row = $(row);
                var groupAttr = row.attributes[groupIndex];
                if (groupAttr == undefined)
                {
                    groupExpand = true;
                }
                else
                {
                    var group = groupAttr.value;
                    //var group = row.attributes[groupIndex].value;
                    if (group != previousGroup)
                    {
                        if (previousGroup && self.hasTotals)
                        {
                            $subtotalsRow = $(_subtotalsRowHtml);
                            $subtotalsRow.data("groupid", self.uniqueGroupId);
                            $previousRow.after($subtotalsRow);
                            $previousRow = $subtotalsRow;
                        }
                        self.uniqueGroupId++;
                        groupExpand = expanded != "none";
                        if (expanded == "first")
                        {
                            expanded = "none";
                        }
                        var $groupRow = $("<div class='grid-group row'><div class='col-xs-12'><span class='grid-group-icon'></span>&nbsp;" + (group == "" ? "(none)" : group) + "</div></div>");
                        var $groupSpan = $groupRow.find(".grid-group-icon");
                        $groupSpan.html("<snap class='fa " + (groupExpand ? icons.groupOpen : icons.groupClosed) + "'></span>");
                        $groupRow.data("groupid", self.uniqueGroupId);
                        $groupRow.data("collapsed", !groupExpand);
                        $groupRow.click(function ()
                        {
                            var $groupRow = $(this);
                            var $groupSpan = $groupRow.find(".grid-group-icon");
                            var groupId = $groupRow.data("groupid");
                            var collapsed = $groupRow.data("collapsed");
                            if (collapsed)
                            {
                                $groupRow.data("collapsed", false);
                                $groupSpan.html("<snap class='fa " + icons.groupOpen + "'></span>");
                                $("[data-groupid='" + groupId + "']").removeClass("grid-group-hide");
                            }
                            else
                            {
                                $groupRow.data("collapsed", true);
                                $groupSpan.html("<snap class='fa " + icons.groupClosed + "'></span>");
                                $("[data-groupid='" + groupId + "']").addClass("grid-group-hide");
                            }
                        });
                        $previousRow.after($groupRow);
                        $previousRow = $groupRow;
                        previousGroup = group;
                    }
                }
                if (!groupExpand)
                {
                    $row.addClass("grid-group-hide");
                }
                $row.attr("data-groupid", self.uniqueGroupId);
                $previousRow.after($row);
                $previousRow = $row;
            }
            if (previousGroup && self.hasTotals)
            {
                $subtotalsRow = $(_subtotalsRowHtml);
                $subtotalsRow.data("groupid", self.uniqueGroupId);
                $previousRow.after($subtotalsRow);
                $previousRow = $subtotalsRow;
            }
            $newRows = self.$element.find(".grid-row,.grid-row-alt");
            $footer.hide();
        }
        self._paginate($newRows);
    };

    self._paginate = function ($rows)
    {
        var currentPage = self.$element.data("pagecurrent");
        if (currentPage == undefined)
        {
            currentPage = 1;
        }
        var pageSize = self.$element.data("grid-pagesize");
        if (pageSize == undefined)
        {
            pageSize = 20;
        }
        var alt = true;
        var position = 0;
        var page = 1;
        var isGrouped = self.settings.groupOrder != 0;
        $rows.each(function ()
        {
            var $row = $(this);
            if ($row.data("isFiltered"))
            {
                $row.attr("data-page", 0);
                $row.addClass("hide");
            }
            else
            {
                $row.attr("data-page", page);
                if (page == currentPage || currentPage == 0 || isGrouped)
                {
                    $row.removeClass("hide");
                }
                else
                {
                    $row.addClass("hide");
                }
                alt = !alt;
                if (alt)
                {
                    $row.addClass("grid-row-alt").removeClass("grid-row");
                }
                else
                {
                    $row.addClass("grid-row").removeClass("grid-row-alt");
                }
                position++;
                if (position == pageSize && pageSize != 0)
                {
                    position = 0;
                    page++;
                }
            }
        });
        if (pageSize != 0)
        {
            var lastPage = Math.ceil($rows.length / pageSize);
            if (position == 0)
            {
                page--;
            }
            for (var loop = 1; loop <= lastPage; loop++)
            {
                if (loop <= page && !isGrouped)
                {
                    self.$element.find("[data-page-select='" + loop + "']").removeClass("hide");
                }
                else
                {
                    self.$element.find("[data-page-select='" + loop + "']").addClass("hide");
                }
            }
        }
        self._updateTotals();
    };

    self._updateTotals = function ()
    {
        if (!self.hasTotals)
            return;

        self.$element.find("[data-total]").each(function ()
        {
            var $column = $(this);
            var totalIndex = "data-sortvalue-" + $column.data("total");
            var type = $column.data("total-type");
            var sub = $column.data("total-sub");
            var subIndex = "data-subvalue-" + $column.data("total");
            var groupId = $column.parent().data("groupid");

            var filter = "[data-page!=0][" + totalIndex + "]";
            if (groupId)
            {
                filter += "[data-groupid=" + groupId + "]";
            }
            var results = {};
            var counts = {};
            if (type == "count" && !sub)
            {
                results[""] = self.$element.find(filter).length;
            }
            else
            {
                self.$element.find(filter).each(function ()
                {
                    //if (this.attributes["data-page"].value == "0")
                    //    return;
                    var subValue = sub ? this.attributes[subIndex].value : "";
                    var value = parseFloat(this.attributes[totalIndex].value);
                    if (results[subValue] == undefined)
                    {
                        counts[subValue] = 0;
                        switch (type)
                        {
                            case "min":
                                results[subValue] = Infinity;
                                break;
                            case "max":
                                results[subValue] = -Infinity;
                                break;
                            default:
                                results[subValue] = 0;
                                break;
                        }
                    }
                    counts[subValue]++;
                    switch (type)
                    {
                        case "sum":
                            results[subValue] += value;
                            break;
                        case "average":
                            results[subValue] += value;
                            break;
                        case "min":
                            if (value < results[subValue])
                            {
                                results[subValue] = value;
                            }
                            break;
                        case "max":
                            if (value > results[subValue])
                            {
                                results[subValue] = value;
                            }
                            break;
                    }
                });
            }

            var style = $column.data("total-style");
            var html = "";
            for (var index in results)
            {
                var result = results[index];
                if (type == "average" && counts[index] > 0)
                {
                    result /= counts[index];
                }
                if (html.length > 0)
                {
                    html += "</br>";
                }
                if (style != "currency")
                {
                    html += index + result;
                }
                else if (index == "")
                {
                    html += "&#163;" + result.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
                }
                else
                {
                    html += index + result.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
                }
            }
            $column.html(html);
        });
    };

    var $moveableRows = self.$element.find("[data-rowid]");
    if ($moveableRows.length > 0)
    {
        self.attachMoveEvents($moveableRows);
    }

    var $automaticFilter;
    self.$element.find("[data-filterable]").each(function ()
    {
        var $column = $(this);
        var index = $column.data("filterable");
        var type = $column.data("filterable-type");
        var $span = $column.children("span");
        var name = $span.text();
        if (name == "")
        {
            name = $span.data("tip");
        }

        if (type != "force")
        {
            var $filterIcon = $(document.createElement("span"));
            $filterIcon.addClass("col-filter");
            $filterIcon.html("<snap class='fa " + icons.filter + "'></span>");
            $filterIcon.attr("title", "Filter by " + name);
            $filterIcon.click(function ()
            {
                var $current = $column.children(".col-filter-input");
                var isHidden = $current.hasClass("hide");
                self.clearFilter($(this));
                if (isHidden)
                {
                    $current.removeClass("hide");
                    $current.children("input").val("");
                }
            });
            $column.prepend($filterIcon);
        }

        var $filterGroup = $(document.createElement("div"));
        $filterGroup.addClass("input-group col-filter-input hide");
        if (type || $automaticFilter == undefined)
        {
            $automaticFilter = $filterGroup;
        }
        $column.append($filterGroup);

        var $filterInput = $(document.createElement("input"));
        $filterInput.addClass("form-control");
        $filterInput.attr("type", "text");
        $filterInput.attr("placeholder", "Filter by " + name);
        $filterInput.keyup(function ()
        {
            var value = $(this).val();
            self.filter($(this), index, value);
        });
        $filterGroup.append($filterInput);

        var $filterSpan = $(document.createElement("span"));
        $filterSpan.addClass("input-group-btn");
        $filterGroup.append($filterSpan);

        var $filterButton = $(document.createElement("button"));
        $filterButton.addClass("btn btn-default");
        $filterButton.attr("title", "Clear the filter");
        $filterButton.attr("type", "button");
        $filterButton.click(function ()
        {
            self.clearFilter($(this));
        });
        $filterSpan.append($filterButton);

        var $filterButtonIcon = $(document.createElement("span"));
        $filterButtonIcon.addClass("fa " + icons.close);
        $filterButton.append($filterButtonIcon);
    });
    if (self.settings.filterColumn != -1)
    {
        var $automaticColumn = $("[data-filterable='" + self.settings.filterColumn + "']");
        $automaticFilter = $automaticColumn.children("div:last-child");
        var $automaticInput = $automaticFilter.children("input");
        $automaticInput.val(self.settings.filterValue);
        self.filter($automaticInput, self.settings.filterColumn, self.settings.filterValue);
    }
    if ($automaticFilter != undefined)
    {
        $automaticFilter.removeClass("hide");
    }

    var automaticSortIndex = 0;
    var $automaticSortIcon;
    self.$element.find("[data-sortable]").each(function ()
    {
        var $column = $(this);
        var index = $column.data("sortable");
        var type = $column.data("sortable-type");

        var $sortIcon;
        if (type != "force")
        {
            var $span = $column.children("span");
            var name = $span.text();
            if (name == "")
            {
                name = $span.data("tip");
            }
            $sortIcon = $(document.createElement("span"));
            $sortIcon.addClass("col-sort");
            $sortIcon.html("<snap class='fa " + icons.sortNone + "'></span>");
            $sortIcon.attr("title", "Sort by " + name);
            $sortIcon.click(function ()
            {
                self.sort($(this), index);
            });
            $column.prepend($sortIcon);
        }
        if (type || automaticSortIndex == 0)
        {
            automaticSortIndex = index;
            $automaticSortIcon = $sortIcon;
        }
    });
    if (!_saved && automaticSortIndex != 0)
    {
        self.sort($automaticSortIcon, automaticSortIndex);
    }

    var automaticGroupIndex = -1;
    var automaticGroupExpanded;
    var $automaticGroupIcon;
    self.$element.find("[data-groupable]").each(function ()
    {
        var $column = $(this);
        var index = $column.data("groupable");
        var type = $column.data("groupable-type");
        var expanded = $column.data("groupable-expanded") || "first";

        var $groupIcon;
        if (type != "force")
        {
            var $span = $column.children("span");
            var name = $span.text();
            if (name == "")
            {
                name = $span.data("tip");
            }
            $groupIcon = $(document.createElement("span"));
            $groupIcon.addClass("col-group");
            $groupIcon.html("<snap class='fa " + icons.groupNone + "'></span>");
            $groupIcon.attr("title", "Group by " + name);
            $groupIcon.click(function ()
            {
                self.group($(this), index, expanded);
            });
            $column.prepend($groupIcon);
        }
        if (type)
        {
            automaticGroupIndex = index;
            $automaticGroupIcon = $groupIcon;
            automaticGroupExpanded = expanded;
        }
    });
    if (!_saved && automaticGroupIndex != -1)
    {
        self.group($automaticGroupIcon, automaticGroupIndex, automaticGroupExpanded);
    }

    if (_saved)
    {
        self.$element.find(".col-sort").html("<snap class='fa " + icons.sortNone + "'></span>");
        if (self.settings.sortOrder == 1)
        {
            self.$element.find("[data-sortable=" + self.settings.sortColumn + "] .col-sort").html("<snap class='fa " + icons.sortUp + "'></span>");
        }
        else if (self.settings.sortOrder == -1)
        {
            self.$element.find("[data-sortable=" + self.settings.sortColumn + "] .col-sort").html("<snap class='fa " + icons.sortDown + "'></span>");
        }

        self.$element.find(".col-group").html("<snap class='fa " + icons.groupNone + "'></span>");
        if (self.settings.groupOrder == 1)
        {
            self.$element.find("[data-groupable=" + self.settings.groupColumn + "] .col-group").html("<snap class='fa " + icons.groupAscending + "'></span>");
        }
        else if (self.settings.groupOrder == -1)
        {
            self.$element.find("[data-groupable=" + self.settings.groupColumn + "] .col-group").html("<snap class='fa " + icons.groupDescending + "'></span>");
        }
        self._groupAndSort(false, automaticGroupExpanded);
    }
    self._updateTotals();
};
