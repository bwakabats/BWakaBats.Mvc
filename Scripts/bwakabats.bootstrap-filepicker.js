var BootstrapImageCropper = function ()
{
    var self = this;
    self.cropping = false;
    self.Id;
    self.R = 0;
    var _Jcrop;
    self.Width;
    self.Height;
    self.Ratio;
    self.ClientWidth;
    self.ClientHeight;
    self.L = 0;
    self.T = 0;
    self.W = 1;
    self.H = 1;

    if ($("#previewCrop").length == 0)
    {
        dialogHtml =
"<div id='previewCrop' class='preview' style='display:none;'>"
+ "    <img id='imageCrop' alt='Crop' />"
+ "    <div class='row'>"
+ "        <div class='col-xs-8 col-sm-10 col-sm-offset-1'>"
+ "            Select the area of the image to crop to"
+ "            <br />"
+ "            Press ESC to close"
+ "        </div>"
+ "    </div>"
+ "    <button class='btn btn-primary' id='previewClose' type='button'><span class='fa fa-remove fa-fw'></span></button>"
+ "</div>";
        $("body").append(dialogHtml);
    }

    self.initialize = function (id, aspectRatio)
    {
        $(".tooltip").hide();
        self.Id = id;
        self.R = aspectRatio || 0;
        var $image = $("#" + id + "_Image");
        var src = $image.attr("src");
        $("#imageCrop").attr('src', src);
        $("#previewCrop").fadeIn(250);
        self.cropping = true;
        return false;
    };

    self.close = function ()
    {
        $("#previewCrop").fadeOut(250);
        self.cropping = false;
        $("#imageCrop").attr('src', '');
    };

    self.onchange = function (c)
    {
        self.L = c.x / self.ClientWidth;
        self.T = c.y / self.ClientHeight;
        self.W = c.w / self.ClientWidth;
        self.H = c.h / self.ClientHeight;
        $("#" + self.Id + "_Crop").val(self.L + "," + self.T + "," + self.W + "," + self.H);
    };

    self.resize = function (isNew)
    {
        if (!isNew && !self.cropping)
            return;

        if (_Jcrop !== undefined)
        {
            _Jcrop.destroy();
        }

        var windowWidth = $(window).width();
        var windowHeight = $(window).height();

        var $back = $("#previewCrop");
        var image = $("#imageCrop");

        $back.width(windowWidth);
        $back.height(windowHeight);

        if (isNew)
        {
            image.removeAttr("height");
            image.removeAttr("width");
            image.css("width", "auto");
            image.css("height", "auto");
            //self.Width = image[0].width;
            //self.Height = image[0].height;
            //self.Ratio = self.Width / self.Height;
            self.Ratio = image[0].width / image[0].height;
        }

        var cropBorder = windowWidth < 400 ? 0 : (windowWidth > 1000 ? 100 : (windowWidth - 400) / 6);
        self.ClientWidth = windowWidth - cropBorder;
        self.ClientHeight = windowHeight - cropBorder;

        //if (self.ClientWidth > self.Width)
        //{
        //    self.ClientWidth = self.Width;
        //}
        //if (self.ClientHeight > self.Height)
        //{
        //    self.ClientHeight = self.Height;
        //}

        var ratio = self.ClientWidth / self.ClientHeight;
        if (ratio > self.Ratio)
        {
            self.ClientWidth = self.ClientHeight * self.Ratio;
        }
        else
        {
            self.ClientHeight = self.ClientWidth / self.Ratio;
        }
        var left = (windowWidth - self.ClientWidth) / 2;
        var top = (windowHeight - self.ClientHeight) / 2 + cropBorder / 3;

        image.css("left", left + "px");
        image.css("top", top + "px");
        image.width(self.ClientWidth);
        image.height(self.ClientHeight);

        _Jcrop = $.Jcrop("#imageCrop");
        var holder = _Jcrop.ui.holder;
        holder.css("left", left + "px");
        holder.css("top", top + "px");
        holder.css("position", "absolute");
        holder.width(self.ClientWidth);
        holder.height(self.ClientHeight);

        _Jcrop.setOptions(
        {
            onChange: self.onchange,
            bgColor: 'white',
            bgOpacity: .25,
            setSelect: [self.L * self.ClientWidth, self.T * self.ClientHeight, (self.L + self.W) * self.ClientWidth, (self.T + self.H) * self.ClientHeight],
            allowSelect: false,
            aspectRatio: self.R
        });
    };

    var $back = $("#previewCrop");
    $back.find("#previewClose").click(self.close);

    $("#imageCrop").on("load", function ()
    {
        self.resize(true);
    });

    $(document).keyup(function (e)
    {
        if (!self.cropping)
            return;

        if (e.keyCode == 27)
        {
            self.close();
        }

        e.preventDefault();
        return false;
    });

    $(window).resize(function ()
    {
        self.resize(false);
    });
};
var imageCropper = new BootstrapImageCropper();

