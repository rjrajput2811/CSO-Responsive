$(function () {
    setTimeout(function () {
        var divisionVal = $('#DivisionId').val();
        if (divisionVal) {
            $('#DivisionId').trigger('change');
        }
    }, 100);

    $("#DivisionId").on('change', function () {
        Blockloadershow();

        $.ajax({
            url: $("#hf_GetBrandListAndPlantListByDivisinAndUser").val(),
            type: "GET",
            data: { divisionId: $("#DivisionId").val() },
            success: function (response) {
                Blockloaderhide();
                $('#PlantId').empty();
                $('#PlantId').append($('<option>').val('').text('Select Plant'));
                $('#BrandId').empty();
                $('#BrandId').append($('<option>').val('').text('Select Brand'));
                if (response.plantList.length > 0) {
                    response.plantList.forEach(function (item) {
                        var option = $('<option>').val(item.value).text(item.text);
                        if ($('#PlantId').val() && $('#PlantId').val() == item.value) {
                            option.prop('selected', true);
                        }
                        $('#PlantId').append(option);
                    });

                    $('#PlantId').val($("#hf_SelectedPlantId").val());
                    $('#PlantId').trigger('change');
                }
                if (response.brandList.length > 0) {
                    response.brandList.forEach(function (item) {
                        var option = $('<option>').val(item.value).text(item.text);
                        if ($('#BrandId').val() && $('#PlantId').val() == item.value) {
                            option.prop('selected', true);
                        }
                        $('#BrandId').append(option);
                    });

                    $('#BrandId').val($("#hf_SelectedBrandId").val());
                    $('#BrandId').trigger('change');
                }
            },
            error: function (xhr, status, error) {
                Blockloaderhide();
                showDangerAlert('Error retrieving data: ' + error);
            }
        });
    });

    $("#BrandId").on('change', function () {
        Blockloadershow();
        var test = $("#BrandId").val();
        $.ajax({
            url: $("#hf_GetProductTypeListByBrandAndUser").val(),
            type: "GET",
            data: { brandId: $("#BrandId").val() },
            success: function (response) {
                $('#ProductTypeId').empty();
                $('#ProductTypeId').append($('<option>').val('').text('Select Product Type'));
                if (response.length > 0) {
                    response.forEach(function (item) {
                        var option = $('<option>').val(item.value).text(item.text);
                        if ($('#ProductTypeId').val() && $('#PlantId').val() == item.value) {
                            option.prop('selected', true);
                        }
                        $('#ProductTypeId').append(option);
                    });

                    $('#ProductTypeId').val($("#hf_SelectedProductTypeId").val());
                    $('#ProductTypeId').trigger('change');
                    Blockloaderhide();
                }
                else {
                    Blockloaderhide();
                }
            },
            error: function (xhr, status, error) {
                Blockloaderhide();
                showDangerAlert('Error retrieving data: ' + error);
            }
        });
    });

    $("#PlantId").on('change', function () {
        Blockloadershow();

        $.ajax({
            url: $("#hf_GetNearestPlantListByPlantAndUser").val(),
            type: "GET",
            data: { brandId: $("#BrandId").val() },
            success: function (response) {
                $('#NearestPlantId').empty();
                $('#NearestPlantId').append($('<option>').val('').text('Select Nearest Plant'));
                if (response.length > 0) {
                    response.forEach(function (item) {
                        var option = $('<option>').val(item.value).text(item.text);
                        if ($('#NearestPlantId').val() && $('#PlantId').val() == item.value) {
                            option.prop('selected', true);
                        }
                        $('#NearestPlantId').append(option);
                    });

                    $('#NearestPlantId').val($("#hf_SelectedNearestPlantId").val());
                    $('#NearestPlantId').trigger('change');
                    Blockloaderhide();
                }
                else {
                    $('#NearestPlantId').prop("disabled", true);
                    Blockloaderhide();
                }
            },
            error: function (xhr, status, error) {
                Blockloaderhide();
                showDangerAlert('Error retrieving data: ' + error);
            }
        });
    });
});

function InsertUpdateCSOLogAnay() {
    debugger;
    Blockloadershow();
    const requiredFields = [
        { id: 'ComplaintTypeId', name: 'Complaint Type' },
        { id: 'CSOClassId', name: 'CSO Class' },
        { id: 'RootCauseAnalysisDescription', name: 'RootCauseAnalysisDescription' },
        { id: 'CorrectiveActionDescription', name: 'CorrectiveActionDescription' },
        { id: 'PreventiveActionDescription', name: 'PreventiveActionDescription' },
        { id: 'Quantity', name: 'Quantity' },
    ];

    let isValid = true;
    let missingFields = [];

    requiredFields.forEach(field => {
        const element = $('#' + field.id);
        if (!element.val()) {
            isValid = false;
            missingFields.push(field.name);
            element.addClass('missing-field');
        }
    });

    if (!isValid) {
        const missingFieldsList = missingFields.map(field => `<li>${field}</li>`).join('');
        const alertMessage = `
                <p>Please fill out the following required field(s):</p>
                <ul>${missingFieldsList}</ul>
            `;
        showDangerAlert(alertMessage);
        Blockloaderhide();
        return false;
    }

    if ($("#Quantity").val() == '0') {
        showDangerAlert("Defective Quantity can't be 0.");
        Blockloaderhide();
        return false;
    }

    $("#Id").val(csoLogId);
    $("#FinancialYear").val($("#ddlFinancialYear").val());

    var files = $('#File')[0].files;

    if (files.length === 0 && csoLogId == 0) {
        showDangerAlert('Please select at least one file to upload.');
        Blockloaderhide();
        return false;
    }

    const form = $('#CSOLogAnay-form')[0];
    const formData = new FormData(form);

    const maxFileSizeMB = 5;
    const maxFileSizeBytes = maxFileSizeMB * 1024 * 1024;

    for (let i = 0; i < files.length; i++) {
        if (files[i].size > maxFileSizeBytes) {
            showDangerAlert(`File "${files[i].name}" exceeds the 5MB limit.`);
            return false;
        }
    }

    for (var i = 0; i < files.length; i++) {
        formData.append('files', files[i]);
    }

    $.ajax({
        url: $('#CSOLogAnay-form').attr('action'),
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            Blockloaderhide();
            if (!response.success) {
                var errorMessg = "";
                for (var error in response.errors) {
                    errorMessg += error + "\n";
                }
                if (errorMessg != "") {
                    showDangerAlert(errorMessg);
                }
                else {
                    showDangerAlert(response.message);
                }
                return false;
            }
            else {
                if ($("#Id").val() > 0) {
                    showSuccessAlert("CSO Log Updated Successfully.");
                }
                else {
                    showSuccessAlert("CSO Log Saved Successfully.");
                    setTimeout(function () {
                        window.open($("#hf_CSOLogGridPage").val(), '_self');
                    }, 2500);
                }
            }
        },
        error: function (xhr, status, error) {
            Blockloaderhide();
            showDangerAlert('Error saving data: ' + error);
        }
    });
}