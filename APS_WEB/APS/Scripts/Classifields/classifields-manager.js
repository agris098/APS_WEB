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
        var table = $("<table class='table table-bordered' id='draftTable'></table>").attr("data-id", data.ID);
        var thead = $("<thead></thead>");
        var theadtr = $("<tr></tr>");
        var tbody = $("<tbody></tbody>");
        var columns = data.Columns;

        if (jQuery.inArray("S_mpicture", _tableColumns) !== -1) {
            theadtr.append("<th></th>");
        }
        for (var i = 0; i < columns.length; i++) {
            if (columns[i] !== "S_mpicture") {
                theadtr.append("<th>" + columns[i] + "</th>");
            }
            
        }
        thead.append(theadtr);
        table.append(thead);
        table.append(tbody);
        _tableContainer.append(table);
    }

    function registerEvents() {
        _table.on("click", "tbody tr", function () {
            window.location.href = "http://localhost:56616/classifield/" + $(this).attr("data-id");
        });

    }

    function updateTableRows(id) {
        var tbody = _tableContainer.find("tbody");
            tbody.empty();

            $.ajax({
                url: "http://localhost:56616/api/classifields/all/" + id,
                type: 'get',
                success: function (data) {
                    var aa = 0
                    while (aa < 12) {
                        $.each(data, function () {
                            var tr = $("<tr data-id='" + this.Id + "'></tr>");
                            var hasPicture = false;
                            if (jQuery.inArray("S_mpicture", _tableColumns) !== -1) {
                                tr.append("<td><img src='data:image/jpg;base64," + this.S_mpicture + "'/></td>");
                                hasPicture = true;
                            }                           
                            for (var i = 0; i < _tableColumns.length; i++) {
                                var val = _tableColumns[i];
                                if (val !== "S_mpicture") {
                                    tr.append("<td>" + this[val] + "</td>");
                                }                             
                            }
                            tbody.append(tr);
                        });
                        aa++;
                    }
                    _table.DataTable();
                }
            });

    }

    return {
        init: init
    }
}
var classifieldManager = new ClassifieldManager();
classifieldManager.init(".classifields-table", section);