var BootstrapFilePicker = function ()
{
    var self = this;
    self.initialize = function ($fileButton, $moreButton)
    {
        var id = $fileButton.attr("data-file-for");
        var url = $fileButton.attr("data-file-url");
        var fileType = $fileButton.attr("data-file-type");
        var buttonId = $moreButton === undefined ? $fileButton.attr("id") : $moreButton.attr("id");
        var progress = $("#" + id + "_Progress");
        var progressContainer = $("#" + id + "_ProgressContainer");
        var progressPercent = $("#" + id + "_ProgressPercent");
        progressContainer.hide();

        var uploader = new plupload.Uploader(
        {
            browse_button: buttonId,
            url: url,
            multi_selection: false,
            chunk_size: "200kb",
            runtimes: "html5,flash,silverlight,html4",
            flash_swf_url: "http://rawgithub.com/moxiecode/moxie/master/bin/flash/Moxie.cdn.swf",
            silverlight_xap_url: "http://rawgithub.com/moxiecode/moxie/master/bin/silverlight/Moxie.cdn.xap",
            init: {
                FilesAdded: function ()
                {
                    progress.attr("width", "0");
                    progressPercent.html("0%");
                    progressContainer.show();
                    uploader.settings.url = url;
                    uploader.start();
                },
                UploadProgress: function (up, file)
                {
                    var width = file.percent + "%";
                    progress.attr("aria-valuenow", file.percent);
                    progress.css("width", width);
                    progressPercent.html(width);
                },
                FileUploaded: function (up, file, info)
                {
                    var data = JSON.parse(info.response);
                    utilities.ajaxCheck(data, "Upload", null, null, function ()
                    {
                        $("#" + id).val(data.fileId);
                        var $image = $("#" + id + "_Image");
                        $image.attr("src", data.imageFileName);
                        if (data.width > 200)
                        {
                            $image.width(200);
                        }
                        else
                        {
                            $image.width(data.width);
                        }
                        self.uploaded(id, data.fileName, data.imageFileName, data.fileContentType);

                        progressPercent.html("Done");
                        setTimeout(function ()
                        {
                            progressContainer.hide();
                        }, 3000);
                    }, function (data, title, textStatus, jqXHR, error)
                    {
                        utilities.failed(data, title, textStatus, jqXHR, error);
                        $("#" + id).val("");
                        $fileButton.closest(".filepicker-buttons").removeClass("selected");
                        $("#" + id).trigger("change");
                    });
                },
                Error: function (up, err)
                {
                    progressContainer.hide();
                    utilities.message("Uploader Error", err.message + "<br/><br/>Error Code: " + err.code, "medium");
                    $("#" + id).val("");
                    $fileButton.closest(".filepicker-buttons").removeClass("selected");
                    $("#" + id).trigger("change");
                }
            }
        });

        $("#" + id).data("uploader", uploader);
        if (fileType == "image")
        {
            uploader.setOption("filters", [{ title: "Pictures", extensions: "jpg,jpeg,gif,png,tif,tiff,exif" }]);
        }
        else if (fileType == "audio")
        {
            uploader.setOption("filters", [{ title: "Audio", extensions: "mp3,m4a" }]);
        }

        uploader.init();
    };

    self.uploaded = function (id, fileName, imageFileName, contentType)
    {
        var extension;
        var name = fileName;
        var lastDot = name.lastIndexOf(".");
        if (lastDot > -1)
        {
            extension = name.substr(lastDot + 1).toLowerCase();
            name = name.substr(0, lastDot);
        }
        else
        {
            extension = "";
        }
        name = name.substr(name.lastIndexOf("/") + 1);
        $("[data-file-filename='" + id + "']").val(fileName);
        $("[data-file-name='" + id + "']").val(name);
        $("[data-file-extension='" + id + "']").val(extension);
        $("[data-file-path='" + id + "']").val(imageFileName);
        $("[data-file-contenttype='" + id + "']").val(contentType);
        $("#" + id + "_FileName").html(name);
        $("[data-container-for='" + id + "_Button']").closest(".filepicker-buttons").addClass("selected");
        $("#" + id).trigger("change");
    };
};
var filePicker = new BootstrapFilePicker();
