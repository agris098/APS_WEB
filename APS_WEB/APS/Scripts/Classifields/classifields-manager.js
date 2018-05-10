function ClassifieldManager() {
    var _tableContainer;
    var _table;
    var _tableColumns;
    var _modal = $("#prevClassified");

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

        theadtr.append("<th></th>");
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
    function updateTableRows(id) {
        var tbody = _tableContainer.find("tbody");
        tbody.empty();

        $.ajax({
            url: "http://localhost:56616/api/classifields/allpublished/" + id,
            type: 'get',
            success: function (data) {
                $.each(data, function () {
                    var tr = $("<tr data-id='" + this.Id + "'></tr>");
                    var hasPicture = false;
                    var hasMark = '';
                    var mark = "";
                    var logged = currentUser.Id && currentUser.Id !== this.S_userId ? true : false;
                    console.log(currentUser);
                    if (logged) {
                        if (this.Marks && this.Marks.indexOf(currentUser.Id) > -1) {
                            hasMark = 'true';
                        }
                        mark = "<i class='fa fa-star markClassified action-icon " + hasMark + "'></i>";
                    }

                    tr.append("<td><i class='fa fa-eye prevClassified action-icon'></i>"+mark+"</td>");
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
                _table.DataTable();
            }
        });

    }

    function registerEvents() {
        _table.on("click", "tbody tr", function () {
            window.location.href = "http://localhost:56616/classifield/" + $(this).attr("data-id");
        });
        _table.on('click', '.prevClassified', function (e) {
            e.stopPropagation();
            var cId = $(this).closest('tr').data('id'),
                modalCid = _modal.find('#c-id');

            modalCid.val(cId);
            getModalData(cId);
            _modal.modal('show');
        });
        _table.on('click', '.markClassified', function (e) {
            e.stopPropagation();
            var cId = $(this).closest('tr').data('id');
            var data = { Id: cId };
            var element = $(this);
            $.ajax({
                url: uriMarkClassified,
                type: 'POST',
                data: data,
                success: function (data) {
                    if (data.Status) {
                        element.addClass('true');
                    } else {
                        element.removeClass('true');
                    }

                    console.log(data);
                }
            });
        });
        _modal.on('show.bs.modal', function () {
        });
        _modal.on('click', '.description-head', function () {
            if ($('.description-content:hidden').length) {
                $('.description-content').slideDown();
              //  $('.main-c').slideUp();
            } else {
                $('.description-content').slideUp();
           //     $('.main-c').slideDown();
            }
        });
    }
    function bindModalData(data) {
        var currentId = data.Id;
        var prevElement = _table.find('[data-id=' + currentId + ']').prev();
        var nextElement = _table.find('[data-id=' + currentId + ']').next();
        var prevB = _modal.find(".btn-prev");
        var nextB = _modal.find(".btn-next");
        var pB = $('.paginate_button.previous');
        var nB = $('.paginate_button.next');
        var pBc = pB.hasClass('disabled');
        var nBc = nB.hasClass('disabled');

        prevB.removeClass('disabled');
        nextB.removeClass('disabled');

        _modal.find('.description-content').hide();
        _modal.find('.main-c').show();
        if (!prevElement.length && pBc) {
            prevB.addClass('disabled');
        }
        if (!nextElement.length && nBc) {
            nextB.addClass('disabled');
        }
        _modal.find("#c-id").val(data.Id);
        _modal.find("#price").html('Price: ' + data.S_price + ' €');
        _modal.find("#description").html(data.S_description);
            var imgContainer = _modal.find(".ubislider-inner");
            imgContainer.empty();
            if (data.S_pictures.length) {
                $.each(data.S_pictures, function (index, value) {
                    imgContainer.append("<li> <img src='data:image/jpg;base64," + value + "' /></li>");
                });
            }
            $('#slider').ubislider({
                arrowsToggle: true,
                type: 'ecommerce',
                autoSlideOnLastClick: true,
                modalOnClick: false,
                position: 'vertical'
            });
    }
    function getModalData(id) {
        $.ajax({
            url: "http://localhost:56616/api/classifields/" + id,
            type: 'get',
            success: function (data) {
                bindModalData(data);
            }
        });
    }
    function nextClassified() {
        var id = _modal.find('#c-id').val();
        var tableRow = _table.find('[data-id=' + id + ']');
        var nextid = tableRow.next().data('id');
        if (!nextid) {
            $('.paginate_button.next').click();
            setTimeout(function () {

                nextid = _table.find('tbody tr').first().data('id');
                getModalData(nextid);
            }, 100);
        } else {
            getModalData(nextid);
        }

        
    }
    function previousClassified() {
        var id = _modal.find('#c-id').val();
        var tableRow = _table.find('[data-id=' + id + ']');
        var previd = tableRow.prev().data('id');
        if (!previd) {
            $('.paginate_button.previous').click();
            setTimeout(function () {

                previd = _table.find('tbody tr').last().data('id');
                getModalData(previd);
            }, 100);
        } else {
            getModalData(previd);
        }
    }

    return {
        init: init,
        nextClassified: nextClassified,
        previousClassified: previousClassified
    }
}
var classifieldManager = new ClassifieldManager();
classifieldManager.init(".classifields-table", section);

