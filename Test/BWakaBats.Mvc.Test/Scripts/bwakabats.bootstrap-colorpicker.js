var BootstrapColorPicker = function ($element, $button, $popup, $name)
{
    var self = this;
    self.$element = $element;
    self.$button = $button;
    self.$popup = $popup;
    self.$name = $name;

    self.knownColors = [
        { name: "Black", h: 0, s: 0, l: 0 },
        { name: "Dim Gray", h: 0, s: 0, l: 0.4118 },
        { name: "Gray", h: 0, s: 0, l: 0.502 },
        { name: "Dark Gray", h: 0, s: 0, l: 0.6627 },
        { name: "Silver", h: 0, s: 0, l: 0.7529 },
        { name: "Light Gray", h: 0, s: 0, l: 0.8275 },
        { name: "Gainsboro", h: 0, s: 0, l: 0.8627 },
        { name: "White Smoke", h: 0, s: 0, l: 0.9608 },
        { name: "White", h: 0, s: 0, l: 1 },
        { name: "Rosy Brown", h: 0, s: 0.2514, l: 0.649 },
        { name: "Indian Red", h: 0, s: 0.5305, l: 0.5824 },
        { name: "Brown", h: 0, s: 0.5942, l: 0.4059 },
        { name: "Firebrick", h: 0, s: 0.6792, l: 0.4157 },
        { name: "Light Coral", h: 0, s: 0.7887, l: 0.7216 },
        { name: "Maroon", h: 0, s: 1, l: 0.251 },
        { name: "Dark Red", h: 0, s: 1, l: 0.2725 },
        { name: "Red", h: 0, s: 1, l: 0.5 },
        { name: "Snow", h: 0, s: 1, l: 0.9902 },
        { name: "Misty Rose", h: 0.0167, s: 1, l: 0.9412 },
        { name: "Salmon", h: 0.0172, s: 0.9315, l: 0.7137 },
        { name: "Tomato", h: 0.0254, s: 1, l: 0.6392 },
        { name: "Dark Salmon", h: 0.042, s: 0.7161, l: 0.6961 },
        { name: "Coral", h: 0.0448, s: 1, l: 0.6569 },
        { name: "Orange Red", h: 0.0451, s: 1, l: 0.5 },
        { name: "Light Salmon", h: 0.0476, s: 1, l: 0.7392 },
        { name: "Sienna", h: 0.0536, s: 0.561, l: 0.402 },
        { name: "Sea Shell", h: 0.0686, s: 1, l: 0.9667 },
        { name: "Chocolate", h: 0.0694, s: 0.75, l: 0.4706 },
        { name: "Saddle Brown", h: 0.0694, s: 0.7595, l: 0.3098 },
        { name: "Sandy Brown", h: 0.0766, s: 0.8706, l: 0.6667 },
        { name: "Peach Puff", h: 0.0786, s: 1, l: 0.8627 },
        { name: "Peru", h: 0.0822, s: 0.5868, l: 0.5255 },
        { name: "Linen", h: 0.0833, s: 0.6667, l: 0.9412 },
        { name: "Bisque", h: 0.0904, s: 1, l: 0.8843 },
        { name: "Dark Orange", h: 0.0915, s: 1, l: 0.5 },
        { name: "Burly Wood", h: 0.0939, s: 0.5686, l: 0.7 },
        { name: "Tan", h: 0.0952, s: 0.4375, l: 0.6863 },
        { name: "Antique White", h: 0.0952, s: 0.7778, l: 0.9118 },
        { name: "Navajo White", h: 0.0996, s: 1, l: 0.8392 },
        { name: "Blanched Almond", h: 0.1, s: 1, l: 0.902 },
        { name: "Papaya Whip", h: 0.1032, s: 1, l: 0.9176 },
        { name: "Moccasin", h: 0.1059, s: 1, l: 0.8549 },
        { name: "Orange", h: 0.1078, s: 1, l: 0.5 },
        { name: "Wheat", h: 0.1086, s: 0.7674, l: 0.8314 },
        { name: "Old Lace", h: 0.1087, s: 0.8519, l: 0.9471 },
        { name: "Floral White", h: 0.1111, s: 1, l: 0.9706 },
        { name: "Dark Goldenrod", h: 0.1185, s: 0.8872, l: 0.3824 },
        { name: "Goldenrod", h: 0.1192, s: 0.744, l: 0.4902 },
        { name: "Cornsilk", h: 0.1333, s: 1, l: 0.9314 },
        { name: "Gold", h: 0.1405, s: 1, l: 0.5 },
        { name: "Khaki", h: 0.15, s: 0.7692, l: 0.7451 },
        { name: "Lemon Chiffon", h: 0.15, s: 1, l: 0.902 },
        { name: "Pale Goldenrod", h: 0.152, s: 0.6667, l: 0.8 },
        { name: "Dark Khaki", h: 0.1545, s: 0.3832, l: 0.5804 },
        { name: "Beige", h: 0.1667, s: 0.5556, l: 0.9118 },
        { name: "Light Goldenrod Yellow", h: 0.1667, s: 0.8, l: 0.902 },
        { name: "Olive", h: 0.1667, s: 1, l: 0.251 },
        { name: "Yellow", h: 0.1667, s: 1, l: 0.5 },
        { name: "Light Yellow", h: 0.1667, s: 1, l: 0.9392 },
        { name: "Ivory", h: 0.1667, s: 1, l: 0.9706 },
        { name: "Olive Drab", h: 0.2212, s: 0.6045, l: 0.3471 },
        { name: "Yellow Green", h: 0.2215, s: 0.6078, l: 0.5 },
        { name: "Dark Olive Green", h: 0.2278, s: 0.3896, l: 0.302 },
        { name: "Green Yellow", h: 0.2324, s: 1, l: 0.5922 },
        { name: "Chartreuse", h: 0.2503, s: 1, l: 0.5 },
        { name: "Lawn Green", h: 0.2513, s: 1, l: 0.4941 },
        { name: "Dark Sea Green", h: 0.3197, s: 0.2678, l: 0.6412 },
        { name: "Forest Green", h: 0.3333, s: 0.6069, l: 0.3392 },
        { name: "Lime Green", h: 0.3333, s: 0.6078, l: 0.5 },
        { name: "Light Green", h: 0.3333, s: 0.7344, l: 0.749 },
        { name: "Pale Green", h: 0.3333, s: 0.9252, l: 0.7902 },
        { name: "Dark Green", h: 0.3333, s: 1, l: 0.1961 },
        { name: "Green", h: 0.3333, s: 1, l: 0.251 },
        { name: "Lime", h: 0.3333, s: 1, l: 0.5 },
        { name: "Honeydew", h: 0.3333, s: 1, l: 0.9706 },
        { name: "Sea Green", h: 0.4068, s: 0.5027, l: 0.3627 },
        { name: "Medium Sea Green", h: 0.4076, s: 0.4979, l: 0.4686 },
        { name: "Spring Green", h: 0.4163, s: 1, l: 0.5 },
        { name: "Mint Cream", h: 0.4167, s: 1, l: 0.9804 },
        { name: "Medium Spring Green", h: 0.436, s: 1, l: 0.4902 },
        { name: "Medium Aquamarine", h: 0.4434, s: 0.5074, l: 0.602 },
        { name: "Aquamarine", h: 0.444, s: 1, l: 0.749 },
        { name: "Turquoise", h: 0.4833, s: 0.7207, l: 0.5647 },
        { name: "Light Sea Green", h: 0.4909, s: 0.6952, l: 0.4118 },
        { name: "Medium Turquoise", h: 0.4939, s: 0.5983, l: 0.551 },
        { name: "Dark Slate Gray", h: 0.5, s: 0.254, l: 0.2471 },
        { name: "Pale Turquoise", h: 0.5, s: 0.6495, l: 0.8098 },
        { name: "Teal", h: 0.5, s: 1, l: 0.251 },
        { name: "Dark Cyan", h: 0.5, s: 1, l: 0.2725 },
        { name: "Aqua", h: 0.5, s: 1, l: 0.5 },
        { name: "Cyan", h: 0.5, s: 1, l: 0.5 },
        { name: "Light Cyan", h: 0.5, s: 1, l: 0.9392 },
        { name: "Azure", h: 0.5, s: 1, l: 0.9706 },
        { name: "Dark Turquoise", h: 0.5024, s: 1, l: 0.4098 },
        { name: "Cadet Blue", h: 0.5051, s: 0.2549, l: 0.5 },
        { name: "Powder Blue", h: 0.5185, s: 0.5192, l: 0.7961 },
        { name: "Light Blue", h: 0.5409, s: 0.5327, l: 0.7902 },
        { name: "Deep Sky Blue", h: 0.5418, s: 1, l: 0.5 },
        { name: "Sky Blue", h: 0.5483, s: 0.7143, l: 0.7255 },
        { name: "Light Sky Blue", h: 0.5638, s: 0.92, l: 0.7549 },
        { name: "Steel Blue", h: 0.5758, s: 0.44, l: 0.4902 },
        { name: "Alice Blue", h: 0.5778, s: 1, l: 0.9706 },
        { name: "Slate Gray", h: 0.5833, s: 0.126, l: 0.502 },
        { name: "Light Slate Gray", h: 0.5833, s: 0.1429, l: 0.5333 },
        { name: "Dodger Blue", h: 0.5822, s: 1, l: 0.5588 },
        { name: "Light Steel Blue", h: 0.5942, s: 0.4107, l: 0.7804 },
        { name: "Cornflower Blue", h: 0.6071, s: 0.7919, l: 0.6608 },
        { name: "Royal Blue", h: 0.625, s: 0.7273, l: 0.5686 },
        { name: "Midnight Blue", h: 0.6667, s: 0.635, l: 0.2686 },
        { name: "Lavender", h: 0.6667, s: 0.6667, l: 0.9412 },
        { name: "Navy", h: 0.6667, s: 1, l: 0.251 },
        { name: "Dark Blue", h: 0.6667, s: 1, l: 0.2725 },
        { name: "Medium Blue", h: 0.6667, s: 1, l: 0.402 },
        { name: "Blue", h: 0.6667, s: 1, l: 0.5 },
        { name: "Ghost White", h: 0.6667, s: 1, l: 0.9863 },
        { name: "Dark Slate Blue", h: 0.6902, s: 0.39, l: 0.3922 },
        { name: "Slate Blue", h: 0.6899, s: 0.5349, l: 0.5784 },
        { name: "Medium Slate Blue", h: 0.6903, s: 0.7976, l: 0.6706 },
        { name: "Medium Purple", h: 0.7212, s: 0.5978, l: 0.649 },
        { name: "Blue Violet", h: 0.7532, s: 0.7593, l: 0.5275 },
        { name: "Indigo", h: 0.7628, s: 1, l: 0.2549 },
        { name: "Dark Orchid", h: 0.7781, s: 0.6063, l: 0.498 },
        { name: "Dark Violet", h: 0.7836, s: 1, l: 0.4137 },
        { name: "Medium Orchid", h: 0.8003, s: 0.5888, l: 0.5804 },
        { name: "Thistle", h: 0.8333, s: 0.2427, l: 0.798 },
        { name: "Plum", h: 0.8333, s: 0.4729, l: 0.7471 },
        { name: "Violet", h: 0.8333, s: 0.7606, l: 0.7216 },
        { name: "Purple", h: 0.8333, s: 1, l: 0.251 },
        { name: "Dark Magenta", h: 0.8333, s: 1, l: 0.2725 },
        { name: "Fuchsia", h: 0.8333, s: 1, l: 0.5 },
        { name: "Magenta", h: 0.8333, s: 1, l: 0.5 },
        { name: "Orchid", h: 0.8396, s: 0.5889, l: 0.6471 },
        { name: "Medium Violet Red", h: 0.8951, s: 0.8091, l: 0.4314 },
        { name: "Deep Pink", h: 0.9099, s: 1, l: 0.5392 },
        { name: "Hot Pink", h: 0.9167, s: 1, l: 0.7059 },
        { name: "Pale Violet Red", h: 0.9455, s: 0.5978, l: 0.649 },
        { name: "Lavender Blush", h: 0.9444, s: 1, l: 0.9706 },
        { name: "Crimson", h: 0.9667, s: 0.8333, l: 0.4706 },
        { name: "Pink", h: 0.9709, s: 1, l: 0.8765 },
        { name: "Light Pink", h: 0.9749, s: 1, l: 0.8569 }
    ];

    self.$buttonCaret = $button.children(".caret");
    self.$spectrum = $popup.children(".colorpicker-spectrum");
    self.$none = $popup.find(".colorpicker-none").children("a");
    self.$hex = $popup.find(".colorpicker-hex");
    self.$more = $popup.find(".colorpicker-more").children("a");
    self.length = self.$element.attr("maxlength");
    self.normalType = $element.data("colorpicker");
    self.additionalType = $element.data("colorpicker-additional");
    self.range = $element.data("colorpicker-range");
    self.type = self.normalType;
    self.none = self.$none.html();
    self.hue = 0;

    $popup.addClass(self.range).addClass(self.type);
    self.$none.click(function ()
    {
        self.update();
        $(document).click();
        event.preventDefault();
        return false;
    });
    self.$more.click(function ()
    {
        $popup.removeClass(self.type);
        if (self.type == self.normalType)
        {
            self.type = self.additionalType;
            self.$more.html("Less");
        }
        else
        {
            self.type = self.normalType;
            self.$more.html("More");
        }
        $popup.addClass(self.type);
        event.preventDefault();
        return false;
    });
    $popup.children(".colorpicker-grey")
        .mousemove(function ()
        {
            var coordinate = self._getCoordinate($(this), event);
            var l = coordinate.x;
            if (self.type == "simple")
            {
                l /= 0.875;
            }
            self.printHsl(0, 0, l);
        })
        .click(function ()
        {
            var coordinate = self._getCoordinate($(this), event);
            var l = coordinate.x;
            if (self.type == "simple")
            {
                l /= 0.875;
            }
            self.updateHsl(0, 0, l);
        });
    $popup.children(".colorpicker-hue")
        .mousemove(function ()
        {
            var coordinate = self._getCoordinate($(this), event);
            var h = self._getHue(coordinate.x);
            self.printHsl(h, 1, .5);
        })
        .click(function ()
        {
            var coordinate = self._getCoordinate($(this), event);
            var h = self._getHue(coordinate.x);
            var backcolor = self.updateHsl(h, 1, .5);
            if (self.type == "full")
            {
                self.hue = h;
                self.$spectrum.css("background-color", "#" + backcolor);
                event.preventDefault();
                return false;
            }
        });
    self.$spectrum
        .mousemove(function ()
        {
            var coordinate = self._getCoordinate($(this), event);
            if (self.type == "full")
            {
                self.printHsl(self.hue, 1 - coordinate.y, coordinate.x);
                return;
            }
            var h = self._getHue(coordinate.x);
            var l = 1 - coordinate.y;
            self.printHsl(h, 1, l);
        })
        .click(function ()
        {
            var coordinate = self._getCoordinate($(this), event);
            if (self.type == "full")
            {
                self.updateHsl(self.hue, 1 - coordinate.y, coordinate.x);
                return;
            }
            var h = self._getHue(coordinate.x);
            var l = 1 - coordinate.y;
            self.updateHsl(h, 1, l);
        });

    self._getCoordinate = function ($target, event)
    {
        var offsetLeft = $target.offset().left - $(window).scrollLeft();
        var offsetTop = $target.offset().top - $(window).scrollTop();
        var x = (event.clientX - offsetLeft) / $target.width();
        var y = (event.clientY - offsetTop) / $target.height();

        if (self.type == "simple")
        {
            x = Math.floor(x * 8) / 8;
            if (y < .5)
            {
                y = Math.floor(y * 8 + 1) / 10;
            }
            else
            {
                y = Math.floor(y * 8 + 2) / 10;
            }
        }

        return { x: x, y: y };
    };

    self._getHue = function (x)
    {
        // Convert X co-ordinate (now in range 0 to 8) to hue (this is non-linear)
        // X        Hue
        // 0.000    0.000
        // 0.125    0.085
        // 0.250    0.166 <<<< gradient change
        // 0.375    0.333
        // 0.500    0.500
        // 0.625    0.666 <<<< gradient change
        // 0.750    0.750
        // 0.875    0.833 <<<< gradient change
        // 1.000    1.000
        return x < .25 ? (x + 0.000) * 0.6667 :
            (x < 0.625 ? (x - 0.125) * 1.3333 :
            (x < 0.875 ? (x + 0.375) * 0.6667 :
                         (x - 0.250) * 1.3333));
    };

    self.update = function (hex)
    {
        if (hex === undefined || hex == null || hex == "")
        {
            self.updateHsl();
            return;
        }

        var bigint = parseInt(hex, 16);
        var r;
        var g;
        var b;
        if (hex.length <= 3)
        {
            r = ((bigint >> 8) & 15) / 15;
            g = ((bigint >> 4) & 15) / 15;
            b = (bigint & 15) / 15;
        }
        else
        {
            r = ((bigint >> 16) & 255) / 255;
            g = ((bigint >> 8) & 255) / 255;
            b = (bigint & 255) / 255;
        }
        return self.updateRgb(r, g, b);
    };

    self.updateRgb = function (r, g, b)
    {
        var max = Math.max(r, g, b);
        var min = Math.min(r, g, b);
        var h;
        var s;
        var l;
        if (max == min)
        {
            // achromatic
            h = 0;
            s = 0;
            l = max;
        }
        else
        {
            l = (max + min) / 2;
            var d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            switch (max)
            {
                case r: h = (g - b) / d + (g < b ? 6 : 0); break;
                case g: h = (b - r) / d + 2; break;
                case b: h = (r - g) / d + 4; break;
            }
            h /= 6;
        }
        return self.updateHsl(h, s, l);
    };

    self.printHsl = function (h, s, l)
    {
        var value;
        if (h === undefined)
        {
            value = "FFF";
        }
        else
        {
            l = self._adjustLightness(l, self.range);
            var color = self._hsl2rgb(h, s, l);
            value = self._color2hex(color);
        }
        self.$hex.html(value);
    };

    self.updateHsl = function (h, s, l)
    {
        var value;
        var backcolor;
        var forecolor;
        if (h === undefined)
        {
            value = null;
            backcolor = "FFF";
            forecolor = "CCC";
            self.$name.html(self.none);
        }
        else
        {
            l = self._adjustLightness(l, self.range);
            var color = self._hsl2rgb(h, s, l);
            value = self._color2hex(color);
            backcolor = value;
            forecolor = (color.r + color.g * 2 + color.b > 512) ? "333" : "FFF";

            var best = 100000;
            var name = "";
            for (var index in self.knownColors)
            {
                var knownColor = self.knownColors[index];
                var dh = knownColor.h - h;
                var ds = knownColor.s - s;
                var dl = knownColor.l - l;
                if (dh > .5)
                {
                    dh = 1 - dh;
                }
                else if (dh < -.5)
                {
                    dh = 1 + dh;
                }
                var d = dh * dh * 4 + ds * ds + dl * dl;
                if (d < best)
                {
                    best = d;
                    name = knownColor.name;
                }
            }
            self.$name.html(name);
        }

        self.$element.val(value).trigger('change');
        self.$button.css("background-color", "#" + backcolor);
        self.$button.css("color", "#" + forecolor);
        self.$buttonCaret.css("border-top-color", "#" + forecolor);

        return backcolor;
    };

    self._adjustLightness = function (l, range)
    {
        switch (range)
        {
            case "light":
                return l / 2 + .5;
            case "dark":
                return l / 2;
        }
        return l;
    };

    self._hsl2rgb = function (h, s, l)
    {
        var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        var p = 2 * l - q;
        var r = Math.round(self._hue2rgb(p, q, h + 0.3333) * 255);
        var g = Math.round(self._hue2rgb(p, q, h + 0.0000) * 255);
        var b = Math.round(self._hue2rgb(p, q, h - 0.3333) * 255);
        return { r: r, g: g, b: b };
    };

    self._hue2rgb = function (p, q, t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 0.1667) return p + (q - p) * 6 * t;
        if (t < 0.5000) return q;
        if (t < 0.6667) return p + (q - p) * (2 / 3 - t) * 6;
        return p;
    };

    self._color2hex = function (color)
    {
        if (self.length < 6)
        {
            var toHexShort = function (x)
            {
                return (parseInt(x / 17).toString(16).toUpperCase());
            };
            return toHexShort(color.r) + toHexShort(color.g) + toHexShort(color.b);
        }
        var toHexLong = function (x)
        {
            return ("0" + parseInt(x).toString(16).toUpperCase()).slice(-2);
        };
        return toHexLong(color.r) + toHexLong(color.g) + toHexLong(color.b);
    };
};
