$(document).on("mousedown", ".section-selector-container select", function () {
    var selector = $(this);
    var uri = "/api/section/getall/parent/" + selector.attr("data-load-section");

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
$(document).on("change", ".section-selector-container select", function (e) {
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
        uri = "/api/section/haschildren/path";
    $.ajax({
        method: "POST",
        url: uri,
        data: data,
        success: function (data) {
            var selectedSectionRow = $("#SelectedSectionRow"),
                classifieldForm = $("#ClassifiedRow"),
                sectionSelectorContainer = $(".section-selector-container");

            selectedSectionRow.hide();
            classifieldForm.hide();
            if (data) {
                makeChild();
            } else {
                var sections = $(".section-selector select"),
                    path = "";
                    pathToForm = "";

                sections.each(function () {
                    path += "<i class='fa fa-chevron-right arrow'></i>" + $(this).val();
                    pathToForm += "/" + $(this).val();
                });

                selectedSectionRow.show();
                selectedSectionRow.find("#SelectedPath").html(path);
                classifieldForm.find("#Path").val(pathToForm);
                classifieldForm.show();
                sectionSelectorContainer.hide();
                getSectionInfo(pathToForm);
            }

        },
        error: function (data) {
            alert("false");
        }
    });
});
$(document).ready( function () {
    $(".section-selector-container select").trigger("mousedown");
});
$(document).on("click", ".selected-section-info .change", function () {
    var sectionSelectorContainer = $(".section-selector-container"),
        selectedSectionRow = $("#SelectedSectionRow"),
        classifieldForm = $("#ClassifiedRow");

    sectionSelectorContainer.show();
    selectedSectionRow.hide();
    classifieldForm.hide();
    sectionSelectorContainer.find("select").first().trigger("change");
    
});
function getSectionInfo(path) {
    var uri = "/api/section/getbypath";
    var data = { Path: path };
    $.ajax({
        method: "POST",
        url: uri,
        data: data,
        success: function (data) {
            console.log(data);
            normalizeForm(data);
        },
        error: function () { }
    });
}
function normalizeForm(fields) {
    var form = $("#ClassifieldForm");

    form.find(".data-field").hide();

    $.each(fields, function (index, value) {
        form.find(".data-field." + value).show();
    });
}
/*
$(document).on("submit", "#ClassifieldForm", function (e) {
    e.preventDefault();
    var data = $(this).serialize();
    data += "&Path=" + $(this).find("#path").val().substring(1);
    var uri = "/api/classifields/add";

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
});*/