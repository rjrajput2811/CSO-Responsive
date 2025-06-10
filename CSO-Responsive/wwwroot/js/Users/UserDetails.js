$(function () {
    $('.select2').select2({
        placeholder: "", // Placeholder text
        allowClear: true,
        width: '100%'
    });

    GetDivisionList();

    $("#lstDivision").on('change', function () {
        Blockloadershow();
        var items = '';
        $('.lstDivision option:selected').each(function (i) {
            items += $(this).val() + ','
        });

        $.ajax({
            url: $("#hf_GetPlantBrandByDivision").val(),
            type: "GET",
            data: { divisionIds: items },
            success: function (response) {
                Blockloaderhide();
                $('#lstPlant').empty();
                $('#lstBrand').empty();
                if (response.plantList.length > 0) {
                    response.plantList.forEach(function (item) {
                        var plantSelected = false;
                        var selectedPlantIdsData = selectedPlantIds.split(',')
                        for (var i = 0; i < selectedPlantIdsData.length; i++) {
                            if (selectedPlantIdsData[i] != "" && selectedPlantIdsData[i] == item.value) {
                                plantSelected = true;
                            }
                        }
                        var option = new Option(item.text, item.value, false, plantSelected);
                        if (item.disabled) option.disabled = true;
                        $('#lstPlant').append(option);
                    });

                    $('#lstPlant').trigger('change');
                }
                if (response.brandList.length > 0) {
                    response.brandList.forEach(function (item) {
                        var brandSelected = false;
                        var selectedBrandIdsData = selectedBrandIds.split(',')
                        for (var i = 0; i < selectedBrandIdsData.length; i++) {
                            if (selectedBrandIdsData[i] != "" && selectedBrandIdsData[i] == item.value) {
                                brandSelected = true;
                            }
                        }
                        var option = new Option(item.text, item.value, false, brandSelected);
                        if (item.disabled) option.disabled = true;
                        $('#lstBrand').append(option);
                    });

                    $('#lstBrand').trigger('change');
                }
            },
            error: function (xhr, status, error) {
                Blockloaderhide();
                showDangerAlert('Error retrieving data: ' + error);
            }
        });
    });

    $("#lstPlant").on('change', function () {
        Blockloadershow();
        var items = '';
        $('.lstPlant option:selected').each(function (i) {
            items += $(this).val() + ','
        });

        $.ajax({
            url: $("#hf_GetNearestPlantByPlant").val(),
            type: "GET",
            data: { plantIds: items },
            success: function (response) {
                Blockloaderhide();
                $('#lstNPlant').empty();
                if (response.length > 0) {
                    response.forEach(function (item) {
                        var nearestPlantSelected = false;
                        var selectedNearestPlantIdsData = selectedNearestPlantIds.split(',')
                        for (var i = 0; i < selectedNearestPlantIdsData.length; i++) {
                            if (selectedNearestPlantIdsData[i] != "" && selectedNearestPlantIdsData[i] == item.value) {
                                nearestPlantSelected = true;
                            }
                        }
                        var option = new Option(item.text, item.value, false, nearestPlantSelected);
                        if (item.disabled) option.disabled = true;
                        $('#lstNPlant').append(option);
                    });

                    $('#lstNPlant').trigger('change');
                }
                else {
                    $('#lstNPlant').prop("disabled", true);
                }
            },
            error: function (xhr, status, error) {
                Blockloaderhide();
                showDangerAlert('Error retrieving data: ' + error);
            }
        });
    });

    $("#lstBrand").on('change', function () {
        Blockloadershow();
        var items = '';
        $('.lstBrand option:selected').each(function (i) {
            items += $(this).val() + ','
        });

        $.ajax({
            url: $("#hf_GetProductTypeByBrand").val(),
            type: "GET",
            data: { brandIds: items },
            success: function (response) {
                $('#lstProductType').empty();
                if (response.length > 0) {
                    response.forEach(function (item) {
                        var productTypeSelected = false;
                        var selectedProductTypeIdsData = selectedProductTypeIds.split(',')
                        for (var i = 0; i < selectedProductTypeIdsData.length; i++) {
                            if (selectedProductTypeIdsData[i] != "" && selectedProductTypeIdsData[i] == item.value) {
                                productTypeSelected = true;
                            }
                        }
                        var option = new Option(item.text, item.value, false, productTypeSelected);
                        if (item.disabled) option.disabled = true;
                        $('#lstProductType').append(option);
                    });

                    $('#lstProductType').trigger('change');
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
});

function GetDivisionList() {
    Blockloadershow();
    $.ajax({
        url: $("#hf_GetDivisions").val(),
        type: "GET",
        success: function (response) {
            Blockloaderhide();
            $('#lstDivision').empty();
            if (response.length > 0) {
                response.forEach(function (item) {
                    var divisionSelected = false;
                    var selectedDivisionIdsData = selectedDivisionIds.split(',')
                    for (var i = 0; i < selectedDivisionIdsData.length; i++) {
                        if (selectedDivisionIdsData[i] != "" && selectedDivisionIdsData[i] == item.value) {
                            divisionSelected = true;
                        }
                    }
                    var option = new Option(item.text, item.value, false, divisionSelected);
                    if (item.disabled) option.disabled = true;
                    $('#lstDivision').append(option);
                });

                $('#lstDivision').trigger('change');
            }
        },
        error: function (xhr, status, error) {
            Blockloaderhide();
            showDangerAlert('Error retrieving data: ' + error);
        }
    });
}

function InsertUpdateUser() {
    Blockloadershow();
    const requiredFields = [
        { id: 'txtADid', name: 'ADid' },
        { id: 'txtName', name: 'Name' },
        { id: 'txtEmail', name: 'Email' },
        { id: 'txtMobile', name: 'Mobile No' },
        { id: 'ddlRole', name: 'Role' },
        { id: 'ddlUserType', name: 'User Type' },
        { id: 'lstDivision', name: 'Division' },
        { id: 'lstPlant', name: 'Plant' }, ,
        { id: 'lstBrand', name: 'Brand' },
        { id: 'lstProductType', name: 'Product Type' },
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

    $("#Id").val(userId);

    var divisions = '';
    $('.lstDivision option:selected').each(function (i) {
        divisions += $(this).val() + ','
    });

    var plants = '';
    $('.lstPlant option:selected').each(function (i) {
        plants += $(this).val() + ','
    });

    var nPlant = '';
    $('.lstNPlant option:selected').each(function (i) {
        nPlant += $(this).val() + ','
    });

    var brands = '';
    $('.lstBrand option:selected').each(function (i) {
        brands += $(this).val() + ','
    });

    var productTypes = '';
    $('.lstProductType option:selected').each(function (i) {
        productTypes += $(this).val() + ','
    });

    const form = $('#userForm')[0];
    const formData = new FormData(form);

    formData.append('DivisionId', divisions);
    formData.append('PlantId', plants);
    formData.append('NearestPlantId', nPlant);
    formData.append('BrandId', brands);
    formData.append('ProductTypeId', productTypes);

    $.ajax({
        url: $('#userForm').attr('action'),
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
                    showSuccessAlert("User Updated Successfully.");
                }
                else {
                    showSuccessAlert("User Saved Successfully.");
                    setTimeout(function () {
                        window.open($("#hf_UsersGridPage").val(), '_self');
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