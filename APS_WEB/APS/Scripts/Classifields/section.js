//$(function () {

    function TreeViewManager() {
        var _container;
        var _tree;

        function init(container) {
            _container = container;
            _tree = container.find(".tree");
            registerEvents();
            updateTree();

        }
        function updateTree() {
            var uri = "/api/section/getall";
            $.ajax({
                method: "GET",
                url: uri,
                success: function (data) {
                    createTreeView(data);
                },
                error: function (data) {
                    alert("false");
                }
            });
        }
        function createTreeView(sections) {
            function addSection(section, wrap) {
                if (wrap === undefined || false) {
                    return $("<li></li>").attr("data-path", section.Path)
                        .attr("data-parent", section.Parent)
                        .attr("data-child", section.Child)
                        .attr("data-id", section.Id);
                } else {
                    return $("<ul></ul>").append($("<li></li>").attr("data-path", section.Path)
                        .attr("data-parent", section.Parent)
                        .attr("data-child", section.Child)
                        .attr("data-id", section.Id));
                }
            }
            if (sections.lenght <= 0) {
                return false;
            }
            _tree.empty();
            _tree.append(addSection(sections[0],true));
            sections.shift();
            while (sections.length > 0) {
            sections = $.grep(sections, function (item) {
                var phasparent = _tree.find("li[data-parent='" + item.Parent + "']"),
                    phaschild = _tree.find("li[data-parent='" + item.Child + "']"),
                    chaschild = _tree.find("li[data-child='" + item.Parent + "']");
                    if (phasparent.length > 0) {
                        phasparent.parent().append(addSection(item));
                        return false;
                    } else if (phaschild.length > 0) {
                        phaschild.parent().wrap(addSection(item, true));
                        return false;
                    } else if (chaschild.length > 0) {
                        if (item.Child === "none") {
                            return false;
                        }
                        chaschild.append(addSection(item, true));
                        return false;
                    }
                    return true;
                });
            }
            _tree.wrapInner("<ul><li data-child='classifields'></li></ul>");
            
            var treeSections = _tree.find("li[data-child]");
            treeSections.each(function () {
                $(this).prepend("<span><i></i>" + Resources[$(this).attr("data-child").replace("-", "")] + "<span style='color:grey; border:0;'>(" + $(this).attr("data-child")+ ")</span></span>" +
                    "<button class='fa fa-plus btn btn-success btn-xs' add-section></button>" +
                    "<button class='fa fa-minus btn btn-danger btn-xs' delete-section></button>");
            });
            $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch')
                .find(' > i').addClass('icon-minus-sign');
        }
        function registerEvents() {
            _tree.on('click', 'li.parent_li > span', function (e) {
                var children = $(this).parent('li.parent_li').find(' > ul > li');
                if (children.is(":visible")) {
                    children.hide('fast');
                    $(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
                } else {
                    children.show('fast');
                    $(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
                }
                e.stopPropagation();
            });
            _container.on('click', '[collapse-sections]', function () {
                var expenders = _tree.find("li:has(ul) > span");

                expenders.each(function () {
                    var isExpanded = $(this).find("> i").hasClass("icon-minus-sign");
                    if (isExpanded) {
                        $(this).trigger("click");
                    }
                });
            });
            _container.on('click', '[expand-sections]', function () {
                var expenders = _tree.find("li:has(ul) > span");

                expenders.each(function () {
                    var isCollapsed = $(this).find("> i").hasClass("icon-plus-sign");
                    if (isCollapsed) {
                        $(this).trigger("click");
                    }
                });
            });
            _tree.on("click", "button[add-section]", function () {
                var section = $(this).closest("li"),
                    newchild;
                newchild = prompt("Enter section name");
                
                if (newchild === null) {
                    return false;
                }
                if (newchild === "") {
                    alert("Invalid Section name");
                    return false;
                }
                $("#sectionId").val(section.attr("data-id"));
                $("#sectionValue").val(newchild);
                $("[collapse-sections]").trigger("click");
                FillStaticData();
            });
            _tree.on("click", "button[delete-section]", function () {
                var section = $(this).closest("li");
                function deleteSection() {
                    var uri = "/api/section/check/" + section.attr("data-id");
                    $.ajax({
                        method: "GET",
                        url: uri,
                        success: function (data) {
                            if (data !== true) {
                                dSection();
                            } else {
                                alert("Denied");
                            }      
                        },
                        error: function (data) {
                            alert("false");
                        }
                    });
                    function dSection(){
                        var uri = "/api/section/delete/" + section.attr("data-id");
                        $.ajax({
                            method: "DELETE",
                            url: uri,
                            success: function (data) {
                                updateTree();
                            },
                            error: function (data) {
                                alert("false");
                            }
                        });
                    }
                }
                deleteSection();
            });
        }
        return {
            init: init,
            updateTree: updateTree
        }
    }
    function confirmSection() {
        function getStaticData() {
            var sectionDetails = $("#section-details"),
                columns = [],
                fields = [],
                ccolumns = sectionDetails.find("#columns input:checked"),
                cfields = sectionDetails.find("#fields input:checked");

            if (ccolumns.length > 0) {
                ccolumns.each(function () {
                    columns.push($(this).val());
                });
            }
            if (cfields.length > 0) {
                cfields.each(function () {
                    fields.push($(this).val());
                });
            }

            var data = {
                Columns: columns,
                Fields: fields
            };

            return data;
        }
        var Id = $("#sectionId").val(),
            newchild = $("#sectionValue").val(),
            data = {
                Id: Id,
                Child: newchild,
                Columns: getStaticData().Columns,
                Fields: getStaticData().Fields
            };

        var uri = "/api/section/add";
        $.ajax({
            method: "POST",
            url: uri,
            data: data,
            success: function (data) {
                $("#section-details").hide();
                treeV.updateTree();
            },
            error: function (data) {
                alert("false");
            }
        });
    }
    function FillStaticData() {

        function newCheckBox(value) {
            return $("<div class='checkbox'><label><input type='checkbox' value=" + value + ">" + Resources[value] + "</label></div >");
        }

        function getStaticData() {
            var uri = "/api/section/staticdata";
            $.ajax({
                method: "GET",
                url: uri,
                success: function (data) {
                    bindStaticData(data);
                },
                error: function (data) {
                    alert("false");
                }
            });
        }
        function bindStaticData(data) {
            var columnContainer = $("#columns"),
                fieldContainer = $("#fields"),
                fields = data.Fields,
                columns = data.Columns;

            columnContainer.empty();
            fieldContainer.empty();

            $.each(columns,function (i, value) {
                columnContainer.append(newCheckBox(value));
            });
            $.each(fields, function (i, value) {
                fieldContainer.append(newCheckBox(value));
            });
        }
        getStaticData();
        $("#section-details").show();

    }
    $("button[confirm-section]").on("click", function () {
        confirmSection();
    });
    $("#section-details").on("change", "input", function () {
        var selector = $(this),
            sectionDetails = selector.closest("#section-details");

        var inputs = sectionDetails.find("input[value=" + selector.attr("value") + "]");
        inputs.each(function () {
            if (!$(this).is(selector)) {
                if (selector.is(":checked")) {
                    if ($(this).closest("#fields").length > 0) {
                        $(this).prop("checked", true);
                    }
                    
                } else {
                    $(this).prop("checked", false);
                }
                
            }
        });
        console.log(inputs);
    });

    var treeV = new TreeViewManager();
    treeV.init($("#treeContainer"));
//});