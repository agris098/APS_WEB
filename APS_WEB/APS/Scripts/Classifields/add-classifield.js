$(document).on("mousedown", "select", function () {
    var selector = $(this);
    var uri = "http://localhost:56616/api/section/getall/parent/" + selector.attr("data-load-section");

    $.ajax({
        method: "GET",
        url: uri,
        success: function (data) {
            var sections = JSON.stringify(data);
            selector.find("option[value]").remove();
            console.log(data);
            $.each(data, function () {
                console.log(this.Path);
                var tpath = this.Path === "" || this.Path === undefined ? this.Child : this.Path + "/" + this.Child;
                selector.append("<option value='" + this.Child + "' data-section-path='" + tpath + "'>" + this.Child + "</option>");
            });
        },
        error: function (data) {
            alert("false");
        }
    });
});
$(document).on("change", "select", function (e) {
    var selector = $(this),
        path = selector.find(":selected").attr("data-section-path"),
        value = selector.find(":selected").attr("value"),
        selectorParent = selector.parent();
    if (!selectorParent.is(":last-child")) {
        selectorParent.nextAll().remove();
    }

    function makeChild() {
        var template = selector.parent().clone(),
            tempselector = template.find("select");
        tempselector.attr("data-load-section", value);
        tempselector.attr("data-section-path", path);
        selector.parent().parent().append(template);
        tempselector.trigger("mousedown");
    }
    var data = { Path: path },
        uri = "http://localhost:56616/api/section/haschildren/path";
    $.ajax({
        method: "POST",
        url: uri,
        data: data,
        success: function (data) {
            var selectedSectionRow = $("#SelectedSectionRow"),
                classifieldForm = $("#ClassifiedRow");

            selectedSectionRow.hide();
            classifieldForm.hide();
            if (data) {
                makeChild();
            } else {
                var sections = $(".section-selector select"),
                    path = "";

                sections.each(function () {
                    path += "/" + $(this).val();
                });

                selectedSectionRow.show();
                selectedSectionRow.find("#SelectedPath").html(path);
                classifieldForm.show();
            }

        },
        error: function (data) {
            alert("false");
        }
    });
});
$(document).on("ready", function () {
    $("select").trigger("mousedown");
});
$(document).on("submit", "#ClassifieldForm", function (e) {
    e.preventDefault();
    var data = $(this).serialize();
        data += "&Path=" + $("#SelectedPath").html().substring(1);
    var uri = "http://localhost:56616/api/classifields/add";

    $.ajax({
        method: "POST",
        url: uri,
        data: data,
        success: function () {
            alert("added");
        },
        error: function (data) {
            alert("false");
        }
    });
});