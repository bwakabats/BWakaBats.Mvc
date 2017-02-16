var BootstrapAddressLookup = function ($element, $button, $addressBoxes)
{
    var self = this;
    self.$element = $element;
    self.$addressBoxes = $addressBoxes;
    var lookupUrl = self.$element.data("address-lookupurl");
    var func = self.$element.data("address-func");

    var id = self.$element.attr("id");
    self.$button = $element.parent().find("#" + id + "_Button");
    if (self.$button.length > 0)
    {
        var externalUrl = self.$element.data("address-externalurl");
        self.$button.click(function ()
        {
            var $this = $(this);
            var latitude = $this.data("address-latitude");
            var longitude = $this.data("address-longitude");
            url = externalUrl.replace("{lat}", latitude).replace("{lng}", longitude);
            window.open(url);
        });
    }

    self._lookup = function (postcode, undefined, cd)
    {
        if (lookupUrl == null)
        {
            if (cd != undefined)
            {
                cd();
            }
            utilities.ajax("Address Lookup", "http://api.postcodes.io/postcodes/" + postcode, function (data)
            {
                var result = data.result;
                var addresses = [];
                var address = {
                    AddressLine1: "",
                    AddressLine2: "",
                    AddressLine3: "",
                    AddressLine4: "",
                    City: result.admin_district != null ? result.admin_district : "",
                    County: result.admin_county != null ? result.admin_county : result.nuts,
                    Country: result.country != null ? result.country : "United Kingdom",
                    Postcode: result.postcode,
                    Latitude: result.latitude,
                    Longitude: result.longitude
                };
                addresses.push(address);
                self._found(addresses);
            }, function () { });
        }
        else
        {
            var url = lookupUrl.replace("{pc}", postcode);
            $.ajax({ async: true, url: url })
                 .done(function (data)
                 {
                     if (func == null || func == "" || func == undefined)
                     {
                         self._found(data, cd);
                     }
                     else
                     {
                         var addresses = eval(func + "(data)");
                         self._found(addresses, cd);
                     }
                 })
                .fail(function ()
                {
                    if (cd != undefined)
                    {
                        cd();
                    }
                });
        }
    };

    self._found = function (addresses, cd)
    {
        if (addresses.length == 1 || cd == undefined)
        {
            self._selected(addresses[0]);
            if (cd != undefined)
            {
                cd();
            }
        }
        else
        {
            cd(addresses);
        }
    };

    self.typeahead = self.$element.typeahead(
        {
            minLength: 6,
            highlight: false
        },
        {
            name: self.$element.attr("name"),
            limit: 100,
            source: self._lookup,
            display: function (item) { return item.Postcode; },
            templates: { suggestion: function (item) { return "<div>" + item.AddressLine1 + "</div>"; } }
        }
    );

    self.$element.on('typeahead:selected', function (e, address)
    {
        self._selected(address);
    });

    self._selected = function (address)
    {
        self.$addressBoxes.each(function ()
        {
            $addressBox = $(this);
            var rep = function (value, find, replace)
            {
                return value.replace(find, (replace == null ? "" : replace));
            };
            var value = $addressBox.data("address-part");
            switch (value)
            {
                case "map":
                    var map = $addressBox.data("map");
                    map.panTo(address.Latitude, address.Longitude);
                    break;
                case "url":
                    $addressBox.data("address-latitude", address.Latitude);
                    $addressBox.data("address-longitude", address.Longitude);
                    break;
                default:
                    if ($element.val() != previousPostCode)
                    {
                        value = rep(value, "addressline1", address.AddressLine1);
                        value = rep(value, "addressline2", address.AddressLine2);
                        value = rep(value, "addressline3", address.AddressLine3);
                        value = rep(value, "addressline4", address.AddressLine4);
                        value = rep(value, "city", address.City);
                        value = rep(value, "county", address.County);
                        value = rep(value, "country", address.Country);
                        value = rep(value, "postcode", address.Postcode);
                        if (value.substr(0, 2) == ", ")
                        {
                            value = value.substr(2);
                        }
                        if (value.length > 2 && value.substr(value.length - 2, 2) == ", ")
                        {
                            value = value.substr(0, value.length - 2);
                        }
                        $addressBox.val(value);
                    }
                    break;
            }
        });
    };

    var previousPostCode = $element.val();
    self._lookup(previousPostCode);
};
