$(function () {
    $('.select2').select2({
        placeholder: "", // Placeholder text
        allowClear: true
    });

    GetDivisionList();

    $("#lstDivision").on('change', function () {
        var items = '';
        $('.lstDivision option:selected').each(function (i) {
            items += $(this).val() + ','
        });

        $.ajax({
            url: $("#hf_GetPlantBrandByDivision").val(),
            type: "GET",
            data: { divisionIds: items },
            success: function (response) {
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
            error: function (e) {
                alert(e.responseText);
            }
        });
    });

    $("#lstPlant").on('change', function () {
        var items = '';
        $('.lstPlant option:selected').each(function (i) {
            items += $(this).val() + ','
        });

        $.ajax({
            url: $("#hf_GetNearestPlantByPlant").val(),
            type: "GET",
            data: { plantIds: items },
            success: function (response) {
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
            error: function (e) {
                alert(e.responseText);
            }
        });
    });

    $("#lstBrand").on('change', function () {
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
                }
            },
            error: function (e) {
                alert(e.responseText);
            }
        });
    });
});

function GetDivisionList() {
    $.ajax({
        url: $("#hf_GetDivisions").val(),
        type: "GET",
        success: function (response) {
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
        error: function (e) {
            alert(e.responseText);
        }
    });
}