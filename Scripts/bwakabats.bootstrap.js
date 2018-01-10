var BootstrapUtilities = function ()
{
    var self = this;
    var _canCapture;

    self.fixForm = function ($container, canBeDirty)
    {
        $(".tooltip").hide();

        $container.find("form").each(function ()
        {
            var $form = $(this)
                .removeData("validator") /* added by the raw jquery.validate plugin */
                .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin */

            $.validator.unobtrusive.parse($form);

            var validator = $.data($form[0], "validator");
            if (validator != undefined)
            {
                var settings = validator.settings;
                settings.ignore = [];
                settings.highlight = function (element)
                {
                    var $element = $(element);
                    var parent = $element.parent();
                    parent.removeClass("has-success").addClass("has-error");
                    parent.find(".form-control-feedback").removeClass("fa-check").addClass("fa-times");
                    var $tabPane = $element.parents(".tab-pane");
                    var tabId = $tabPane.attr("id");
                    if (tabId != undefined)
                    {
                        $form.find("a[href='#" + tabId + "'][data-toggle]").tab("show");
                    }
                };

                settings.unhighlight = function (element)
                {
                    var $element = $(element);
                    var parent = $element.parent();
                    parent.removeClass("has-error").addClass("has-success");
                    parent.find(".form-control-feedback").removeClass("fa-times").addClass("fa-check");
                };

                $form.find(".field-validation-error").each(function ()
                {
                    var $this = $(this);
                    var parent = $this.parent();
                    parent.addClass("has-error");
                    parent.find(".form-control-feedback").addClass("fa-times");
                });
            }

            $form.submit(function ()
            {
                var $this = $(this);
                $this.find(".text-upper").each(function ()
                {
                    var $this = $(this);
                    $this.val($this.val().toUpperCase());
                });
                $this.find(".text-lower").each(function ()
                {
                    var $this = $(this);
                    $this.val($this.val().toLowerCase());
                });
                $this.find(".text-mixed").each(function ()
                {
                    var $this = $(this);
                    $this.val($this.val().replace(/^(.)|(\s|\-)(.)/g, function (c) { return c.toUpperCase(); }));
                });
                window.isDirty = false;
            });
        });

        var $inputs;

        if ($container.datetimepicker != undefined)
        {
            var awesomeIcons = {
                time: "fa fa-clock-o fa-fw",
                date: "fa fa-calendar fa-fw",
                up: "fa fa-chevron-up fa-fw",
                down: "fa fa-chevron-down fa-fw",
                previous: "fa fa-chevron-left fa-fw",
                next: "fa fa-chevron-right fa-fw",
                today: "fa fa-crosshairs fa-fw",
                clear: "fa fa-trash-o fa-fw",
                close: "fa fa-remove fa-fw"
            };

            $inputs = $container.find("input[data-type]");
            $inputs.filter("[data-type=datetime]")
                .datetimepicker({
                    format: "DD/MM/YYYY HH:mm",
                    stepping: 1,
                    icons: awesomeIcons,
                    useCurrent: false,
                    showClose: true
                    //defaultSelect: false,
                    //allowBlank: true,
                    //closeOnDateSelect: true,
                });

            $inputs.filter("[data-type=time]")
                .datetimepicker({
                    format: "HH:mm",
                    stepping: 1,
                    icons: awesomeIcons,
                    useCurrent: false,
                    showClose: true
                    //defaultSelect: false,
                    //allowBlank: true,
                    //closeOnDateSelect: true,
                });

            $inputs.filter("[data-type=date]")
                .datetimepicker({
                    format: "DD/MM/YYYY",
                    icons: awesomeIcons,
                    useCurrent: false,
                    showClose: true
                    //defaultSelect: false,
                    //allowBlank: true,
                    //closeOnDateSelect: true,
                });
        }

        if ($container.htmlbox != undefined)
        {
            $container.find("textarea[data-html]").each(function ()
            {
                var $this = $(this);
                var $parent = $this.parent();

                var flags = $this.data("html") + ", ";
                flags = flags.replace("All", "Common, Font, StylePlus, Code");
                flags = flags.replace("Common", "Simple, CopyPaste, Undo, Indents, Paragraph, Links, Format");
                flags = flags.replace("Simple", "Style, Justification");

                var toolbars = [];
                var toolbar = null;
                if (flags.indexOf("Style, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "bold", "italic", "underline");
                }
                if (flags.indexOf("StylePlus, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "strike", "sub", "sup");
                }
                if (flags.indexOf("Justification, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "left", "center", "right", "justify");
                }
                if (flags.indexOf("CopyPaste, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "cut", "copy", "paste");
                }
                toolbar = null;

                if (flags.indexOf("Undo, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "undo", "redo");
                }
                if (flags.indexOf("Indents, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "ol", "ul", "indent", "outdent");
                }
                if (flags.indexOf("Paragraph, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "hr", "br", "paragraph");
                }
                toolbar = null;

                if (flags.indexOf("Font, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "fontsize", "fontfamily");
                }
                if (flags.indexOf("Format, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "formats");
                }
                toolbar = null;

                if (flags.indexOf("Links, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "link", "unlink");
                }
                if (flags.indexOf("Code, ") > -1)
                {
                    if (toolbar == null)
                    {
                        toolbar = [];
                        toolbars.push(toolbar);
                    }
                    toolbar.push("separator", "code", "removeformat", "striptags");
                }
                toolbar = null;

                var htmlbox = $this.htmlbox({
                    toolbars: toolbars,
                    about: false,
                    idir: urlRoot + "Images/htmlbox/",
                    css: "body{font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;}",
                    skin: "silver",
                    icons: "silk",
                    change: function ()
                    {
                        window.isDirty = true;
                        if (typeof $this[0].onchange === "function")
                        {
                            $this[0].onchange();
                        }
                    }
                });

                $this.data("htmlbox", htmlbox);

                var $table = $parent.children("table");
                var $frame = $table.find("iframe");
                $(window).resize(function ()
                {
                    setTimeout(function ()
                    {
                        var oldWidth = $frame.width();
                        var newWidth = $parent.width();
                        if (oldWidth != newWidth - 1)
                        {
                            $frame.width(newWidth);
                            $table.width(newWidth);
                        }
                    }, 0);
                });
            });
        }

        if ($container.typeahead != undefined)
        {
            $inputs = $container.find("input[data-typeahead]");
            $inputs.each(function ()
            {
                var $this = $(this);
                var delimiter = $this.data("typeahead-delimiter");
                var source = $this.data("typeahead").split(delimiter);
                $this.typeahead(
                    {
                        minLength: 1,
                        highlight: true
                    },
                    {
                        name: $this.attr("name"),
                        limit: 10,
                        source: function (query, cd)
                        {
                            var q = query.toLowerCase();
                            var search = source.filter(function (s) { return s.toLowerCase().indexOf(q) !== -1; });
                            cd(search);
                        }
                    }
                );
            });
            $container.typeahead = true;
        }

        $container.find("[data-colorpicker]").each(function ()
        {
            if (typeof BootstrapColorPicker === "undefined")
            {
                utilities.message("Javascript Missing", "The javascript file for the BWakaBats color picker is missing");
                return false;
            }

            var $this = $(this);
            var id = $this.attr("id");
            var $button = $container.find("#" + id + "_button");
            var $popup = $container.find("#" + id + "_popup");
            var $name = $container.find("#" + id + "_ColorName");
            $this.data("colorpicker", new BootstrapColorPicker($this, $button, $popup, $name));
        });

        $container.find("[data-address-part='postcode']").each(function ()
        {
            if (typeof BootstrapAddressLookup === "undefined")
            {
                utilities.message("Javascript Missing", "The javascript file for the BWakaBats postcode box is missing");
                return false;
            }
            var $this = $(this);
            var $button = $this.parent().find("button");
            var $addressBoxes = $container.find("[data-address-part]");
            $this.data("addresslookup", new BootstrapAddressLookup($this, $button, $addressBoxes));
        });

        var input = document.createElement("input");
        input.type = "file";
        var isUploadDisabled = input.disabled;

        $container.find("[data-file-for]").each(function ()
        {
            if (isUploadDisabled)
            {
                utilities.message("File Upload Error", "Sorry, your device does not support file uploads", "medium");
                return false;
            }
            if (typeof filePicker === "undefined")
            {
                utilities.message("Javascript Missing", "The javascript file for the BWakaBats file picker is missing");
                return false;
            }

            filePicker.initialize($(this));
        });

        $container.find("[data-file-rotate-for]").each(function ()
        {
            var $this = $(this);
            var id = $this.attr("data-file-rotate-for");
            $this.click(function ()
            {
                var $id = $("#" + id);
                var guid = $id.val();
                if (guid == "")
                    return;

                utilities.ajax("Rotate Image", $this.attr("data-file-url").replace("_ID_", guid), function (data)
                {
                    $id.val(data.fileId);
                    $("[data-file-path='" + id + "']").val(data.imageFileName);

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
                    $("#" + id).trigger("change");
                });
            });
        });

        $container.find("[data-file-crop-for]").each(function ()
        {
            var $this = $(this);
            var id = $this.attr("data-file-crop-for");
            $this.click(function ()
            {
                var $id = $("#" + id);
                var guid = $id.val();
                if (guid == "")
                    return;

                imageCropper.initialize(id);
            });
        });

        var $captureFor = $container.find("[data-file-capture-for]");
        if (_canCapture == false)
        {
            $this.hide();
        }
        else
        {
            $captureFor.each(function ()
            {
                var $this = $(this);
                var id = $this.attr("data-file-capture-for");
                $this.click(function ()
                {
                    if (_canCapture == undefined)
                    {
                        var getUserMedia = Modernizr.prefixed("getUserMedia", navigator);
                        if (getUserMedia)
                        {
                            getUserMedia({ video: true }, function (stream)
                            {
                                _canCapture = true;
                                var videoControl = document.createElement("video");
                                videoControl.src = window.URL.createObjectURL(stream);
                                self.videoControl = videoControl
                                self._capture(id);
                            }, function ()
                                {
                                    $captureFor.hide();
                                    self.message("Capture Failed", "Sorry, you cannot capture at this time.");
                                    _canCapture = false;
                                });
                        }
                        else
                        {
                            $captureFor.hide();
                            self.message("Capture Failed", "Sorry, you cannot capture at this time.");
                            _canCapture = false;
                        }
                    }
                    else if (_canCapture)
                    {
                        self._capture(id);
                    }
                });
            });
        }

        $container.find("[data-file-cancel-for]").each(function ()
        {
            var $this = $(this);
            var id = $this.attr("data-file-cancel-for");
            $this.click(function ()
            {
                var $id = $("#" + id);
                var guid = $id.val();
                if (guid == "")
                    return;

                $id.val("");
                var $image = $("#" + id + "_Image");
                $image.attr("src", urlRoot + "Images/Defaults/" + ($this.attr("data-file-type") == "image" ? "Image" : "Document") + ".png");
                $image.width(200);
                $("[data-file-path='" + id + "']").val("");
                $("[data-file-name='" + id + "']").val("");
                $("[data-file-extension='" + id + "']").val("");
                $("#" + id + "_FileName").html("");
                $this.closest(".filepicker-buttons").removeClass("selected");
                $("#" + id).trigger("change");
            });
        });

        $container.find("[data-grid-pagesize]").each(function ()
        {
            if (typeof BootstrapGrid === "undefined")
            {
                utilities.message("Javascript Missing", "The javascript file for the BWakaBats grid is missing");
                return false;
            }
            var $this = $(this);
            $this.data("grid", new BootstrapGrid($this));
        });

        $container.find("[data-map-zoom]").each(function ()
        {
            if (typeof BootstrapMap === "undefined")
            {
                utilities.message("Javascript Missing", "The javascript file for the BWakaBats location picker is missing");
                return false;
            }
            var $this = $(this);
            $this.data("map", new BootstrapMap($this));
        });

        //$container.find(".tab-content > .tab-pane.active:gt(0)").removeClass("active");

        if (canBeDirty != false)
        {
            $container.find("form input, form select, form textarea").change(function ()
            {
                window.isDirty = true;
            });
        }

        self.applyTooltips($container);
        self.interruptClick($container);
    };

    self._capture = function (id)
    {
        var $id = $("#" + id);
        var uploader = $id.data("uploader");
        var videoControl = self.videoControl;
        self.dialog({
            id: "CaptureDialog",
            title: "Capture Image",
            size: "large",
            message: self.videoControl,
            buttons: {
                Capture: {
                    stayOpen: true,
                    func: function ()
                    {
                        var videoControl = self.videoControl;
                        var $videoControl = $(videoControl);
                        var canvas = document.createElement("canvas");
                        var context = canvas.getContext("2d");
                        var height = $videoControl.height();
                        var maxWidth = $videoControl.css("max-height").replace(/[^-\d\.]/g, '')
                        var width = height < maxWidth ? $videoControl.width() : (maxWidth / .75);
                        canvas.width = width;
                        canvas.height = height;
                        context.drawImage(videoControl, 0, 0, width, height);
                        var data = canvas.toDataURL("image/png");
                        uploader.addFile(new o.File(null, {
                            name: "capture.png",
                            data: data
                        }));
                        self.dialogClose("#CaptureDialog")
                    }
                },
                Cancel: null
            }
        });
        videoControl.play();
    }

    self.applyTooltips = function ($container)
    {
        $container.find("[data-customtooltip]").each(function ()
        {
            var element = $(this);
            var customId = element.data("customtooltip");
            element.attr("title", $("#" + customId).html());
            element.tooltip({ html: true });
        });

        $container.find("[title]").tooltip({ container: "body" });
    };

    self._tab = null;

    self.interruptClick = function ($container)
    {
        $container.find("[data-interrupt]").each(function ()
        {
            var $this = $(this);
            var href = $this.attr("href") || $this.data("href") || $this.attr("onclick");
            $this.removeAttr("href");
            $this.removeAttr("onclick");
            var title = $this.data("interrupt");
            if (title == true || title == "true" || title == "True")
            {
                title = $this.attr("title") || $this.attr("data-original-title") || "Delete";
            }
            $this.off("click").click(function (event)
            {
                var $tab = $this.closest(".tab-content");
                self._tab = $tab.parent().find("ul > li.active > a").attr("href");
                if (self._tab != undefined && self._tab != null)
                {
                    self._tab = self._tab.substring(1);
                }

                self.interrupt(title, href);
                event.preventDefault();
                return false;
            });
        });
    };

    self.ajax = function (title, url, successFunction, failFunction, model, headers)
    {
        $.ajax({
            async: true,
            cache: false,
            url: url,
            data: model,
            type: (model == undefined) ? "GET" : "POST",
            headers: headers
        })
            .done(function (data, textStatus, jqXHR) { self.ajaxCheck(data, title, textStatus, jqXHR, successFunction, failFunction); })
            .fail(function (jqXHR, textStatus, errorThrown, data) { self._ajaxFail(data, title, textStatus, jqXHR, successFunction, failFunction, errorThrown); });
    };

    var notify_stack = {
        dir1: "up",
        dir2: "left",
        push: "top",
        spacing1: 15,
        spacing2: 15,
        context: $("body")
    };

    self.notify = function (title, message, type)
    {
        if (PNotify == undefined)
        {
            self.message(title, message);
        }
        PNotify.desktop.permission();
        return new PNotify({
            title: title,
            text: message,
            type: type || "info",
            addclass: "stack-bottomright",
            stack: notify_stack,
            icon: false,
            //hide: false,
            styling: "fontawesome",
            buttons: {
                sticker: false
            },
            desktop: {
                desktop: true
            }
        });
    };

    self.message = function (title, message, size, okFunction)
    {
        self.dialog(
        {
            title: title,
            message: message,
            size: size,
            buttons:
            {
                "Close": okFunction
            }
        });
    };

    self.confirm = function (title, message, yesFunction, noFunction, question, size)
    {
        self.dialog(
        {
            title: title,
            message: message,
            extra: question || "Are you sure?",
            size: size,
            buttons:
            {
                "Yes": yesFunction,
                "No": noFunction
            }
        });
    };

    self.confirmOrCancel = function (title, message, yesFunction, noFunction, cancelFunction)
    {
        self.dialog(
        {
            title: title,
            message: message,
            buttons:
            {
                "Yes": yesFunction,
                "No": noFunction,
                "Cancel": cancelFunction
            }
        });
    };

    self.input = function (title, message, okFunction, cancelFunction)
    {
        self.dialog(
        {
            title: title,
            message: message + "<div><input id='dialogInput' type='text' class='form-control'/></div>",
            buttons:
            {
                "OK": function ()
                {
                    var answer = $("#dialog #dialogInput").val();
                    okFunction(answer);
                },
                "Cancel": cancelFunction
            }
        });
    };

    self.popup = function (title, url, size, buttons, loadedFuncton)
    {
        self.dialog({
            title: title,
            message: "Loading... <span class='fa fa-refresh fa-spin'></span>",
            size: size || "medium",
            buttons: buttons || { "Close": null }
        });
        self.ajax(
            title,
            url,
            function (data)
            {
                $message = $("#dialogMessage");
                $message.html(data);
                self.fixForm($message);
                if (loadedFuncton != undefined)
                {
                    loadedFuncton(data);
                }
            }
        );
    };

    var _pleaseWaiting = true;

    self.pleaseWait = function (title, message, size)
    {
        _pleaseWaiting = true;
        setTimeout(function ()
        {
            if (_pleaseWaiting)
            {
                self.dialog({
                    title: title,
                    message: (message || "Please wait") + "... <span class='fa fa-refresh fa-spin'></span>",
                    size: size || "small"
                });
            }
        }, 500);
    };

    self.dataHasErrors = function (data)
    {
        return (data != undefined && typeof data.error !== "undefined");
    };

    self.success = function (data, title)
    {
        self.notify(title, title + " Successful");
    };

    self.failed = function (data, title, textStatus, jqXHR, errorThrown)
    {
        self.message(title + " Failed", errorThrown.message || errorThrown, "medium");
    };

    self.dialog = function (options)
    {
        var id = options.id || "dialog";
        var title = options.title;
        var message = options.message;
        var extra = options.extra;
        var size = options.size || "small";
        var buttons = options.buttons;
        var fixForm = options.fixForm;
        //var isStatic = options.isStatic;

        _pleaseWaiting = false;

        var selector = "#" + id;
        var $selector = $(selector);
        if ($selector.length == 0)
        {
            var dialogHtml =
  "<div class='modal fade' id='" + id + "' tabindex='-1' role='dialog' aria-labelledby='dialogTitle' aria-hidden='true'>"
+ "  <div class='modal-dialog'>"
+ "    <div class='modal-content'>"
+ "      <div class='modal-header'>"
+ "        <button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>"
+ "        <h3 class='modal-title' id='" + id + "Title'></h3>"
+ "      </div>"
+ "      <div class='modal-body'>"
+ "        <p id='" + id + "Message'></p>"
+ "        <p id='" + id + "Extra'></p>"
+ "      </div>"
+ "      <div class='modal-footer'>"
+ "        <button class='btn btn-primary' id='" + id + "Button5' type='button'></button>"
+ "        <button class='btn btn-primary' id='" + id + "Button4' type='button'></button>"
+ "        <button class='btn btn-primary' id='" + id + "Button3' type='button'></button>"
+ "        <button class='btn btn-success' id='" + id + "Button2' type='button'></button>"
+ "        <button class='btn btn-danger' id='" + id + "Button1' type='button'></button>"
+ "      </div>"
+ "    </div>"
+ "  </div>"
+ "</div>";
            $("body").append(dialogHtml);
            var $selector = $(selector);
        }
        else if ($selector.hasClass("in"))
        {
            if ($selector.hasClass("fade"))
            {
                $selector.removeClass("fade").modal("hide").addClass("fade");
            }
            else
            {
                $selector.modal("hide");
            }
        }

        $(selector + "Title").empty().append(title);
        $(selector + "Message").empty().append(message);
        if (extra == undefined)
        {
            $(selector + "Extra").hide();
        }
        else
        {
            $(selector + "Extra").html(extra).show();
        }
        switch (size)
        {
            case "large":
                $(selector + " .modal-dialog").addClass("modal-lg").removeClass("modal-sm modal-md");
                break;
            case "medium":
                $(selector + " .modal-dialog").addClass("modal-md").removeClass("modal-sm modal-lg");
                break;
            case "small":
                $(selector + " .modal-dialog").addClass("modal-sm").removeClass("modal-md modal-lg");
                break;
        }

        var buttonCount = 0;
        for (var b in buttons)
        {
            buttonCount++;
        }
        for (var index = buttonCount + 1; index <= 5; index++)
        {
            $(selector + "Button" + index).hide();
        }
        for (var button in buttons)
        {
            var $button = $(selector + "Button" + buttonCount).show().off();
            $button.html(button);
            (function (func)
            {
                if (func == null)
                {
                    $button.click(function ()
                    {
                        $selector.off();
                        self.dialogClose(selector);
                    });
                    $selector.off();
                }
                else if (func.stayOpen)
                {
                    func = func.func;
                    if (typeof (func) == "function")
                    {
                        $button.click(function ()
                        {
                            return func();
                        });
                    }
                    else
                    {
                        alert("func.func not a function");
                    }
                }
                else if (typeof (func) == "function")
                {
                    $button.click(function ()
                    {
                        $selector.off().on("hidden.bs.modal", function ()
                        {
                            $selector.off();
                            func();
                        });
                        self.dialogClose(selector);
                    });
                    // Last button is close action
                    $selector.off().on("hidden.bs.modal", function ()
                    {
                        func();
                    });
                }
                else // if (typeof (func) == "string")
                {
                    $button.click(function ()
                    {
                        $selector.off().on("hidden.bs.modal", function ()
                        {
                            $selector.off();
                            location.href = func;
                        });
                        self.dialogClose(selector);
                    });
                    // Last button is close action
                    $selector.off().on("hidden.bs.modal", function ()
                    {
                        location.href = func;
                    });
                }
            })(buttons[button]);
            buttonCount--;
            if (buttonCount == 0)
                break;
        }

        //if (isStatic)
        //{
        //    $selector.modal({ backdrop: "static" });
        //}
        //else
        //{
        //    $selector.modal({ backdrop: true });
        //}
        //var bsModal = $selector.data("bs.modal");
        //if (!(bsModal && bsModal.isShown))
        //{
        //    $selector.modal({ backdrop: "static" });
        //}
        if (fixForm)
        {
            $selector.on("shown.bs.modal", function ()
            {
                utilities.fixForm($("#" + id + "Message"), false);
            });
        }
        $selector.modal({ backdrop: "static" });
    };

    self.dialogClose = function (selector)
    {
        selector = selector || "#dialog";
        _pleaseWaiting = false;
        $(selector).modal("hide");
    };

    self.dialogReopen = function (selector)
    {
        selector = selector || "#dialog";
        $(selector).modal("show");
    };

    self.ajaxCheck = function (data, title, textStatus, jqXHR, successFunction, failFunction)
    {
        if (data == "")
        {
            var responsed = jqXHR.getResponseHeader("X-Responded-JSON");
            if (responsed != null)
            {
                var realData = JSON.parse(responsed);
                if (realData.status != null && realData.status > 299)
                {
                    if (realData.status == 401)
                    {
                        data = { error: "User Authentication Failed" };
                    }
                    else
                    {
                        data = { error: "Error status code " + realData.status };
                    }
                }
            }
        }
        if (self.dataHasErrors(data))
        {
            if (failFunction === undefined)
            {
                self.failed(data, title, textStatus, jqXHR, data.error);
            }
            else if (failFunction !== null)
            {
                failFunction(data, title, textStatus, jqXHR, data.error);
            }
        }
        else if (successFunction === undefined)
        {
            self.success(data, title, textStatus, jqXHR);
        }
        else if (successFunction !== null)
        {
            successFunction(data, title, textStatus, jqXHR);
        }
    };

    self._ajaxFail = function (data, title, textStatus, jqXHR, successFunction, failFunction, errorThrown)
    {
        $(".tooltip").hide();
        if (errorThrown != "")
        {
            if (failFunction === undefined)
            {
                self.failed(data, title, textStatus, jqXHR, errorThrown);
            }
            else if (failFunction !== null)
            {
                failFunction(data, title, textStatus, jqXHR, errorThrown);
            }
        }
    };

    self.interrupt = function (title, href, message)
    {
        self.confirm(title, message || "You are about to " + title.toLowerCase() + ".", function ()
        {
            if (typeof href === "function")
            {
                href();
            }
            else if (href.substring(0, 11) == "javascript:")
            {
                eval(href);
            }
            else
            {
                self.interrupted(title, href);
            }
        });
    };

    self.interrupted = function (title, href)
    {
        self.pleaseWait(title);
        self.ajax(title, href, function (data)
        {
            self.dialogClose();
            if (typeof data.confirmationMessage !== "undefined")
            {
                if (href.indexOf("?") > -1)
                {
                    href += "&confirmed=true";
                }
                else
                {
                    href += "?confirmed=true";
                }
                self.interrupt(title, href, data.confirmationMessage);
            }
            else if (data.href != null)
            {
                location.href = data.href;
            }
            else if (self._tab == undefined || self._tab == null)
            {
                history.go(0);
            }
            else if (location.href.indexOf("?") > -1)
            {
                location.href = location.href.replace("&tab=", "&tabold=").replace("?tab=", "?tabold=") + "&tab=" + self._tab;
            }
            else
            {
                location.href = location.href + "?tab=" + self._tab;
            }
        }, function (data, title, textStatus, jqXHR, errorThrown)
        {
            self.dialogClose();
            self.failed(data, title, textStatus, jqXHR, errorThrown);
        });
    };

    self.navbarCollapse = function () { };

    var navbarAutocollapseDelay;
    var navbarAutocollapseRepeat = 0;
    var navbarAutocollapseLongDelay;

    $(document).ready(function ()
    {
        setTimeout(function ()
        {
            utilities.fixForm($(document));
        }, 0);

        var collapsibles = ".navbar .autocollapse, .navbar .autocollapseImportant, .nav-tabs .autocollapse, .nav-tabs .autocollapseImportant, .autocollapsebar .autocollapse, .autocollapsebar .autocollapseImportant";
        var $collapsibles = $(collapsibles);
        if ($collapsibles.length > 0)
        {
            self.navbarCollapse = function ()
            {
                $(collapsibles).show();

                var $navbars = $(".nav-tabs:has(.autocollapse,.autocollapseImportant), .autocollapsebar");
                $navbars.css("white-space", "nowrap").css("overflow", "hidden");
                $navbars.children().filter("li").css("display", "inline-block").css("float", "none");

                $navbars.removeClass("squeeze");
                $navbars.each(function ()
                {
                    var $navbar = $(this);
                    var navbarReal = $navbar[0]
                    if ($navbar.outerWidth() < navbarReal.scrollWidth)
                    {
                        $navbar.addClass("squeeze");
                    }
                    var $collapsibles = $navbar.find(".autocollapse").reverse();
                    $collapsibles.each(function ()
                    {
                        if ($navbar.outerWidth() < navbarReal.scrollWidth)
                        {
                            $(this).hide();
                        }
                        else
                        {
                            return false;
                        }
                    });
                    $collapsibles = $navbar.find(".autocollapseImportant").reverse();
                    $collapsibles.each(function ()
                    {
                        if ($navbar.outerWidth() < navbarReal.scrollWidth)
                        {
                            $(this).hide();
                        }
                        else
                        {
                            return false;
                        }
                    });
                });

                if (!$(".navbar-toggle").is(":visible"))
                {
                    $navbars = $(".navbar");
                    $navbars.removeClass("squeeze");
                    $navbars.each(function ()
                    {
                        var $navbar = $(this);
                        if ($navbar.height() > 75)
                        {
                            $navbar.addClass("squeeze");
                        }
                        var $collapsibles = $navbar.find(".autocollapse").reverse();
                        $collapsibles.each(function ()
                        {
                            if ($navbar.height() > 75)
                            {
                                $(this).hide();
                            }
                            else
                            {
                                return false;
                            }
                        });
                        $collapsibles = $navbar.find(".autocollapseImportant").reverse();
                        $collapsibles.each(function ()
                        {
                            if ($navbar.height() > 75)
                            {
                                $(this).hide();
                            }
                            else
                            {
                                return false;
                            }
                        });
                    });
                }
            };

            var navbarAutocollapse = function (delay, repeat, longDelay)
            {
                self.navbarCollapse();
                if (delay != undefined)
                {
                    navbarAutocollapseDelay = delay;
                    navbarAutocollapseRepeat = repeat;
                    navbarAutocollapseLongDelay = longDelay;
                }
                navbarAutocollapseRepeat--;
                if (navbarAutocollapseRepeat <= 0)
                {
                    navbarAutocollapseDelay = navbarAutocollapseLongDelay;
                    navbarAutocollapseRepeat = 100000;
                }
                setTimeout(navbarAutocollapse, navbarAutocollapseDelay);
            };

            $(window).resize(self.navbarCollapse);
            navbarAutocollapse(50, 10, 2000);
        }

        if (navigator.userAgent.match(/IEMobile\/10\.0/))
        {
            var msViewportStyle = document.createElement("style");
            msViewportStyle.appendChild(document.createTextNode("@-ms-viewport{width:auto!important}"));
            document.querySelector("head").appendChild(msViewportStyle);
        }

        $(window).bind("beforeunload", function ()
        {
            if (window.isDirty && window.ignoreDirty != true)
                return "You have not saved your changes.";
        });
    });
};
var utilities = new BootstrapUtilities();

jQuery.fn.reverse = [].reverse;
