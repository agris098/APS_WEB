function ClassifieldManager() {
    var _tableContainer;
    var _table;
    var _tableColumns;

    function init(container, section) {
        _tableContainer = $(container);
        _tableColumns = section.Columns;
        createTable(section);
        _table = _tableContainer.find("table");
        updateTableRows(section.ID);
        registerEvents();
    }
    function createTable(data) {
        var table = $("<table class='table table-bordered' data-filter data-sort='asc'></table>").attr("data-id", data.ID);
        var thead = $("<thead></thead>");
        var theadtr = $("<tr></tr>");
        var tbody = $("<tbody></tbody>");
        var columns = data.Columns;

        for (var i = 0; i < columns.length; i++) {
            theadtr.append("<th data-sort data-filter=" + columns[i] + ">" + columns[i] + "<i></i></th>");
        }
        thead.append(theadtr);
        table.append(thead);
        table.append(tbody);
        _tableContainer.append(table);
    }

    function registerEvents() {
        _table.on("click", "thead th", function () {
            var filter = $(this);
            _table.attr("data-filter", filter.attr("data-filter"));
            if (filter.attr("data-sort") === "") {
                _table.find("thead[data-filter]").attr("data-sort", "");
                filter.attr("data-sort", "asc");
                _table.attr("data-sort", "asc");
            } else if (filter.attr("data-sort") === "asc") {
                filter.attr("data-sort", "desc");
                _table.attr("data-sort", "desc");
            } else if (filter.attr("data-sort") === "desc") {
                filter.attr("data-sort", "asc");
                _table.attr("data-sort", "asc");
            }
            _table.find("thead th").each(function () {
               if($(this).attr("data-filter") !== filter.attr("data-filter")){
                    $(this).attr("data-sort", "");
                }
            });
            updateTableRows(_table.attr("data-id"));
        });
        _table.on("click", "tbody tr", function () {
            window.location.href = "http://localhost:56616/classifield/" + $(this).attr("data-id");

        });

    }

    function updateTableRows(id) {
        var tbody = _tableContainer.find("tbody");
            tbody.empty();
            var filter = {
                Column: _table.attr("data-filter"),
                Order: _table.attr("data-sort")
            };

            $.ajax({
                url: "http://localhost:56616/api/classifields/all/" + id,
                data: filter,
                type: 'get',
                success: function (data) {
                    $.each(data, function () {
                        var tr = $("<tr data-id='" + this.Id + "'></tr>");
                        for (var i = 0; i < _tableColumns.length; i++) {
                            var val = _tableColumns[i];
                            tr.append("<td>" + this[val] + "</td>");
                        }
                        tbody.append(tr);
                    });
                }
            });

    }

    return {
        init: init
    }
}
var classifieldManager = new ClassifieldManager();
classifieldManager.init(".classifields-table", section);