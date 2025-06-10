$(function () {
    loadUsersData();
});

function loadUsersData() {
    Blockloadershow();
    $.ajax({
        url: $("#hf_GetUsersList").val(),
        type: 'GET',
        success: function (data) {
            Blockloaderhide();
            if (data && Array.isArray(data)) {
                OnDivGridLoad(data);
            }
            else {
                showDangerAlert('No data available to load.');
            }
        },
        error: function (xhr, status, error) {
            showDangerAlert('Error retrieving data: ' + error);
            Blockloaderhide();
        }
    });
}

//define column header menu as column visibility toggle
var headerMenu = function () {
    var menu = [];
    var columns = this.getColumns();

    for (let column of columns) {

        //create checkbox element using font awesome icons
        let icon = document.createElement("i");
        icon.classList.add("fas");
        icon.classList.add(column.isVisible() ? "fa-check-square" : "fa-square");

        //build label
        let label = document.createElement("span");
        let title = document.createElement("span");

        title.textContent = " " + column.getDefinition().title;

        label.appendChild(icon);
        label.appendChild(title);

        //create menu item
        menu.push({
            label: label,
            action: function (e) {
                //prevent menu closing
                e.stopPropagation();

                //toggle current column visibility
                column.toggle();

                //change menu item icon
                if (column.isVisible()) {
                    icon.classList.remove("fa-square");
                    icon.classList.add("fa-check-square");
                } else {
                    icon.classList.remove("fa-check-square");
                    icon.classList.add("fa-square");
                }
            }
        });
    }

    return menu;
};

function OnDivGridLoad(response) {
    Blockloadershow();

    tabledata = [];
    let columns = [];

    // Map the response to the table format
    if (response.length > 0) {
        $.each(response, function (index, item) {

            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Name: item.name,
                ADid: item.adid,
                Email: item.email,
                MobileNo: item.mobileNo,
                Designation: item.designation,
                Role: item.role
            });
        });

        columns.push(
            {
                title: "Action",
                field: "Action",
                responsive: 0,
                width: 120,
                headerMenu: headerMenu,
                hozAlign: "center",
                headerHozAlign: "center",
                formatter: function (cell, formatterParams) {
                    const rowData = cell.getRow().getData();
                    let actionButtons = "";

                    actionButtons += `<i data-toggle="modal" onclick="delConfirm(${rowData.Id})" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                    return actionButtons;
                }
            },
            {
                title: "SNo", field: "Sr_No", width: 100, responsive: 1, sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "left"
            },
            { title: "Name", field: "Name", responsive: 2, headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "ADid", field: "ADid", responsive: 3, headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "Email", field: "Email", responsive: 4, headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "MobileNo", field: "MobileNo", responsive: 5, headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "Designation", field: "Designation", responsive: 6, headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            { title: "Role", field: "Role", responsive: 7, headerMenu: headerMenu, headerFilter: "input", hozAlign: "left", headerHozAlign: "center" },
            
        );

        // // Initialize Tabulator
        table = new Tabulator("#div_Table", {
            data: tabledata,
            layout: "fitColumns",
            responsiveLayout: "collapse",
            responsiveLayoutCollapseFormatter: function (data) {
                return data.map(d => `<strong>${d.title}</strong>: ${d.value}`).join("<br>");
            },
            renderHorizontal: "virtual",
            movableColumns: true,
            pagination: "local",
            paginationSize: 10,
            paginationSizeSelector: [50, 100, 500, 1500, 2000],
            paginationCounter: "rows",
            paginationElement: document.getElementById("pager"),
            dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
            columns: columns
        });

        table.on("cellClick", function (e, cell) {
            let columnField = cell.getColumn().getField();

            if (columnField !== "Action") {
                let rowData = cell.getRow().getData();
                editUser(rowData.Id);
            }
        });
    }
    else {
        showDangerAlert('No data available.');
    }

    // Hide loader
    Blockloaderhide();
}

function delConfirm(recid) {
    PNotify.prototype.options.styling = "bootstrap3";
    (new PNotify({
        title: 'Confirmation Needed',
        text: 'Are you sure to delete? It will not delete if this record is used in transactions.',
        icon: 'glyphicon glyphicon-question-sign',
        hide: false,
        confirm: {
            confirm: true
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: {
            history: false
        },
    })).get().on('pnotify.confirm', function () {
        $.ajax({
            url: $("#hf_DeleteUser").cal(),
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("User Deleted successfully.");
                    setTimeout(function () {
                        window.location.reload();
                    }, 2500);
                }
                else {
                    showDangerAlert(data.message);
                }
            },
            error: function () {
                showDangerAlert('Error retrieving data.');
            }
        });
    }).on('pnotify.cancel', function () {
        loadData();
    });
}

function editUser(recid) {
    var url = $("#hf_UserDetailsPage").val() + "/" + recid;
    window.open(url, '_self');
}

function userDetailsPage() {
    var url = $("#hf_UserDetailsPage").val();
    window.open(url, '_self');
}