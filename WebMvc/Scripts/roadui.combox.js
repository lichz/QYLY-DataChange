//Combox
; RoadUI.Combox = function ()
{
    var instance = this;
    this.init = function ($comboxs)
    {
        $comboxs.each(function ()
        {
            var id = $(this).attr('id');
            var name = $(this).attr('name');
            var validate = $(this).attr('validate');
            var title = $(this).attr("title") || "";
            var readonly = true;// $(this).attr("readonly") && "readonly" == $(this).attr("readonly").toLowerCase();
            var disabled = $(this).prop("disabled");
            var width1 = $(this).attr("width1");
            var height1 = $(this).attr("height1");
            var datatype = $(this).attr("datatype");
            var change = $(this).attr("onchange");
            if (RoadUI.Core.isIe6Or7())
            {
                $(this).css({ "display": "inline-block"});
            }
            var $hide = $('<input type="text" style="display:none;" id="' + id + '" name="' + name + '" ' + (change ? change.indexOf('"') >= 0 ? 'onchange=\'' + change + '\'' : 'onchange="' + change + '"' : '') + '/>');
            var $input = $('<input type="text" autocomplete="off" class="comboxtext1" style="' + $(this).attr("style") + '" id="' + id + '_text" name="' + name + '_text" value="" />');
            var $div = $('<div id="' + id + '_selectdiv" style="left:' +
                (RoadUI.Core.isIe6Or7() ? $(this).get(0).offsetLeft + 5 : $(this).get(0).offsetLeft + 6) + 'px;width:' +
                (width1 ? width1 + "px" : "auto") + ';height:' + height1 + 'px;white-space:nowrap;' +
                (RoadUI.Core.isIe6Or7() ? 'top:' + ($(this).get(0).offsetTop + 37) + 'px' : '')
                + '" class="comboxdiv"></div>');
            if (readonly)
            {
                $input.prop("readonly", true);
            }
            initElement($input, "comboxtext");
            $(this).after($hide).after($input).after($div).remove();
            $div.hide();
            var tagName = $(this).get(0).tagName.toLowerCase();
            switch (tagName)
            {
                case "select":
                    var multiple = $(this).prop("multiple");
                    var $options = $(this).children("option");
                    var texts = [];
                    var values = [];
                    for (var i = 0; i < $options.size() ; i++)
                    {
                        if ($options.eq(i).attr("selected") && $options.eq(i).prop("selected"))
                        {
                            texts.push($options.eq(i).text());
                            values.push($options.eq(i).val());
                        }
                        var optionValue = $options.eq(i).val();
                        var $divoptions = $('<div class="' + ($options.eq(i).prop("selected") ? "comboxoption1" : "comboxoption") + '" ' +
                            'value="' + optionValue + '"></div>');
                        if (multiple)
                        {
                            var $checkbox = $('<input type="checkbox" id="checkbox_' + id + "_" + optionValue + '" name="radio_' + id + '" value="' + optionValue + '" '
                                + ($options.eq(i).prop("selected") ? 'checked="checked"' : '')
                                + ' style="vertical-align:middle;"/>');
                            $divoptions.append($checkbox);
                            $checkbox.bind("click", function ()
                            {
                                instance.setValue($(this).val(), $(this).next().text(), id, true);
                                $(this).parent().children("div").removeClass().addClass("comboxoption");
                                $(this).removeClass().addClass("comboxoption1");
                            });
                        }
                        else
                        {
                            $divoptions.bind("click", function ()
                            {
                                instance.setValue($(this).attr("value"), $(this).text(), id, false);
                                $(this).parent().children("div").removeClass().addClass("comboxoption");
                                $(this).removeClass().addClass("comboxoption1");
                            });
                        }
                        
                        $divoptions.hover(function ()
                        {
                            $(this).removeClass().addClass('comboxoption1');
                        }, function ()
                        {
                            var cvalue = $("#" + id).val();
                            if (!cvalue || (',' + cvalue + ',').indexOf(',' + $(this).attr("value") + ',') == -1)
                            {
                                $(this).removeClass().addClass('comboxoption');
                            }
                        });
                        var $label = $('<label style="vertical-align:middle;" ' + (multiple ? 'for="checkbox_' + id + "_" + optionValue + '"' : '') + '>' + $options.eq(i).text() + '</label>');
                        $divoptions.append($label);
                        $div.append($divoptions);
                    }
                    $hide.val(values.join(','));
                    $input.val(texts.join(',')).attr("title", texts.join(','));
                    break;
                case "span":
                    var texts = [];
                    var values = [];
                    var $table = $(this).children("table");
                    var multiple = $(this).attr("multiple") && ("multiple" == $(this).attr("multiple").toLowerCase()
                        || "1" == $(this).attr("multiple") || "true" == $(this).attr("multiple").toLowerCase());
                    if (multiple)
                    {
                       
                    }
                    $("tbody tr", $table).each(function ()
                    {
                        var $td = $("td:first", $(this));
                        var $tdHtml = $td.html();
                        var selected = $td.attr("selected") && ("selected" == $td.attr("selected").toLowerCase() || "1" == $td.attr("selected") || "true" == $td.attr("selected").toLowerCase());
                        if (!selected)
                        {
                            selected = $td.attr("isselected") && "1" == $td.attr("isselected");
                        }
                        if (selected)
                        {
                            values.push($td.attr("value"));
                            texts.push($td.text());
                        }
                        if (multiple)
                        {
                            var $checkbox = $('<input ' + (selected ? 'checked="checked"' : '') + ' id="checkbox_' + id + '_' + $td.attr("value") + '" name="radio_' + id + '" type="checkbox" value="' + $td.attr("value") + '" style="vertical-align:middle;"/>');
                            var $label = $('<label for="checkbox_' + id + '_' + $td.attr("value") + '" style="vertical-align:middle;">' + $tdHtml + '</label>');
                            $checkbox.bind("click", function ()
                            {
                                instance.setValue($(this).val(), $(this).next().text(), id, true);
                            });
                            $td.html('').append($checkbox, $label);
                        }
                        else
                        {
                            var $radio = $('<input ' + (selected ? 'checked="checked"' : '') + ' id="radio_' + id + '_' + $td.attr("value") + '" name="radio_' + id + '" type="radio" value="' + $td.attr("value") + '" style="vertical-align:middle;"/>');
                            var $label = $('<label for="radio_' + id + '_' + $td.attr("value") + '" style="vertical-align:middle;">' + $tdHtml + '</label>');
                            $radio.bind("click", function ()
                            {
                                instance.setValue($(this).val(), $(this).next().text(), id, false);
                            });
                            $td.html('').append($radio, $label);
                        }
                    });
                    $div.append($table);
                    $hide.val(values.join(','));
                    $input.val(texts.join(',')).attr("title", texts.join(','));
                    $table.attr('border', '0');
                    $table.attr('cellpadding', '1');
                    $table.attr('cellspacing', '0');
                    $table.css('width', '100%');
                    $table.removeClass().addClass("listtable");
                    //$div.css("overflow", "hidden");
                    //new RoadUI.Grid({ table: $table, height: $div.height(), resizeCol:false });
                    break;
            }


            if (disabled)
            {
                $input.prop("disabled", true);
            }
            else
            {
                $input.bind("focus", function ()
                {
                    $div.show();
                }).bind("click", function ()
                {
                    $div.show();
                }).bind("blur", function ()
                {
                    $div.hover(function () { $div.show(); }, function () { $div.hide(); });
                });
            }
            $(this).remove();
        });
    };

    this.setValue = function (value, text, id, isMultiple)
    {
        var $valueele = $("#" + id);
        var $textele = $("#" + id + "_text");
        if (isMultiple)
        {
            var values = [];
            var txts = [];
            $("input:checked", $("#" + id + "_selectdiv")).each(function ()
            {
                values.push($(this).val());
                txts.push($(this).next().text());
            });
            value = values.join(',');
            text = txts.join(',');
        }
        $textele.val(text).attr("title", text);
        $valueele.val(value);
        if (!isMultiple)
        {
            $("#" + id + "_selectdiv").hide();
        }
        $valueele.change();
    };
}