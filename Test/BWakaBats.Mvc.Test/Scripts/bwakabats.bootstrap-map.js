var BootstrapMap = function ($element)
{
    var self = this;
    self.$element = $element;

    L.Icon.Default.imagePath = "../../../Content/images";

    var $siblings = $element.siblings("input");
    self.$latitude = $siblings.first();
    self.$longitude = $siblings.eq(1);

    var isSet = self.$latitude.val();
    var latitude = self.$latitude.val() || 54.3372136;
    var longitude = self.$longitude.val() || -3.7699527;
    var zoom = $element.data("map-zoom");
    var minZoom = $element.data("map-minzoom");
    var maxZoom = $element.data("map-maxzoom");
    var image = $element.data("map-image");
    var createLayerFunc = $element.data("map-createlayer");
    var onLoadFunc = $element.data("map-onload");
    var layerOptions;
    var isImage = false;

    if (image != null && image != "" && image != undefined)
    {
        layerOptions = { crs: L.CRS.Simple };
        isImage = true;
    }
    else if (createLayerFunc == null || createLayerFunc == "" || createLayerFunc == undefined)
    {
        var osmUrl = "http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png";
        var osmAttrib = "Map data © <a href='http://openstreetmap.org'>OpenStreetMap</a> contributors";
        var layer = new L.TileLayer(osmUrl, { minZoom: minZoom, maxZoom: maxZoom, attribution: osmAttrib });
        layerOptions = { layer: layer };
    }
    else
    {
        layerOptions = eval(createLayerFunc + "()");
    }

    var options = {
        zoom: minZoom,
        maxZoom: maxZoom,
        minZoom: minZoom,
        center: ([latitude, longitude]),
        layers: layer,
        zoomControl: minZoom != maxZoom
    };
    $.extend(options, layerOptions);
    self.map = L.map(self.$element[0], options);

    if (isImage)
    {
        $("<img/>").attr("src", image).on("load", function ()
        {
            var x = this.width;
            var y = this.height;
            $(this).remove();
            var bounds1 = [[0, 0], [y, x]];
            L.imageOverlay(image, bounds1).addTo(self.map);
            self.map.setMaxBounds(bounds1);
            var bounds2 = [[0, 0], [y * .9, x * .9]];
            self.map.fitBounds(bounds2);
        });
    }

    self._marker = undefined;
    self.panTo = function (latitude, longitude)
    {
        self.$latitude.val(latitude);
        self.$longitude.val(longitude);

        var latLng = L.latLng(latitude, longitude);
        if (self.map.getZoom(zoom) == minZoom)
        {
            self.map.setZoom(zoom);
        }
        self.map.panTo(latLng);

        if (self._marker != undefined)
        {
            self.map.removeLayer(self._marker);
        }
        self._marker = L.marker(latLng).addTo(self.map);
    };

    self.pin = function (latitude, longitude, color, icon)
    {
        var latLng = L.latLng(latitude, longitude);
        var options = { prefix: 'fa' };
        if (color != undefined)
        {
            options.markerColor = color;
        }
        if (icon != undefined)
        {
            options.icon = icon;
        }
        return L.marker(latLng, { icon: L.AwesomeMarkers.icon(options) }).addTo(self.map);
    };

    if (isSet)
    {
        self.panTo(latitude, longitude);
    }
    if (onLoadFunc != null && onLoadFunc != "" && onLoadFunc != undefined)
    {
        eval(onLoadFunc + "(self)");
    }
};
