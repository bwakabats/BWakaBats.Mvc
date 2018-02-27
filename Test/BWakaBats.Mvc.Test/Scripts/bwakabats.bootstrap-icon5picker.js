var BootstrapIcon5Picker = function ($element, $button, $popup, $icon, $name)
{
    var self = this;
    self.$element = $element;
    self.$button = $button;
    self.$popup = $popup;
    self.$icon = $icon;
    self.$name = $name;

    self.$searchTextBox = $popup.find(".icon5picker-popup-header input");
    self.$results = $popup.find(".icon5picker-popup-results");

    self.optionalLabel = $element.data("icon5picker");
    self.isFree = $element.data("icon5picker-free");

    self._search = function ()
    {
        results = self.optionalLabel === "" ? "" : ("<span data-index='-1' class='optional'>" + self.optionalLabel + "</span>");
        var searchText = self.$searchTextBox.val();
        var count = 0;
        if (searchText !== "")
        {
            var words = searchText.split(" ");
            bootstrapIcon5PickerMetadata.some(function (icon, index)
            {
                if (icon.isFree || !self.isFree)
                {
                    var keywords = icon.title.toLowerCase() + " " + icon.keywords.toLowerCase();
                    var every = words.every(function (word)
                    {
                        return keywords.indexOf(word.toLowerCase()) > -1;
                    });
                    if (every)
                    {
                        results += "<span data-index='" + index + "'><i class='" + icon.className + " fa-fw'></i></span>";
                        count++;
                        return count >= 388;
                    }
                }
            });
            if (count === 0)
            {
                results += " <span class='small'>Sorry, no matching icons found.</span>";
            }
        }
        else
        {
            results += " <span class='small'>Type a word above to search.</span>";
        }
        var size;
        if (count <= 16)
        {
            size = "icon-lg";
        }
        else if (count <= 190)
        {
            size = "icon-md";
        }
        else
        {
            size = "icon-sm";
        }
        self.$results.removeClass("icon-sm icon-md icon-lg").addClass(size).html(results);
        self.$results.find("span").click(function ()
        {
            var $this = $(this);
            var index = $this.data("index");
            if (index === -1)
            {
                self.$icon.addClass("fa-none").html("<i class='fas fa-times fa-fw' data-fa-transform='grow-8'>");
                self.$name.html(self.optionalLabel);
                self.$element.val("");
            }
            else
            {
                var icon = bootstrapIcon5PickerMetadata[index];
                self.$icon.removeClass("fa-none").html("<i class='" + icon.className + " fa-fw' data-fa-transform='grow-8'>");
                self.$name.html(icon.title);
                self.$element.val(icon.className);
            }
        });
    }
    self.$searchTextBox.change(self._search);
    self.$searchTextBox.keypress(self._search);
    self.$searchTextBox.keyup(self._search);
    self.$searchTextBox.keydown(self._search);
    self._search();
};
